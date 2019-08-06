using AzureWebMonitor.Test.PageModel.AzureDotCom;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Shouldly;
using System;

namespace AzureWebMonitor.Test
{
    [TestClass]
    public class AzureDotCom_AzureSignalR
    {
        static IWebDriver _driver;
        static ApplicationInsights _appInsights;

        static string _url = "http://azure.com";

        [TestMethod]
        public void Azure_SignalR_Pricing()
        {
            try
            {
                _driver = WebDriverHelper.GetDriver();
                _appInsights = new ApplicationInsights(ConfigurationHelper.GetApplicationConfiguration().InstrumentationKey, _url);
                
                var fiveSecondWait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                
                _driver.Navigate().GoToUrl(_url);

                var home = new Home(_driver, fiveSecondWait);
                
                _appInsights.Success("Home");
                
                var pricing = home.ClickPricing();

                pricing.Search("signalr");

                var signalRPricing = pricing.ClickSearchResult("Azure SignalR Service");

                _appInsights.Success("Pricing");

                signalRPricing.SelectRegion("Australia East");

                signalRPricing.SelectCurrency("Australian Dollar ($)");

                signalRPricing.Prices["Price / Unit / Day"].ShouldBe("$2.2106");
            
                signalRPricing.Prices["Max Units"].ShouldBe("100");
            }
            catch (Exception e)
            {
                _appInsights.Error(e);
            }

        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _driver.Dispose();
        }
    }
}
