using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AzureWebMonitor.Test.PageModel.AzureDotCom
{
    public class Home 
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private IWebElement PricingLink => _driver.FindElement(By.LinkText("Pricing"));

        public Home(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public Pricing ClickPricing()
        {
            PricingLink.Click();

            return new Pricing(_driver, _wait);
        }
    }
}