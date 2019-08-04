using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
            var rules = string.Join(',', _blockUrls.Select(r => $"MAP {r} 127.0.0.1"));
            options.AddArgument($"--host-resolver-rules={rules}");
            options.AddArgument("--headless");
            return new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);
        }
    }
}