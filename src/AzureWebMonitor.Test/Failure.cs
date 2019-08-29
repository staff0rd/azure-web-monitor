using System;
using OpenQA.Selenium;

namespace AzureWebMonitor.Test
{
    public class Failure
    {
        public string ScreenshotPath { get; set; }

        public Exception Exception { get; set; }

        public string Url { get; set; }

        public string Action { get; set; }

        public TimeSpan Duration { get; set; }
    }
}