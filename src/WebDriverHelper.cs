using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AzureWebMonitor.Test
{
    public static class WebDriverHelper 
    {
        public static IWebDriver GetDriver() 
        {
            var options = new ChromeOptions();
            options.AddArgument("--host-resolver-rules=MAP www.google-analytics.com 127.0.0.1");
            options.AddArgument("--headless");
            return new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);
        }
    }
}