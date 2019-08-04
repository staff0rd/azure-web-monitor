using AzureWebMonitor.Test.PageModel.AzureDotCom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Shouldly;

namespace AzureWebMonitor.Test
{
    [TestClass]
    public class Given_StaffordwilliamsDotCom_When_loading_home_page
    {
        static IWebDriver _driver;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _driver = WebDriverHelper.GetDriver();
        
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
