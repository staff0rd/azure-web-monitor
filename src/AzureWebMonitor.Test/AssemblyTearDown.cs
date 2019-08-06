
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureWebMonitor.Test
{
    [TestClass]
    public class Teardown
    {
        [AssemblyCleanup]
        static public void AssemblyCleanup()
        {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill(); // nasty hack to stop hanging chrome drivers keeping the build agent running
            }
        }
    }
}