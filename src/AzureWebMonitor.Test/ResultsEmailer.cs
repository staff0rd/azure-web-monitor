using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AzureWebMonitor.Test
{
    public class ResultsEmailer
    {
        const string template = @"<html><body>{0}</body></html>";

        private readonly Failure[] _failures;

        public ResultsEmailer(IEnumerable<Failure> failures)
        {
            _failures = failures.ToArray();   
        }

        public void Process(string outputPath)
        {
            if (_failures.Any())
            {
                var content = "";
                foreach ( var fail in _failures)
                {
                    content += "<div>";
                    content += $"<h1>{fail.Url} - {fail.Action} after {fail.Duration:mm}m{fail.Duration:ss}s</h1>";
                    content += $"<pre>{fail.Exception}</pre>";
                    content += $"<img src='{fail.ScreenshotPath}' />";
                    content += "</div>";
                }
                File.WriteAllText(outputPath, string.Format(template, content));

                Email(outputPath);
            }
        }

        private void Email(string outputPath)
        {
            // create mail message
            // insert html
            // attach screenshots
        }
    }
}