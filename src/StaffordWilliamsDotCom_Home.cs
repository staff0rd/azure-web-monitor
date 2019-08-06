using System;
using AzureWebMonitor.Test.PageModel.AzureDotCom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Shouldly;

namespace AzureWebMonitor.Test
{
    [TestClass]
    public class StaffordWilliamsDotCom_Home
    {
        static IWebDriver _driver;
        static ApplicationInsights _appInsights;
        

        [TestMethod]
        public void Blog_HomeLoads()
        {
            var url = "https://staffordwilliams.com";
            _appInsights = new ApplicationInsights(ConfigurationHelper.GetApplicationConfiguration().InstrumentationKey, url);
            try
            {
                _driver = WebDriverHelper.GetDriver();
                _driver.Navigate().GoToUrl(url);
                var homeLink = _driver.FindElement(By.LinkText("stafford williams"));
                homeLink.ShouldNotBeNull();
                _appInsights.Success("Home");
            }
            catch (Exception e)
            {
                _appInsights.Error(e);
                throw;
            } 
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _driver.Dispose();
            _appInsights.Dispose();
        }
    }
}
