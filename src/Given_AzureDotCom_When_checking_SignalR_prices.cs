using AzureWebMonitor.Test.PageModel.AzureDotCom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Shouldly;
using System;
using System.IO;
using System.Reflection;

namespace AzureWebMonitor.Test
{
    [TestClass]
    public class Given_AzureDotCom_When_checking_SignalR_prices
    {
        static IWebDriver _driver;
        static SignalRPricing _signalRPricing;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _driver = WebDriverHelper.GetDriver();
            var fiveSecondWait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            
            _driver.Navigate().GoToUrl(@"http://azure.com");

            var home = new Home(_driver, fiveSecondWait);
            
            var pricing = home.ClickPricing();

            pricing.Search("signalr");

            _signalRPricing = pricing.ClickSearchResult("Azure SignalR Service");

            _signalRPricing.SelectRegion("Australia East");

            _signalRPricing.SelectCurrency("Australian Dollar ($)");
        }


        [TestMethod]
        public void Then_price_should_be_expensive()
        {
            _signalRPricing.Prices["Price / Unit / Day"].ShouldBe("$2.2106");
        }

        [TestMethod]
        public void Then_max_unit_count_should_be_unchanged()
        {
            _signalRPricing.Prices["Max Units"].ShouldBe("100");
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _driver.Dispose();
        }
    }
}
