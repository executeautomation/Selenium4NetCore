using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using System;
using System.Threading;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V89.DevToolsSessionDomains;
using Network = OpenQA.Selenium.DevTools.V89.Network;


/// <summary>
/// Author: Karthik KK
/// Owner : ExecuteAutomation
/// </summary>
namespace Selenium4NetCoreProj
{
    public class Tests
    {

        protected IDevToolsSession session;
        protected IWebDriver driver;
        protected DevToolsSessionDomains devToolsSession;


        [SetUp]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            //Set ChromeDriver
            driver = new ChromeDriver();
            //Get DevTools
            IDevTools devTools = driver as IDevTools;
            //DevTools Session
            session = devTools.GetDevToolsSession();

            devToolsSession = session.GetVersionSpecificDomains<DevToolsSessionDomains>();
            devToolsSession.Network.Enable(new Network.EnableCommandSettings());
        }

        /// <summary>
        /// Network interception testing with .NET Core
        /// </summary>
        [Test]
        public void NetworkInterception()
        {
            devToolsSession.Network.SetBlockedURLs(new Network.SetBlockedURLsCommandSettings()
            {
                Urls = new string[] { "*://*/*.css", "*://*/*.jpg", "*://*/*.png" }
            });

            driver.Url = "https://www.executeautomation.com";
        }


        [Test]
        public void SetAddionalHeaders()
        {
            var extraHeader = new Network.Headers();
            extraHeader.Add("headerName", "executeHacked");
            devToolsSession.Network.SetExtraHTTPHeaders(new Network.SetExtraHTTPHeadersCommandSettings()
            {
                Headers = extraHeader
            });

            driver.Url = "https://www.executeautomation.com";
        }

        [Test]
        public void SetUserAgent()
        {
            devToolsSession.Network.SetUserAgentOverride(new Network.SetUserAgentOverrideCommandSettings()
            {
                UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko)"
            });

            ManualResetEventSlim requestSync = new ManualResetEventSlim(false);
            EventHandler<Network.RequestWillBeSentEventArgs> requestWillBeSentHandler = (sender, e) =>
            {
                Assert.That(e.Request.Headers["User-Agent"], Does.Contain("CPU iPhone OS"));
                requestSync.Set();
            };

            devToolsSession.Network.RequestWillBeSent += requestWillBeSentHandler;

            driver.Url = "https://www.executeautomation.com";
            requestSync.Wait(TimeSpan.FromSeconds(6));

        }
    }
}