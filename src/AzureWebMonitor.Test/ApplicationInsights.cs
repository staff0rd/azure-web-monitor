using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace AzureWebMonitor.Test
{
    public class ApplicationInsights : IDisposable
    {
        private readonly TelemetryClient _client;

        private readonly string _url;

        private string _lastAction = "None";

        private readonly Stopwatch _stopwatch;

        public ApplicationInsights(string instrumentationKey, string url)
        {
            _client = new TelemetryClient(new TelemetryConfiguration(instrumentationKey));
            _url = url;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void Success(string action)
        {
            _client.TrackAvailability(action, DateTimeOffset.Now, _stopwatch.Elapsed, Environment.MachineName, true, properties: GetProperties(action));
            _stopwatch.Restart();
        }

        public void Error(Exception e)
        {
            var action = $"After-{_lastAction}";
            _client.TrackAvailability(action, DateTimeOffset.Now, _stopwatch.Elapsed, Environment.MachineName, false, properties: GetProperties(action));
            _client.TrackException(e, GetProperties(action));
        }

        private IDictionary<string, string> GetProperties(string action)
        {
            return new Dictionary<string, string> {
                {"url", _url},
                {"action", action}
            };
        }

        public void Dispose()
        {
            _client.Flush();
        }
    }
}