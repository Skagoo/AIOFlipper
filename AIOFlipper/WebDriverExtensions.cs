using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AIOFlipper
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            try
            {
                if (timeoutInSeconds > 0)
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                    return wait.Until(ExpectedConditions.ElementToBeClickable(by));
                }
                return driver.FindElement(by);
            }
            catch (Exception)
            {
                bool hasInternetConnectivity = CheckInternetConnectivity();
                int internetCheckingDurationInSeconds = 600;

                while (!hasInternetConnectivity && internetCheckingDurationInSeconds > 0)
                {
                    hasInternetConnectivity = CheckInternetConnectivity();
                    internetCheckingDurationInSeconds--;
                }

                if (hasInternetConnectivity)
                {
                    try
                    {
                        return driver.FindElement(by);
                    }
                    catch (NoSuchElementException e)
                    {
                        // Check if the currentAccount has been disconnected. If so throw new DisconnectedFromRSCompanionException.
                        if (CheckAccountDisconnected(driver))
                        {
                            throw new DisconnectedFromRSCompanionException();
                        }
                        else
                        {
                            throw e;
                        }
                    }

                }
                else
                {
                    throw new NoSuchElementException();
                }
            }
        }
        private static bool CheckInternetConnectivity()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool CheckAccountDisconnected(IWebDriver driver)
        {
            return driver.Url.Contains("login");
        }
    }
}
