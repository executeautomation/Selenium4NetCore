using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;

/// <summary>
/// Author: Karthik KK
/// Owner : ExecuteAutomation
/// </summary>
namespace Selenium4NetCoreProj
{
    public class Tests
    {

        protected DevToolsSession session;
        protected IWebDriver driver;

        [SetUp]
        public void Setup()
        {

            //Set ChromeDriver
            driver = new ChromeDriver();
            //Get DevTools
            IDevTools devTools = driver as IDevTools;
            //DevTools Session
            session = devTools.CreateDevToolsSession();
        }

        /// <summary>
        /// Network interception testing with .NET Core
        /// </summary>
        [Test]
        public void NetworkInterception()
        {
            session.Network.Enable(new OpenQA.Selenium.DevTools.Network.EnableCommandSettings());

            session.Network.SetBlockedURLs(new OpenQA.Selenium.DevTools.Network.SetBlockedURLsCommandSettings()
            {
                Urls = new string[] { "*://*/*.css", "*://*/*.jpg", "*://*/*.png" }
            });

            driver.Url = "http://www.executeautomation.com";
        }
    }
}