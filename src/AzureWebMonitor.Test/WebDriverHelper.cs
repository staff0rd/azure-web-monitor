using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace AzureWebMonitor.Test
{
    public static class WebDriverHelper 
    {
        static readonly string[] _blockUrls = new [] {
            "www.google-analytics.com"
        };

        public static ChromeDriver GetDriver() 
        {
            var options = new ChromeOptions();
            var urls = GetUrls();
            var rules = string.Join(',', urls.Select(r => $"MAP {r} 127.0.0.1"));
            options.AddArgument($"--host-resolver-rules={rules}");
            options.AddArgument("--headless");
            return new ChromeDriver(ConfigurationHelper.WorkingDirectory, options);
        }

        private static string[] GetUrls()
        {
            var shouldForceFailure = new Random((int)DateTime.Now.Ticks).Next(10) == 0;
            if (shouldForceFailure)
                return _blockUrls.Union(new [] { "staffordwilliams.com"}).ToArray();
            else
                return _blockUrls;
        }
    }
}