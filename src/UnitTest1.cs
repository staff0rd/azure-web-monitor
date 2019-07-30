using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
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
                // var link = driver.FindElement(By.PartialLinkText("TFS Test API"));
                // var jsToBeExecuted = $"window.scroll(0, {link.Location.Y});";
                // ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted);
                // var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                // var clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.PartialLinkText("TFS Test API")));
                // clickableElement.Click();
            }
        }
    }
}
