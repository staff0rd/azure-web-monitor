using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;

namespace AzureWebMonitor.Test.PageModel.AzureDotCom
{
    public class Pricing
    {
        private readonly IWebDriver _driver;
        private readonly OpenQA.Selenium.Support.UI.WebDriverWait _wait;

        private IWebElement PricingByProductLink => _driver.FindElement(By.LinkText("Pricing by product"));

        private IWebElement SearchBox => _driver.FindElement(By.Id("searchPicker-input"));

        public Pricing(IWebDriver driver, OpenQA.Selenium.Support.UI.WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        internal void Search(string searchString)
        {
            Actions actions = new Actions(_driver);
            actions.MoveToElement(SearchBox);
            actions.Perform();
            SearchBox.SendKeys(searchString);
        }

        public SignalRPricing ClickSearchResult(string result)
        {
            var clickableElement = _wait.Until(ExpectedConditions.ElementToBeClickable(By.PartialLinkText(result)));
            clickableElement.Click();
            return new SignalRPricing(_driver);
        }
    }
}