using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using SeleniumExtras.WaitHelpers;
using Shouldly;
using System;
using System.IO;
using System.Reflection;

namespace AzureWebMonitor.Test
{
    [TestClass]
    public class Given_AzureDotCom_When_checking_SignalR_prices
    {
        static RemoteWebDriver _driver;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), new ChromeOptions { });
            _driver.Navigate().GoToUrl(@"http://azure.com");

            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Pricing")));
            clickableElement.Click();

            clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Pricing by product")));
            clickableElement.Click();

            var searchBox = _driver.FindElement(By.Id("searchPicker-input"));
            Actions actions = new Actions(_driver);
            actions.MoveToElement(searchBox);
            actions.Perform();

            searchBox.SendKeys("signalr");

            clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.PartialLinkText("Azure SignalR Service")));
            clickableElement.Click();

            OpenQA.Selenium.Support.UI.SelectElement select = new OpenQA.Selenium.Support.UI.SelectElement(_driver.FindElement(By.Id("region-selector")));
            select.SelectByText("Australia East");

            select = new OpenQA.Selenium.Support.UI.SelectElement(_driver.FindElementByClassName("currency-selector"));
            select.SelectByText("Australian Dollar ($)");
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _driver.Dispose();
        }

        [TestMethod]
        public void Then_price_should_be_expensive()
        {
            var price = GetPriceTableValue("Price / Unit / Day");
            price.ShouldBe("$2.2106");
        }

        [TestMethod]
        public void Then_max_unit_count_should_be_unchanged() {
            var maxUnitCount = GetPriceTableValue("Max Units");
            maxUnitCount.ShouldBe("100");
        }

        private static string GetPriceTableValue(string label)
        {
            var rows = _driver.FindElementsByTagName("tr");
            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells.Count > 0 && cells[0].Text == label)
                {
                    return cells[2].Text;
                }
            }
            return "";
        }
    }
}
