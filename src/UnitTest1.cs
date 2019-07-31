using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Reflection;

namespace AzureWebMonitor.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), new ChromeOptions { }))
            {
                driver.Navigate().GoToUrl(@"http://azure.com");
                //var link = driver.FindElement(By.PartialLinkText("TFS Test API"));
                // var jsToBeExecuted = $"window.scroll(0, {link.Location.Y});";
                // ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted);
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Pricing")));
                clickableElement.Click();

                clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Pricing by product")));
                clickableElement.Click();

                var searchBox = driver.FindElement(By.Id("searchPicker-input"));
                Actions actions = new Actions(driver);
                actions.MoveToElement(searchBox);
                actions.Perform();

                searchBox.SendKeys("signalr");

                clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.PartialLinkText("Azure SignalR Service")));
                clickableElement.Click();







            }
        }
    }
}
