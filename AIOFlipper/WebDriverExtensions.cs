using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

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
                    catch (WebDriverException)
                    {
                        // Most likely means we where disconnected anyways
                        throw new DisconnectedFromRSCompanionException();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }
                else
                {
                    throw new NoSuchElementException();
                }
            }
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds, string valueAttributeText)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(by));
                wait.Until(ExpectedConditions.TextToBePresentInElement(element, valueAttributeText));

                return element;
            }
            catch (Exception)
            {
                int internetCheckingDurationInSeconds = 600;

                while (!CheckInternetConnectivity() && internetCheckingDurationInSeconds > 0)
                {
                    internetCheckingDurationInSeconds--;
                }

                if (CheckInternetConnectivity())
                {
                    // Check if the currentAccount has been disconnected. If so throw new DisconnectedFromRSCompanionException.
                    if (CheckAccountDisconnected(driver))
                    {
                        throw new DisconnectedFromRSCompanionException();
                    }
                    else
                    {
                        throw new NoSuchElementException();
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
            try
            {
                if (driver.Url.Contains("login") || driver.Url.Contains("unavailable"))
                {
                    return true;
                }
                return false;
                
            }
            catch (WebDriverException)
            {
                throw new DisconnectedFromRSCompanionException();
            }
        }

        public static IWebDriver WindowForceBackground(this ITargetLocator targetLocator, string windowName)
        {
            IWebDriver driver = targetLocator.Window(windowName);

            try
            {
                foreach (Process process in SetWindowPosition.GetPrimaryProcesses("chrome"))
                {
                    if (process.Id != 1816)
                    {
                        SetWindowPosition.ForceWindowToStayOnBottom(process);
                        Console.WriteLine("Successfully pushed back a chrome window with PID: " + process.Id);
                    }
                    else
                    {
                        Console.WriteLine("Left chrome window with PID " + process.Id + " untouched.");
                    }
                }

                Console.WriteLine("Successfully pushed back the chrome windows.");
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to push back the chrome windows.");
            }

            return driver;
        }
    }
}
