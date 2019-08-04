using AzureWebMonitor.Test.PageModel.AzureDotCom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Shouldly;
using System.IO;
using System.Reflection;

namespace AzureWebMonitor.Test
{
    [TestClass]
    public class Given_StaffordwilliamsDotCom_When_loading_home_page
    {
        static RemoteWebDriver _driver;
        static SignalRPricing _signalRPricing;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), new ChromeOptions { });
        
            _driver.Navigate().GoToUrl(@"https://staffordwilliams.com");
        }

        [TestMethod]
        public void Then_loads()
        {
            var homeLink = _driver.FindElement(By.LinkText("stafford williams"));
            homeLink.ShouldNotBeNull();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _driver.Dispose();
        }
    }
}
