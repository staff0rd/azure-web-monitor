using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AzureWebMonitor.Test
{
    public static class WebDriverHelper 
    {
        static readonly string[] _blockUrls = new [] {
            "www.google-analytics.com"
        };

        static readonly List<Failure> _failures = new List<Failure>();

        public static Failure[] Failures => _failures.ToArray();

        public static ChromeDriver GetDriver() 
        {
            var options = new ChromeOptions();
            var urls = GetUrls();
            var rules = string.Join(',', urls.Select(r => $"MAP {r} 127.0.0.1"));
            options.AddArgument($"--host-resolver-rules={rules}");
            //options.AddArgument("--headless");
            return new ChromeDriver(ConfigurationHelper.WorkingDirectory, options);
        }

        private static string[] GetUrls()
        {
            var shouldForceFailure = false;
            if (shouldForceFailure)
                return _blockUrls.Union(new [] { "staffordwilliams.com"}).ToArray();
            else
                return _blockUrls;
        }

        public static void RecordFailure(string url, string action, IWebDriver driver, Exception exception, TimeSpan duration)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var path = Path.GetTempFileName();
            screenshot.SaveAsFile(path);
            _failures.Add(new Failure { Url = url, Action = action, ScreenshotPath = path, Exception = exception, Duration = duration });
        }
    }
}