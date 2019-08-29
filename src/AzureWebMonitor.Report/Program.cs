using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AzureWebMonitor.Report
{
    class Program
    {
        class TimeSeries
        {
            public DateTimeOffset TimeStamp { get; set; }
            public double Count { get; set; }
        }
        static void Main(string[] args)
        {
            new[] { "1d", "7d", "28d" }
            .ToList()
            .ForEach((ago) => WriteExceptions(ago));

            new[] { "1d", "7d", "28d" }
            .ToList()
            .ForEach((ago) => WriteAvailability(ago, $"{ago}.png", "Page load times"));
        }

        private static void WriteExceptions(string ago)
        {
            var query = $"exceptions | where timestamp > ago({ago}) | make-series count() default=0 on timestamp in range(ago({ago}), now(), 3h) by type";
            var result = GetTelemetry(query);

            JArray rows = JsonConvert.DeserializeObject<dynamic>(result).tables[0].rows;

            if (rows.Count == 0)
                return;

            var row = JsonConvert.DeserializeObject<dynamic>(result).tables[0].rows[0];

            string row1 = row[1];
            string row2 = row[2];

            var counts = JsonConvert.DeserializeObject<int[]>(row1);
            var timeStamps = JsonConvert.DeserializeObject<DateTimeOffset[]>(row2);

            var timeSeries = counts.Select((c, i) => new TimeSeries { Count = c, TimeStamp = timeStamps[i] });
            var series = new Series();
            //series.ChartType = SeriesChartType.StackedBar;
            timeSeries.ToList().ForEach(s => series.Points.AddXY(s.TimeStamp.LocalDateTime, s.Count));
            WriteGraph($"exceptions-{ago}.png", $"Exceptions ({ago})", "Count", new[] { series }.ToList(), false);
        }


        private static void WriteAvailability(string ago, string fileName, string title)
        {
            var query = $"availabilityResults | where timestamp > ago({ago}) | where success == 1 | project timestamp, duration/1000, name, url=customDimensions['url'] | order by timestamp desc";
            WriteGraph(fileName, $"{title} ({ago})", query);
        }

        private static void WriteGraph(string fileName, string title, string query)
        {
            var result = GetTelemetry(ConfigurationManager.AppSettings["ApplicationInsightsId"], ConfigurationManager.AppSettings["ApplicationInsightsApiKey"], query);
            var json = JsonConvert.DeserializeObject<dynamic>(result);
            var rows = new List<Row>();

            foreach (var row in json.tables[0].rows)
            {
                rows.Add(new Row { TimeStamp = row[0], Count = row[1], Action = row[2], Url = row[3] });
            }

            var groups = rows.GroupBy(k => new { url = k.Url, action = k.Action }, g => new { g.TimeStamp, g.Count });

            var series = new List<Series>();
            foreach (var group in groups)
            {
                var newSeries = new Series($"{group.Key.url}/{group.Key.action}");
                var points = group.OrderBy(p => p.TimeStamp);
                foreach (var item in points)
                {
                    newSeries.Points.AddXY(item.TimeStamp.LocalDateTime, item.Count);
                }
                newSeries.ChartType = SeriesChartType.Line;
                newSeries.IsVisibleInLegend = true;
                series.Add(newSeries);
            }

            WriteGraph(fileName, title, "Seconds", series);
        }

        private static void WriteGraph(string fileName, string title, string yAxisTitle, List<Series> series, bool showLegend = true)
        {
            var chart = new Chart();
            series.ForEach(chart.Series.Add);
            chart.Size = new System.Drawing.Size(1200, 300);
            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.LabelStyle.Format = "dd/MM/yyyy\nhh:mm tt";
            chartArea.AxisY.Title = yAxisTitle;

            chart.ChartAreas.Add(chartArea);
            if (showLegend)
                chart.Legends.Add(new Legend());
            chart.Titles.Add(new Title { Text = title });

            chart.SaveImage(fileName, ChartImageFormat.Png);
        }

        interface ITimeSeries
        {
            DateTimeOffset TimeStamp { get; }

            object XAxis {  get; }
        }

        class Row : TimeSeries
        { 
            public string Action { get; set; }
            public string Url { get; set; }
        }

        public static string GetTelemetry(string query)
        {
            return GetTelemetry(ConfigurationManager.AppSettings["ApplicationInsightsId"], ConfigurationManager.AppSettings["ApplicationInsightsApiKey"], query);
        }

        public static string GetTelemetry(string appid, string apikey, string query)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", apikey);
            var req = $"https://api.applicationinsights.io/v1/apps/{appid}/query?query={query}";
            HttpResponseMessage response = client.GetAsync(req).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return response.ReasonPhrase;
            }
        }
    }
}
