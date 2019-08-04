using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace AzureWebMonitor.Test.PageModel.AzureDotCom
{
    public class SignalRPricing
    {
        private readonly IWebDriver _driver;

        private SelectElement Currency => new SelectElement(_driver.FindElement(By.ClassName("currency-selector")));

        private SelectElement Region => new SelectElement(_driver.FindElement(By.Id("region-selector")));

        public Dictionary<string, string> Prices => _driver.FindElements(By.TagName("tr"))
                .Select(row => row.FindElements(By.TagName("td")))
                .Where(cells => cells.Count == 3)
                .ToDictionary(k => k[0].Text, e => e[2].Text);

        public SignalRPricing(IWebDriver driver)
        {
            _driver = driver;
        }

        public void SelectCurrency(string currency)
        {
            Currency.SelectByText(currency);
        }

        public void SelectRegion(string region)
        {
            Region.SelectByText(region);
        }
    }
}