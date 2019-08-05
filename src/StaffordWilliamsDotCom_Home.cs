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

        [TestMethod]
        public void Blog_HomeLoads()
        {
            _driver = WebDriverHelper.GetDriver();
            _driver.Navigate().GoToUrl(@"https://staffordwilliams.com");
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
