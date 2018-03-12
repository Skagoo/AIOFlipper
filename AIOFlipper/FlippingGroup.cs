using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Protractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIOFlipper
{
    class FlippingGroup
    {
        public NgWebDriver Driver { get; set; }
        public ChromeOptions Options { get; set; }
        public Account[] Accounts { get; set; }

        public FlippingGroup(ChromeOptions options, Account[] accounts)
        {
            Options = options;
            Accounts = accounts;
        }

        public void InitializeWebdriver()
        {
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            IWebDriver chromeDriver = new ChromeDriver(driverService, Options, Timeout.InfiniteTimeSpan);

            // Configure timeouts (important since Protractor uses asynchronous client side scripts)
            chromeDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(5);

            // Initialize the NgWebDriver
            Driver = new NgWebDriver(chromeDriver);
        }
    }
}
