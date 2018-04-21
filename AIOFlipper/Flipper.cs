using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TwoFactorAuthNet;
using System.Text.RegularExpressions;
using NLog;
using Protractor;
using OpenQA.Selenium.Support.UI;
using System.Net;
using Newtonsoft.Json.Linq;

namespace AIOFlipper
{
    class Flipper
    {
        public FlippingGroup[] flippingGroups;
        public FlippingGroup currentFlippingGroup;

        public NgWebDriver currentDriver;

        private Account currentAccount;

        CouchPortal couchPortal;

        private const int timeBeforePriceUpdate = 15;

        private int timeBeforePriceUpdateLong = 25;

        // Constuctor
        public Flipper(FlippingGroup[] flippingGroups)
        {
            this.flippingGroups = flippingGroups;
            couchPortal = new CouchPortal();
        }

        // Methods
        private void AbortSlot(Slot slot)
        {
            try
            {
                string slotOpenElementCsss = ((string)Program.Elements["elements"][2]["grand_exchange_page"][0]["slot"][15]["css_selector"]).Replace("SLOT_NUMBER", slot.Number.ToString());
                string offerAbortButtonElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][24]["css_selector"];
                string offerAbortConfirmOkElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][25]["css_selector"];
                string offerAbortAcknowledgedOkElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][27]["css_selector"];
                string offerCollectionSlot1ElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][28]["css_selector"];
                string offerCollectionSlot2ElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][29]["css_selector"];
                string slotItemNameElementCsss = ((string)Program.Elements["elements"][2]["grand_exchange_page"][0]["slot"][9]["css_selector"]).Replace("SLOT_NUMBER", slot.Number.ToString());
                string offerItemNameElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][30]["css_selector"];

                // Wait for the slot with the correct item name to be visible
                currentDriver.FindElement(By.CssSelector(slotItemNameElementCsss), 20, slot.ItemName);

                // Open the slot.
                IWebElement slotOpenElement = currentDriver.FindElement(By.CssSelector(slotOpenElementCsss), 20);
                slotOpenElement.Click();

                // Check if the correct slot was opened, else try again.
                currentDriver.FindElement(By.CssSelector(offerItemNameElementCsss), 20, slot.ItemName);

                try
                {
                    // Click the "Abort" button.
                    IWebElement offerAbortButtonElement = currentDriver.FindElement(By.CssSelector(offerAbortButtonElementCsss), 20);
                    offerAbortButtonElement.Click();

                    // Click the "Ok" button to confirm the offer abortion.
                    IWebElement offerAbortConfirmOkElement = currentDriver.FindElement(By.CssSelector(offerAbortConfirmOkElementCsss), 20);
                    offerAbortConfirmOkElement.Click();

                    // Click the "Ok" button on the "abort request acknowledged" pop-up.
                    IWebElement offerAbortAcknowledgedOkElement = currentDriver.FindElement(By.CssSelector(offerAbortAcknowledgedOkElementCsss), 20);
                    offerAbortAcknowledgedOkElement.Click();
                }
                catch (NoSuchElementException e)
                {
                    // Slot was already aborted, so just collect
                }

                IWebElement offerCollectionSlot1Element;

                try
                {
                    // Try to find the first collection slot
                    offerCollectionSlot1Element = currentDriver.FindElement(By.CssSelector(offerCollectionSlot1ElementCsss), 5);

                    // Try to find the second collection slot
                    IWebElement offerCollectionSlot2Element = currentDriver.FindElement(By.CssSelector(offerCollectionSlot2ElementCsss), 0);
                    offerCollectionSlot2Element.Click();
                }
                catch (Exception e)
                {
                    // There was only one collection slot.
                }

                // Try to find the first collection slot
                offerCollectionSlot1Element = currentDriver.FindElement(By.CssSelector(offerCollectionSlot1ElementCsss), 5);
                offerCollectionSlot1Element.Click();

                // For safety try collecting the first slot again
                try
                {
                    offerCollectionSlot1Element = currentDriver.FindElement(By.CssSelector(offerCollectionSlot1ElementCsss), 0);
                    offerCollectionSlot1Element.Click();
                }
                catch (Exception)
                {
                    // There was only one collection slot.
                }

                // This is just for safety, normaly we would be at the grand exchange page already.
                OpenBank();
                OpenGrandExchange();
            }
            catch (DisconnectedFromRSCompanionException e)
            {
                // Reconnect
                Reconnect();

                // Open Grand Exchange
                OpenGrandExchange();

                // Retry aborting the slot (Recursive call)
                AbortSlot(slot);
            }
            catch (NoSuchElementException e)
            {
                // Open Bank
                OpenBank();

                // Open Grand Exchange
                OpenGrandExchange();

                // Retry aborting the slot (Recursive call)
                AbortSlot(slot);
            }
            catch (StaleElementReferenceException e)
            {
                // Open Bank
                OpenBank();

                // Open Grand Exchange
                OpenGrandExchange();

                // Retry aborting the slot (Recursive call)
                AbortSlot(slot);
            }
            catch (InvalidOperationException e)
            {
                // Open Bank
                OpenBank();

                // Open Grand Exchange
                OpenGrandExchange();

                // Retry aborting the slot (Recursive call)
                AbortSlot(slot);
            }
        }

        private void BuyItem(Slot slot)
        {
            try
            {
                string slotBuyButtonElementCsss = ((string)Program.Elements["elements"][2]["grand_exchange_page"][0]["slot"][7]["css_selector"]).Replace("SLOT_NUMBER", slot.Number.ToString());
                string buySearchElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][2]["css_selector"];
                string buySearchResultListElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][3]["css_selector"];
                string buySearchResultItemElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][6]["css_selector"];

                string offerCollectionSlot1ElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][28]["css_selector"];
                string offerCollectionSlot2ElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][29]["css_selector"];

                IWebElement buySearchResultItemElement = null;

                // Click the "Buy" button of the slot.
                IWebElement slotBuyButtonElement = currentDriver.FindElement(By.CssSelector(slotBuyButtonElementCsss), 20);
                slotBuyButtonElement.Click();

                bool itemFound = false;
                int attemps = 1;
                do
                {
                    try
                    {
                        // Enter the item name in the search bar.
                        IWebElement buySearchElement = currentDriver.FindElement(By.CssSelector(buySearchElementCsss), 20);
                        buySearchElement.Clear();
                        buySearchElement.SendKeys(slot.ItemName);

                        // Wait for list to appear (slow)
                        currentDriver.FindElement(By.CssSelector(buySearchResultItemElementCsss.Replace("CHILD_NUMBER", "1")), 10);

                        // Find the correct item in the list.
                        // Sometimes no result shows up in the list due to a bug in the RuneScape Companion web app.
                        string searchResultItemName = "";

                        int i = 1; // CHILD_NUMBER is not zero-based.
                        do
                        {
                            buySearchResultItemElement = currentDriver.FindElement(By.CssSelector(buySearchResultItemElementCsss.Replace("CHILD_NUMBER", i.ToString())), 10);
                            searchResultItemName = buySearchResultItemElement.Text;
                            i++;
                        } while (searchResultItemName != slot.ItemName);

                        // Set the itemFound varriable to true to escape the while loop.
                        itemFound = true;
                    }
                    catch (DisconnectedFromRSCompanionException)
                    {
                        throw new DisconnectedFromRSCompanionException();
                    }
                    catch (Exception)
                    {
                        // The item was not present. Making a new attempt to enter the name in the search bar.
                        Console.WriteLine("Error in searching for item. Attempt: " + attemps);
                        attemps++;
                    }
                } while (!itemFound && attemps <= 5);

                if (attemps <= 5)
                {
                    // Found the item in the list, click on it to open the buying page.
                    buySearchResultItemElement.Click();

                    string offerQuantityElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][9]["css_selector"];
                    string offerPricePerItemElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][10]["css_selector"];
                    string offerPopupConfirmOkElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][16]["css_selector"];

                    // Fill in the quantity.
                    IWebElement offerQuantityElement = currentDriver.FindElement(By.CssSelector(offerQuantityElementCsss), 20);
                    offerQuantityElement.Clear();

                    offerQuantityElement.SendKeys("1");
                    // Replace with | offerQuantityElement.SendKeys(slot.Item.BuyLimit.toString());

                    IWebElement offerPricePerItemElement = currentDriver.FindElement(By.CssSelector(offerPricePerItemElementCsss), 20);
                    do
                    {
                        offerPricePerItemElement.Clear();
                        offerPricePerItemElement.SendKeys(slot.Value.ToString());
                        //Thread.Sleep(500);

                        // Replacement for above sleep
                        currentDriver.WaitForAttribute(offerPricePerItemElement, "value", slot.Value.ToString(), 1);

                    } while (offerPricePerItemElement.GetAttribute("value") != slot.Value.ToString());

                    offerPricePerItemElement.SendKeys(Keys.Enter);

                    try
                    {
                        IWebElement offerPopupConfirmOkElement = currentDriver.FindElement(By.CssSelector(offerPopupConfirmOkElementCsss), 15);
                        offerPopupConfirmOkElement.Click();
                    }
                    catch (Exception)
                    {
                        // The popup was not present. Making a new attempt to buy the item.
                        Console.WriteLine("Error in buying item. Confirmation popup not showing.");

                        OpenGrandExchange();

                        // Recursive call
                        BuyItem(slot);

                        // Exit the method
                        return;
                    }

                    // Check the collection slots to check for INB (instant buy)
                    IWebElement offerCollectionSlot1Element;
                    bool INB;
                    try
                    {
                        offerCollectionSlot1Element = currentDriver.FindElement(By.CssSelector(offerCollectionSlot1ElementCsss), 5);
                        // INB
                        INB = true;
                    }
                    catch (Exception)
                    {
                        // No INB
                        INB = false;
                    }

                    if (INB)
                    {
                        // Check the price it bought for and apply rule:
                        string offerTotalPriceElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][23]["css_selector"];

                        IWebElement offerTotalPriceElement = currentDriver.FindElement(By.CssSelector(offerTotalPriceElementCsss), 10);
                        string offerTotalPriceRaw = offerTotalPriceElement.Text.Replace(" ", "").Replace(",", "").Replace("gp", "");
                        offerTotalPriceRaw = Regex.Replace(offerTotalPriceRaw, @"\D", "");

                        long offerTotalPrice = 0;
                        try
                        {
                            offerTotalPrice = 25000 * (long.Parse(offerTotalPriceRaw) / 25000);
                        }
                        catch (FormatException)
                        {
                            offerTotalPrice = slot.Value - 50000;
                        }

                        if (offerTotalPrice == 0)
                        {
                            // This is a bug with the RuneScape Companion website.
                            offerTotalPrice = slot.Value - 50000;
                        }

                        // Apply Flipchat rule:
                        // Reports labelled with 'INB' require you to reduce the buy price of the reported item by 50k from the reported price.
                        // Change only with 25K, since the slot will be evaluated as NIB later on the next slotCheck, and will thus be changed with the other 25K.
                        // Assign the offerTotalPrice value to slotValue
                        slot.Value = offerTotalPrice - 25000;

                        Item item = slot.GetItem();
                        item.CurrentBuyPrice = slot.Value;

                        // Update the item
                        UpdateItem(item);

                        // Open the GE
                        OpenBank();
                        OpenGrandExchange();
                    }
                    else
                    {
                        // Open the GE
                        OpenBank();
                        OpenGrandExchange();
                    }
                }
                else
                {
                    // Failed, reconnect
                    throw new DisconnectedFromRSCompanionException();
                }


            }
            catch (DisconnectedFromRSCompanionException)
            {
                Reconnect();
                OpenGrandExchange();
                BuyItem(slot);
            }
            catch (InvalidOperationException)
            {
                OpenBank();
                OpenGrandExchange();
                BuyItem(slot);
            }
            catch (NoSuchElementException)
            {
                OpenBank();
                OpenGrandExchange();
                BuyItem(slot);
            }
        }

        private string CheckSlotState(long slotNumber)
        {
            try
            {
                string slotStateElementCsss = ((string)Program.Elements["elements"][2]["grand_exchange_page"][0]["slot"][16]["css_selector"]).Replace("SLOT_NUMBER", slotNumber.ToString());

                IWebElement slotStateElement = currentDriver.FindElement(By.CssSelector(slotStateElementCsss), 20);

                // This switch value filters the slot state out of the attribute.
                // e.g.
                // attribute: slot clearfix ng-scope empty
                // Reverse(): ytpme epocs-gn xifraelc tols
                // Split(' ')[0]: ytpme
                // Reverse(): empty
                // ToString() is placed behind the Reverse methods, because Reverse returns a character array.
                string slotState = new string(new string(slotStateElement.GetAttribute("class").ToCharArray().Reverse().ToArray()).Split(' ')[0].ToCharArray().Reverse().ToArray());
                string completeOrAborted = new string(new string(slotStateElement.GetAttribute("class").ToCharArray().Reverse().ToArray()).Split(' ')[1].ToCharArray().Reverse().ToArray());
                if (completeOrAborted == "complete" || completeOrAborted == "aborted")
                {
                    slotState = completeOrAborted + " " + slotState;
                }

                return slotState;
            }
            catch (DisconnectedFromRSCompanionException)
            {
                Reconnect();
                OpenGrandExchange();
                return CheckSlotState(slotNumber);
            }
            catch (WebDriverException)
            {
                Reconnect();
                OpenGrandExchange();
                return CheckSlotState(slotNumber);
            }
        }

        private bool CheckUpdateItemAndSlot(long slotNumber)
        {
            // Use slotNumber - 1. slotNumber is not zero-based. The array is.
            // The returned boolean states whether the slot's value's have changed.
            Slot slot = currentAccount.Slots[slotNumber - 1];
            Item item = slot.GetItem();

            switch (slot.SlotState)
            {
                case "empty":
                    {
                        // The slot does not contain an item yet, so the assigned value of slotValue will be incorrect for sure.
                        // Assign the Item's currentBuyPrice to the slotValue.
                        slot.Value = item.CurrentBuyPrice;
                        currentAccount.Slots[slotNumber - 1] = slot;

                        return true;
                    }
                case "aborted buying":
                    {
                        return false;
                    }
                case "aborted selling":
                    {
                        return false;
                    }
                case "buying":
                    {
                        bool changed = false;
                        if ((DateTime.Now - slot.Time).TotalMinutes >= timeBeforePriceUpdateLong)
                        {
                            // The item is trying to be bought for the slotValue for more than the allowed timeBeforePriceUpdate.
                            // Increase the slotValue with the priceIncrementValue.
                            // Add 1 to the pricingRule.
                            slot.Value = slot.Value + item.PriceIncrementValue;
                            slot.PricingRule++;

                            currentAccount.Slots[slotNumber - 1] = slot;

                            changed = true;
                        }

                        if (slot.Value < item.CurrentBuyPrice)
                        {
                            // The slotValue is lower than Item's currentBuyPrice. The currentBuyPrice is the adviced price to buy the item for.
                            // Change the slotValue of the item to the currentBuyPrice and update the currentAccount's slot.
                            // Reset the pricingRule to 0.
                            slot.Value = item.CurrentBuyPrice;
                            slot.PricingRule = 0;

                            currentAccount.Slots[slotNumber - 1] = slot;

                            changed = true;
                        }

                        // If margin gets lower than the minimalMargin of the Item,
                        // increase the currentSellPrice of the item with minimalMargin to keep margins ok.
                        if (item.CurrentSellPrice - slot.Value < item.MinimalMargin)
                        {
                            item.CurrentSellPrice = item.CurrentSellPrice + item.MinimalMargin;
                            Console.WriteLine("Increased item.CurrentSellPrice to keep margins: " + item.CurrentSellPrice);
                            UpdateItem(item);
                        }

                        if (slot.PricingRule >= 5)
                        {
                            // The item is trying to be bought for more than RX5.
                            // Increase the slotValue with the priceIncrementValue.
                            // Add 1 to the pricingRule.
                            slot.Value = slot.Value + item.PriceIncrementValue;
                            slot.PricingRule++;

                            currentAccount.Slots[slotNumber - 1] = slot;

                            changed = true;
                        }

                        return changed;
                    }
                case "selling":
                    {
                        bool changed = false;
                        if ((DateTime.Now - slot.Time).TotalMinutes >= timeBeforePriceUpdateLong)
                        {
                            // The item is trying to be sold for the slotValue for more than the allowed timeBeforePriceUpdate.
                            // Decrease the slotValue with the priceDecrementValue.
                            // Add 1 to the pricingRule.
                            slot.Value = slot.Value - item.PriceDecrementValue;
                            slot.PricingRule++;

                            currentAccount.Slots[slotNumber - 1] = slot;

                            changed = true;
                        }

                        else if (slot.Value > item.CurrentSellPrice)
                        {
                            // The slotValue is higher than Item's currentSellPrice. The currentSellPrice is the adviced price to sell the item for.
                            // Change the slotValue of the item to the currentSellPrice and update the currentAccount's slot.
                            // Reset the pricingRule to 0.
                            slot.Value = item.CurrentSellPrice;
                            slot.PricingRule = 0;

                            currentAccount.Slots[slotNumber - 1] = slot;

                            changed = true;
                        }

                        // If margin gets lower than the minimalMargin of the Item,
                        // Decrease the currentBuyPrice of the item with MinimalMargin to keep margins ok.
                        if (slot.Value - item.CurrentBuyPrice < item.MinimalMargin)
                        {
                            item.CurrentBuyPrice = item.CurrentBuyPrice - item.MinimalMargin;
                            Console.WriteLine("Decreased item.CurrentBuyPrice to keep margins: " + item.CurrentBuyPrice);
                            UpdateItem(item);
                        }

                        if (slot.PricingRule >= 5)
                        {
                            // The item is trying to be sold for more than RX5.
                            // Decrease the slotValue with the PriceIncrementValue.
                            // Add 1 to the pricingRule.
                            slot.Value = slot.Value - item.PriceIncrementValue;
                            slot.PricingRule++;

                            currentAccount.Slots[slotNumber - 1] = slot;

                            changed = true;
                        }

                        return changed;
                    }
                case "complete buying":
                    {
                        // Apply Flipchat rule:
                        // Reports labelled with 'NIB' require you to reduce the buy price of the reported item by 25k from the reported price.
                        item.CurrentBuyPrice = slot.Value - 25000;
                        UpdateItem(item);

                        // Set the slots boughtFor property to the slots Value.
                        slot.BoughtFor = slot.Value;

                        return true;
                    }
                case "complete selling":
                    {
                        // Apply Flipchat rule:
                        // Reports labelled with 'NIS' require you to increase the sell price of the reported item by 25k from the reported price.
                        item.CurrentSellPrice = slot.Value + 25000;
                        UpdateItem(item);

                        // Set the slots boughtFor property to the slots Value.
                        slot.SoldFor = slot.Value;

                        return true;
                    }
                default:
                    return false;
            }
        }

        private void CheckUpdateProxyAuthIp()
        {
            string apiKey = Environment.GetEnvironmentVariable("PROXY_API_KEY");
            string authIP = "";
            string publicIP = "";
            string updateResponse = "";

            WebClient wc = new WebClient();

            authIP = wc.DownloadString("https://api.myprivateproxy.net/v1/fetchAuthIP/" + apiKey).Replace("\"", "").Replace("[", "").Replace("]", "");
            publicIP = wc.DownloadString("http://icanhazip.com").Replace("\n", "");

            if (authIP != publicIP)
            {
                updateResponse = wc.UploadString("https://api.myprivateproxy.net/v1/updateAuthIP/" + apiKey, new JArray(publicIP).ToString());
            }

            wc.Dispose();
        }

        private void CollectSlot(Slot slot)
        {
            try
            {
                string slotOpenElementCsss = ((string)Program.Elements["elements"][2]["grand_exchange_page"][0]["slot"][15]["css_selector"]).Replace("SLOT_NUMBER", slot.Number.ToString());
                string offerCollectionSlot1ElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][28]["css_selector"];
                string offerCollectionSlot2ElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][29]["css_selector"];

                string slotItemNameElementCsss = ((string)Program.Elements["elements"][2]["grand_exchange_page"][0]["slot"][9]["css_selector"]).Replace("SLOT_NUMBER", slot.Number.ToString());
                string offerItemNameElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][30]["css_selector"];

                // Wait for the slot with the correct item name to be visible
                currentDriver.FindElement(By.CssSelector(slotItemNameElementCsss), 20, slot.ItemName);

                // Open the slot.
                IWebElement slotOpenElement = currentDriver.FindElement(By.CssSelector(slotOpenElementCsss), 20);
                slotOpenElement.Click();

                //Thread.Sleep(1000);
                // Disabled the above since it might not need to be replaced.

                // Sleep to prevent collecting other slots
                // Check if the correct slot was opened, else try again.
                currentDriver.FindElement(By.CssSelector(offerItemNameElementCsss), 20, slot.ItemName);

                IWebElement offerCollectionSlot1Element;
                try
                {
                    offerCollectionSlot1Element = currentDriver.FindElement(By.CssSelector(offerCollectionSlot1ElementCsss), 5);
                    IWebElement offerCollectionSlot2Element = currentDriver.FindElement(By.CssSelector(offerCollectionSlot2ElementCsss), 0);
                    offerCollectionSlot2Element.Click();
                }
                catch (Exception)
                {
                    // There was only one collection slot.
                }

                offerCollectionSlot1Element = currentDriver.FindElement(By.CssSelector(offerCollectionSlot1ElementCsss), 5);
                offerCollectionSlot1Element.Click();



            }
            catch (DisconnectedFromRSCompanionException)
            {
                Reconnect();
                OpenGrandExchange();
                CollectSlot(slot);
            }
            catch (NoSuchElementException)
            {
                // Error finding the element, load the bank and GE again and retry
                OpenBank();
                OpenGrandExchange();
                CollectSlot(slot);
            }
            catch (InvalidProgramException)
            {
                // Error finding the element, load the bank and GE again and retry
                OpenBank();
                OpenGrandExchange();
                CollectSlot(slot);
            }
            catch (StaleElementReferenceException)
            {
                OpenBank();
                OpenGrandExchange();
                CollectSlot(slot);
            }
        }

        private string GetAbortedItemName(Slot slot)
        {
            try
            {
                string slotItemNameElementCsss = ((string)Program.Elements["elements"][2]["grand_exchange_page"][0]["slot"][9]["css_selector"]).Replace("SLOT_NUMBER", slot.Number.ToString());

                // Remove sleep since it is not needed
                Thread.Sleep(500);

                IWebElement slotItemNameElement = currentDriver.FindElement(By.CssSelector(slotItemNameElementCsss), 20);
                return slotItemNameElement.Text;
            }
            catch (DisconnectedFromRSCompanionException)
            {
                Reconnect();
                OpenGrandExchange();
                return GetAbortedItemName(slot);
            }
        }

        private Item GetItemToBuy(Slot slot)
        {
            // Check if the previous item is already bought the max allowed times.
            // If not return the previous item and leave the tracker unchanged.
            // If it did reach the limit, try to get a new item and reset the tracker to 0.
            //Item previousItem = slot.GetItem();
            //if (slot.BuyLimitTracker < previousItem.BuyLimit)
            //{
            //    return previousItem;
            //}
            //else
            //{
            //    if (currentAccount.GetAvailableItems().Count > 0)
            //    {
            //        // Get first item in availableItems.
            //        // The list was sorted so the item with the highest margin would be first.
            //        slot.BuyLimitTracker = 0;
            //        return currentAccount.GetAvailableItems().First();
            //    }
            //}

            List<Item> availableItems = currentAccount.GetAvailableItems();
            if (availableItems.Count > 0)
            {
                // Get first item in availableItems.
                // The list was sorted so the item with the highest margin would be first.
                slot.BuyLimitTracker = 0;

                // Randomise taking a top margin one, and a random one to keep margins up to date.
                Random random = new Random();
                if (random.Next(0, 4) != 0) return availableItems.First();
                else return availableItems[random.Next(0, availableItems.Count)];
            }

            return null;
        }

        private long GetMoneyPouchValue()
        {
            try
            {
                string moneyPouchElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][0]["slot"][17]["css_selector"];

                IWebElement moneyPouchElement;
                string moneyPouchElementValue;
                do
                {
                    moneyPouchElement = currentDriver.FindElement(By.CssSelector(moneyPouchElementCsss), 5);

                    moneyPouchElementValue = Regex.Replace(moneyPouchElement.Text, @"\D", "");

                    if (moneyPouchElementValue == "")
                    {
                        // This is a bug in the RuneScape Compannion Webapp.
                        // Problem: Money pouch not showing up.
                        // Cause: Unknown, but usually only tends to happen when first loading the "Grand Exchange" page.
                        // Solution/Workaround: Switch between the "Bank" page and the "Grand Exchange" page until the money pouch shows up.
                        OpenBank();
                        OpenGrandExchange();
                    }

                } while (moneyPouchElementValue == "");

                return long.Parse(Regex.Replace(moneyPouchElement.Text, @"\D", ""));
            }
            catch (DisconnectedFromRSCompanionException)
            {
                Reconnect();
                OpenGrandExchange();
                return GetMoneyPouchValue();
            }
        }

        private long GetSlotsValue()
        {
            long slotsValue = 0;

            foreach (Slot slot in currentAccount.Slots)
            {
                if (slot.SlotState == "buying")
                {
                    slotsValue = slotsValue + slot.Value;
                }
                else if (slot.SlotState == "selling")
                {
                    slotsValue = slotsValue + slot.BoughtFor;
                }
            }

            return slotsValue;
        }

        private void GoToRuneScapeCompanionPage(Account account)
        {
            CheckUpdateProxyAuthIp();
            currentDriver.WrappedDriver.Navigate().GoToUrl(String.Format("https://secure.runescape.com/m=world{0}/html5/comapp/", account.World));
        }

        private void Login(Account account, int attempt = 1)
        {
            if (attempt <= 3)
            {
                string usernameElementCsss = (string)Program.Elements["elements"][0]["login_form"][0]["css_selector"];
                string passwordElementCsss = (string)Program.Elements["elements"][0]["login_form"][1]["css_selector"];
                string authElementCsss = (string)Program.Elements["elements"][0]["login_form"][2]["css_selector"];
                string savePasswordNoElementCsss = (string)Program.Elements["elements"][0]["login_form"][3]["css_selector"];

                string geTabElementCsss = (string)Program.Elements["elements"][1]["menu_tabs"][0]["css_selector"];

                try
                {
                    IWebElement usernameElement = currentDriver.WrappedDriver.FindElement(By.CssSelector(usernameElementCsss), 30);

                    try
                    {
                        IWebElement passwordElement = currentDriver.WrappedDriver.FindElement(By.CssSelector(passwordElementCsss), 0);

                        usernameElement.SendKeys(account.Email);
                        passwordElement.SendKeys(account.Password);
                        passwordElement.SendKeys(Keys.Enter);

                        TwoFactorAuth tfa = new TwoFactorAuth();

                        IWebElement authElement = currentDriver.WrappedDriver.FindElement(By.CssSelector(authElementCsss), 20);
                        authElement.SendKeys("" + tfa.GetCode(account.AuthKey));
                        authElement.SendKeys(Keys.Enter);

                        IWebElement savePasswordNoElement = currentDriver.WrappedDriver.FindElement(By.CssSelector(savePasswordNoElementCsss), 20);
                        savePasswordNoElement.Click();

                        // Success
                        currentAccount.ConnectionRefused = false;
                        UpdateAccount(currentAccount);
                    }
                    catch (DisconnectedFromRSCompanionException)
                    {
                        currentAccount.ConnectionRefused = true;
                        UpdateAccount(currentAccount);
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        currentDriver.WrappedDriver.Navigate().Refresh();
                        Login(account, attempt + 1);
                    }
                    catch (WebDriverException)
                    {
                        Reconnect();
                    }
                }
            }
            else
            {
                currentAccount.ConnectionRefused = true;
                UpdateAccount(currentAccount);
            }
        }

        private void OpenBank()
        {
            try
            {
                string bankTabElementCsss = (string)Program.Elements["elements"][1]["menu_tabs"][1]["css_selector"];

                IWebElement bankTabElement = currentDriver.FindElement(By.CssSelector(bankTabElementCsss), 10);
                bankTabElement.Click();
            }
            catch (DisconnectedFromRSCompanionException)
            {
                Reconnect();
                OpenBank();
            }
            catch (InvalidOperationException)
            {
                Reconnect();
                OpenBank();
            }
        }

        private void OpenGrandExchange()
        {
            try
            {
                string geTabElementCsss = (string)Program.Elements["elements"][1]["menu_tabs"][0]["css_selector"];
                string slotListElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][1]["css_selector"];

                IWebElement geTabElement = currentDriver.FindElement(By.CssSelector(geTabElementCsss), 10);
                geTabElement.Click();

                // Get the slots list element
                IWebElement slotListElement = currentDriver.FindElement(By.CssSelector(slotListElementCsss), 20);

                // Wait for all slots to load properly
                var wait = new WebDriverWait(currentDriver, TimeSpan.FromSeconds(20));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.TagName("li")));

                // Sleep since the slot elements behave weird and this prevents the wrong slot being used.
                // NEEDS TO BE REPLACED!
                //Thread.Sleep(1000);

                // Replacement for sleep
                // Check if the class of the body tag is NOT 'loading'
                IWebElement bodyElement = currentDriver.FindElement(By.TagName("body"));
                currentDriver.WaitForAttribute(bodyElement, "class", "loading", 2);
            }
            catch (DisconnectedFromRSCompanionException)
            {
                Reconnect();
                OpenGrandExchange();
            }
            catch (InvalidOperationException)
            {
                Reconnect();
                OpenGrandExchange();
            }
            catch (NoSuchElementException)
            {
                OpenBank();
                OpenGrandExchange();
            }
            catch (TimeoutException)
            {
                Reconnect();
                OpenGrandExchange();
            }
            catch (WebDriverException e)
            {
                Reconnect();
                OpenGrandExchange();
            }
        }

        private void Reconnect()
        {
            try
            {
                CheckUpdateProxyAuthIp();

                GoToRuneScapeCompanionPage(currentAccount);

                Login(currentAccount);
                OpenGrandExchange();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SellItem(Slot slot)
        {
            try
            {
                string slotSellButtonElementCsss = ((string)Program.Elements["elements"][2]["grand_exchange_page"][0]["slot"][8]["css_selector"]).Replace("SLOT_NUMBER", slot.Number.ToString());
                string bankSearchElementCsss = (string)Program.Elements["elements"][3]["bank_page"][0]["css_selector"];
                string bankSearchResultItemElementCsss = (string)Program.Elements["elements"][3]["bank_page"][1]["css_selector"];
                string bankSearchResultItemNameElementCsss = (string)Program.Elements["elements"][3]["bank_page"][2]["css_selector"];
                string bankSearchResultItemSellElementCsss = (string)Program.Elements["elements"][3]["bank_page"][4]["css_selector"];

                string offerCollectionSlot1ElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][28]["css_selector"];
                string offerCollectionSlot2ElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][29]["css_selector"];

                // Click the "Sell" button of the slot.
                IWebElement slotSellButtonElement = currentDriver.FindElement(By.CssSelector(slotSellButtonElementCsss), 20);
                slotSellButtonElement.Click();

                bool itemFound = false;
                int attemps = 1;
                do
                {
                    try
                    {
                        // Enter the item name in the search bar and send "Enter" press to search.
                        IWebElement bankSearchElement = currentDriver.FindElement(By.CssSelector(bankSearchElementCsss), 10);
                        bankSearchElement.Clear();

                        bankSearchElement.SendKeys(slot.ItemName);

                        // Find the correct item in the list.
                        // Sometimes no result shows up in the list due to a bug in the RuneScape Companion web app.
                        string searchResultItemName = "";

                        int i = 1; // CHILD_NUMBER is not zero-based.
                        do
                        {
                            try
                            {
                                IWebElement bankSearchResultItemElement = currentDriver.FindElement(By.CssSelector(bankSearchResultItemElementCsss), 10);
                                bankSearchResultItemElement.Click();
                            }
                            catch (Exception)
                            {
                                IWebElement bankSearchResultItemElement = currentDriver.FindElement(By.CssSelector(bankSearchResultItemElementCsss + " active"), 10);
                                bankSearchResultItemElement.Click();
                            }

                            // Sleep replacement
                            try
                            {
                                // Wait for the sell button.
                                IWebElement bankSearchResultItemSellElement = currentDriver.FindElement(By.CssSelector(bankSearchResultItemSellElementCsss), 2);
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    // RuneScape Companion only showing stock market button and sell button so we need 2nd child instead of 3rd.
                                    IWebElement bankSearchResultItemSellElement = currentDriver.FindElement(By.CssSelector(bankSearchResultItemSellElementCsss.Replace('3', '2')), 2);
                                }
                                catch (Exception)
                                {
                                    // Retry, do nothing
                                }
                                
                            }

                            IWebElement bankSearchResultItemNameElement = currentDriver.FindElement(By.CssSelector(bankSearchResultItemNameElementCsss), 10);
                            searchResultItemName = bankSearchResultItemNameElement.Text;
                            i++;
                        } while (searchResultItemName != slot.ItemName);

                        // Set the itemFound varriable to true to escape the while loop.
                        itemFound = true;

                    }
                    catch (DisconnectedFromRSCompanionException)
                    {
                        throw new DisconnectedFromRSCompanionException();
                    }
                    catch (NoSuchElementException)
                    {
                        // The offer probably sold while trying to abort, so change the slotstate to complete selling
                        slot.SlotState = "complete selling";

                        CheckUpdateItemAndSlot(slot.Number);

                        // Write it as a sale
                        couchPortal.WriteSale(new Sale(currentAccount.Username, slot.ItemName, slot.BoughtFor, slot.SoldFor, slot.SoldFor - slot.BoughtFor, slot.GetItem().Tier, DateTime.Now));

                        OpenGrandExchange();

                        return;

                        // Now the slotstate will be set to selling again, but on the next check it will be noticed as empty.
                    }
                    catch (Exception)
                    {
                        // The item was not present. Making a new attempt to enter the name in the search bar.
                        Console.WriteLine("Error in searching for item. Attempt: " + attemps);
                        attemps++;
                    }
                } while (!itemFound && attemps <= 5);

                if (attemps <= 5)
                {
                    try
                    {
                        // Found the item in the list, click on the sell button to open the selling page.
                        IWebElement bankSearchResultItemSellElement = currentDriver.FindElement(By.CssSelector(bankSearchResultItemSellElementCsss), 0);
                        bankSearchResultItemSellElement.Click();
                    }
                    catch (Exception)
                    {
                        // RuneScape Companion only showing stock market button and sell button so we need 2nd child instead of 3rd.
                        IWebElement bankSearchResultItemSellElement = currentDriver.FindElement(By.CssSelector(bankSearchResultItemSellElementCsss.Replace('3', '2')), 0);
                        bankSearchResultItemSellElement.Click();
                    }

                    string offerQuantityElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][9]["css_selector"];
                    string offerPricePerItemElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][10]["css_selector"];
                    string offerPopupConfirmOkElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][18]["css_selector"];

                    // Fill in the quantity.
                    IWebElement offerQuantityElement = currentDriver.FindElement(By.CssSelector(offerQuantityElementCsss), 10);
                    offerQuantityElement.Clear();

                    offerQuantityElement.SendKeys("1");
                    // Replace with | offerQuantityElement.SendKeys(slot.Item.BuyLimit.toString());

                    IWebElement offerPricePerItemElement = currentDriver.FindElement(By.CssSelector(offerPricePerItemElementCsss), 20);
                    do
                    {
                        offerPricePerItemElement.Clear();
                        offerPricePerItemElement.SendKeys(slot.Value.ToString());
                        //Thread.Sleep(500);

                        // Replacement for above sleep
                        currentDriver.WaitForAttribute(offerPricePerItemElement, "value", slot.Value.ToString(), 1);

                    } while (offerPricePerItemElement.GetAttribute("value") != slot.Value.ToString());

                    offerPricePerItemElement.SendKeys(Keys.Enter);

                    try
                    {
                        IWebElement offerPopupConfirmOkElement = currentDriver.FindElement(By.CssSelector(offerPopupConfirmOkElementCsss), 20);
                        offerPopupConfirmOkElement.Click();
                    }
                    catch (Exception)
                    {
                        // The popup was not present. Making a new attempt to buy the item.
                        Console.WriteLine("Error in selling item. Confirmation popup not showing.");

                        OpenGrandExchange();

                        // Recursive call
                        SellItem(slot);

                        // Exit the method
                        return;
                    }

                    // Check the collection slots to check for INS (instant sell)
                    IWebElement offerCollectionSlot1Element;
                    bool INS;
                    try
                    {
                        offerCollectionSlot1Element = currentDriver.FindElement(By.CssSelector(offerCollectionSlot1ElementCsss), 5);
                        // INS
                        INS = true;
                    }
                    catch (Exception)
                    {
                        // No INS
                        INS = false;
                    }

                    if (INS)
                    {
                        // Check the price it bought for and apply rule:
                        string offerTotalPriceElementCsss = (string)Program.Elements["elements"][2]["grand_exchange_page"][23]["css_selector"];

                        IWebElement offerTotalPriceElement = currentDriver.FindElement(By.CssSelector(offerTotalPriceElementCsss), 10);
                        string offerTotalPriceRaw = offerTotalPriceElement.Text.Replace(" ", "").Replace(",", "").Replace("gp", "");
                        offerTotalPriceRaw = Regex.Replace(offerTotalPriceRaw, @"\D", "");

                        long offerTotalPrice = 0;
                        try
                        {
                            offerTotalPrice = 25000 * (long.Parse(offerTotalPriceRaw) / 25000);
                        }
                        catch (FormatException)
                        {
                            offerTotalPrice = slot.Value + 50000;
                        }

                        if (offerTotalPrice == 0)
                        {
                            // This is a bug with the RuneScape Companion website
                            offerTotalPrice = slot.Value + 50000;
                        }

                        // Apply Flipchat rule:
                        // Reports labelled with 'INS' require you to increase the sell price of the reported item by 50k from the reported price.
                        // Change only with 25K, since the slot will be evaluated as NIS later on the next slotCheck, and will thus be changed with the other 25K.
                        // Assign the offerTotalPrice value to slotValue
                        slot.Value = offerTotalPrice + 25000;
                        Item item = slot.GetItem();
                        item.CurrentSellPrice = slot.Value;

                        // Update the item
                        UpdateItem(item);

                        // Open the GE
                        OpenBank();
                        OpenGrandExchange();
                    }
                    else
                    {
                        // Open the GE
                        OpenBank();
                        OpenGrandExchange();
                    }
                }
                else
                {
                    // Failed, reconnect
                    throw new DisconnectedFromRSCompanionException();
                }

            }
            catch (DisconnectedFromRSCompanionException)
            {
                Reconnect();
                OpenGrandExchange();
                SellItem(slot);
            }
            catch (InvalidOperationException)
            {
                OpenBank();
                OpenGrandExchange();
                SellItem(slot);
            }
            catch (NoSuchElementException)
            {
                OpenBank();
                OpenGrandExchange();

                /*
                 * The sell button was not found.
                 *      Case 1: The wrong slot was collected before.
                 *      Case 1 validation: Try to find the correct slot. If this is successfull continue with the solution.
                 *      Case 1 solution: 
                 *          Step 1: Try to collect the correct slot again.
                 *          Step 2: Update the slotState.
                 *          Step 3: Update the slotValue.
                 *          Step 4: Try to sell the item in the slot again. (recursive call)
                 *          Step 5: Update the slotState
                 *          Step 6: Update the slot's time
                 *          Step 7: Find out what wrong slot was collected by getting the slot state
                 *                  If both the result of CheckSlotState is empty and the slot.State property is not empty, this is the slot we seek.
                 *          Step 8: Sell the item in the slot that we just found.
                 *              Step 8.1: Update the slotState
                 *              Step 8.2: Update the slotValue
                 *              Step 8.3: Try to sell the item in the slot again. (recursive call)
                 *              Step 8.4: Update the slotState
                 *              Step 8.5: // Update the slot's time
                 *          
                 *      Case 2: Something went wrong loading the grand exchange page.
                 *      Case 2 solution:
                 *          Step 1: Reload the Grand Exhange page.
                 *          Step 2: Try to sell the item in the slot again. (recursive call)
                */

                #region Case 1
                // Case 1 validation: Try to find the correct slot. If this is successfull continue with the solution.
                bool caseOneValidated = false;
                string slotItemNameElementCsss = ((string)Program.Elements["elements"][2]["grand_exchange_page"][0]["slot"][9]["css_selector"]).Replace("SLOT_NUMBER", slot.Number.ToString());

                try
                {
                    // Wait for the slot with the correct item name to be visible
                    currentDriver.FindElement(By.CssSelector(slotItemNameElementCsss), 20, slot.ItemName);
                    // The correct slot was found, set caseOnValidated to true to continue with the solution.
                    caseOneValidated = true;
                }
                catch (NoSuchElementException)
                {
                    // The correct slot was not found, so case 1 was not validated.
                    // do nothing
                }

                if (caseOneValidated)
                {
                    // Case 1 solution
                    // Step 1: Try to collect the correct slot again.
                    CollectSlot(slot);

                    // Case 1 solution
                    // Step 2: Update the slotState
                    slot.SlotState = "empty";

                    // Case 1 solution
                    // Step 3: Update the slotValue
                    slot.Value = slot.GetItem().CurrentSellPrice;

                    // Case 1 solution
                    // Step 4: Try to sell the item in the slot again. (recursive call)
                    SellItem(slot);

                    // Case 1 solution
                    // Step 5: Update the slotState
                    slot.SlotState = "selling";

                    // Case 1 solution
                    // Step 6: // Update the slot's time
                    slot.Time = DateTime.Now;

                    // Case 1 solution
                    // Step 7: Find out what wrong slot was collected by getting the slot state
                    //         If both the result of CheckSlotState is empty and the slot.SlotState property is not empty, this is the slot we seek.
                    foreach (Slot tempSlot in currentAccount.Slots)
                    {
                        string slotState = CheckSlotState(tempSlot.Number);
                        if (slotState == "empty" && tempSlot.SlotState != "empty")
                        {
                            // Case 1 solution
                            // Step 8: Sell the item in the slot that we just found.
                            // Case 1 solution
                            // Step 8.1: Update the slotState
                            tempSlot.SlotState = "empty";

                            // Case 1 solution
                            // Step 8.2: Update the slotValue
                            tempSlot.Value = tempSlot.GetItem().CurrentSellPrice;

                            // Case 1 solution
                            // Step 8.3: Try to sell the item in the slot again. (recursive call)
                            SellItem(tempSlot);

                            // Case 1 solution
                            // Step 8.4: Update the slotState
                            tempSlot.SlotState = "selling";

                            // Case 1 solution
                            // Step 8.5: // Update the slot's time
                            tempSlot.Time = DateTime.Now;
                        }
                    }
                }
                #endregion

                #region Case 2
                else
                {
                    // Case 2 solution:
                    // Step 1: Reload the Grand Exhange page.
                    OpenGrandExchange();

                    // Case 2 solution:
                    // Step 2: Try to sell the item in the slot again. (recursive call)
                    SellItem(slot);
                }
                #endregion

            }
            catch (WebDriverException)
            {
                Reconnect();
                OpenGrandExchange();
            }
        }

        public void Start()
        {
            while (true)
            {
                try
                {
                    CheckUpdateProxyAuthIp();

                    foreach (FlippingGroup flippingGroup in flippingGroups)
                    {
                        
                        flippingGroup.InitializeWebdriver();
                        currentDriver = flippingGroup.Driver;

                        for (int i = 0; i < flippingGroup.Accounts.Length; i++)
                        {
                            // Set the currentAccount to tempAccount
                            currentAccount = flippingGroup.Accounts[i];

                            // Execute some JavaScript to open a new window
                            ((IJavaScriptExecutor)currentDriver).ExecuteScript("window.open();");

                            // Save a reference to our new tab's window handle, this would be the last entry in the WindowHandles collection
                            flippingGroup.Accounts[i].TabReference = currentDriver.WindowHandles[currentDriver.WindowHandles.Count - 1];

                            // Switch our driver to the new tab's window handle
                            currentDriver.SwitchTo().WindowForceBackground(flippingGroup.Accounts[i].TabReference);

                            // Lets navigate to the RuneScape Companion web app in our new tab
                            GoToRuneScapeCompanionPage(flippingGroup.Accounts[i]);

                            // Login the current Account
                            Login(flippingGroup.Accounts[i]);

                            if (!flippingGroup.Accounts[i].ConnectionRefused)
                            {
                                // Open the Grand Exchange page
                                OpenGrandExchange();
                            }

                            Console.WriteLine("Successfully logged in: " + currentAccount.Email);
                        }
                    }

                    StartFlipping();
                }
                catch(InvalidOperationException e)
                {
                    Console.WriteLine("==================================BEGIN STACKTRACE==============================================");
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine("==================================END STACKTRACE==============================================");
                    Console.WriteLine("==================================BEGIN MESSAGE==============================================");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("==================================END MESSAGE==============================================");
                }
                catch (Exception)
                {
                    foreach (FlippingGroup flippingGroup in flippingGroups)
                    {
                        if (flippingGroup.Driver != null)
                        {
                            flippingGroup.Driver.Quit();
                        }
                    }
                }
            }
        }

        private void StartFlipping()
        {
            do
            {
                for (int f = 0; f < flippingGroups.Length; f++)
                {
                    currentFlippingGroup = flippingGroups[f];
                    currentDriver = currentFlippingGroup.Driver;

                    for (int i = 0; i < currentFlippingGroup.Accounts.Length; i++)
                    {
                        // Check if the first currentAccount has passed already.
                        // If so, save the currentAccount to the accounts array to save the info before overriding the currentAccount varraible.
                        if (i > 0)
                        {
                            currentFlippingGroup.Accounts[i - 1] = currentAccount;
                        }

                        // Get the correct currentAccount
                        currentAccount = currentFlippingGroup.Accounts[i];

                        try
                        {
                            // Get the correct tab
                            currentDriver.SwitchTo().WindowForceBackground(currentAccount.TabReference);

                            // Get the world from the url
                            string url = currentDriver.Url;
                            string worldParameter = url.Split('/')[3];
                            long world = long.Parse(Regex.Replace(worldParameter, @"\D", ""));

                            if (currentAccount.World != world)
                            {
                                // If the worlds don't match, the wrong window is open, so throw an exeption.
                                throw new NoSuchWindowException();
                            }
                        }
                        catch (NoSuchWindowException)
                        {
                            try
                            {
                                // The correct tab was not found, assign the correct tabs for the accounts
                                // We do this based on the world parameter in the url
                                // Start J on 1 since the tab with index 0 will be the blank starting tab
                                for (int j = 1; j < currentDriver.WindowHandles.Count; j++)
                                {
                                    // Switch to the tab
                                    currentDriver.SwitchTo().WindowForceBackground(currentDriver.WindowHandles[j]);

                                    // Get the world from the url
                                    string url = currentDriver.Url;
                                    string worldParameter = url.Split('/')[3];
                                    long world = long.Parse(Regex.Replace(worldParameter, @"\D", ""));

                                    // Get the matching account based on the world
                                    for (int k = 0; k < currentFlippingGroup.Accounts.Length; k++)
                                    {
                                        if (currentFlippingGroup.Accounts[k].World == world)
                                        {
                                            // Assign the current window handle to the same index in the tabs list as the index of the account in the accounts list.
                                            currentFlippingGroup.Accounts[k].TabReference = currentDriver.CurrentWindowHandle;
                                        }
                                    }
                                }

                                // Get the correct tab
                                currentDriver.SwitchTo().WindowForceBackground(currentAccount.TabReference);

                                StartFlipping();
                                return;
                            }
                            catch (WebDriverException)
                            {
                                Reconnect();
                                StartFlipping();
                                return;
                            }
                        }
                        catch (WebDriverException)
                        {
                            Reconnect();
                            StartFlipping();
                            return;
                        }

                        // Check if the account has the connectionRefused property on true.
                        // If so, try to log the account in and skip checking the account for now
                        // Else, continue flipping with the account
                        if (currentAccount.ConnectionRefused)
                        {
                            currentDriver.WrappedDriver.Navigate().Refresh();
                            Login(currentAccount);

                            if (!currentAccount.ConnectionRefused)
                            {
                                OpenGrandExchange();
                            }
                        }
                        else
                        {
                            for (int timesToCheckSlots = 5; timesToCheckSlots > 0; timesToCheckSlots--)
                            {
                                foreach (Slot slot in currentAccount.Slots)
                                {
                                    bool accountHasChangedValues = false;

                                    string slotState = CheckSlotState(slot.Number);
                                    string oldSlotState = slot.SlotState;

                                    if (slot.SlotState != slotState)
                                    {
                                        slot.SlotState = slotState;
                                        UpdateAccount(currentAccount);
                                    }

                                    switch (slotState)
                                    {
                                        case "empty":
                                            {
                                                // Check if there are items available for the account. If not, skip the slot.
                                                if (currentAccount.GetAvailableItems().Count > 0)
                                                {
                                                    // Get item to buy + update slot value's: itemName, Value.
                                                    Item itemToBuy = GetItemToBuy(slot);

                                                    // Check if itemToBuy is not null. If not continue, else it means a cooldown has been set.
                                                    if (itemToBuy != null)
                                                    {
                                                        slot.ItemName = itemToBuy.Name;

                                                        // Update the slotValue or currentBuyPrice if necessary.
                                                        CheckUpdateItemAndSlot(slot.Number);

                                                        // Buy item
                                                        BuyItem(slot);

                                                        // Update the slotState
                                                        slot.SlotState = "buying";

                                                        // Update the slot's time
                                                        slot.Time = DateTime.Now;

                                                        accountHasChangedValues = true;
                                                    }
                                                }

                                                break;
                                            }
                                        case "aborted buying":
                                            {
                                                // Get the name of the aborted item.
                                                slot.ItemName = GetAbortedItemName(slot);

                                                // Update the slotValue or currentBuyPrice if necessary.
                                                CheckUpdateItemAndSlot(slot.Number);

                                                // Collect the item.
                                                CollectSlot(slot);

                                                // Update the slotState
                                                slot.SlotState = "empty";

                                                // Buy the item for the updated price.
                                                BuyItem(slot);

                                                // Update the slotState
                                                slot.SlotState = "buying";

                                                // Update the slot's time
                                                slot.Time = DateTime.Now;

                                                accountHasChangedValues = true;

                                                break;
                                            }
                                        case "aborted selling":
                                            {
                                                // Get the name of the aborted item.
                                                slot.ItemName = GetAbortedItemName(slot);

                                                // Update the slotValue or currentBuyPrice if necessary.
                                                CheckUpdateItemAndSlot(slot.Number);

                                                // Collect the cash.
                                                CollectSlot(slot);

                                                // Update the slotState
                                                slot.SlotState = "empty";

                                                // Sell the item for the updated price.
                                                SellItem(slot);

                                                // Update the slotState
                                                slot.SlotState = "selling";

                                                // Update the slot's time
                                                slot.Time = DateTime.Now;

                                                accountHasChangedValues = true;

                                                break;
                                            }
                                        case "buying":
                                            {
                                                if (CheckUpdateItemAndSlot(slot.Number))
                                                {
                                                    // The slot's values have changed. Abort the offer and make a new one with the slot's new values.
                                                    // Abort the offer.
                                                    AbortSlot(slot);

                                                    // Update the slotState
                                                    slot.SlotState = "aborted buying";

                                                    // Buy the item.
                                                    BuyItem(slot);

                                                    // Update the slotState
                                                    slot.SlotState = "buying";

                                                    // Update the slot's time
                                                    slot.Time = DateTime.Now;

                                                    accountHasChangedValues = true;
                                                }

                                                break;
                                            }
                                        case "selling":
                                            {
                                                if (CheckUpdateItemAndSlot(slot.Number))
                                                {
                                                    // The slot's values have changed. Abort the offer and make a new one with the slot's new values.
                                                    // Abort the offer.
                                                    AbortSlot(slot);

                                                    // Update the slotState
                                                    slot.SlotState = "aborted selling";

                                                    // Sell the item.
                                                    SellItem(slot);

                                                    // Update the slotState
                                                    slot.SlotState = "selling";

                                                    // Update the slot's time
                                                    slot.Time = DateTime.Now;

                                                    accountHasChangedValues = true;
                                                }

                                                break;
                                            }
                                        case "complete buying":
                                            {
                                                CheckUpdateItemAndSlot(slot.Number);

                                                // Reset the pricingRule to 0.
                                                slot.PricingRule = 0;

                                                // Collect the item from the slot.
                                                CollectSlot(slot);

                                                // Update last buy of item
                                                currentAccount.UpdateLastBuy(slot);

                                                // Add 1 to the slots BuyLimitTracker
                                                slot.BuyLimitTracker++;

                                                // Update the slotState
                                                slot.SlotState = "empty";

                                                // Update the slotValue
                                                slot.Value = slot.GetItem().CurrentSellPrice;

                                                // Sell the item.
                                                SellItem(slot);

                                                // Update the slotState
                                                slot.SlotState = "selling";

                                                // Update the slot's time
                                                slot.Time = DateTime.Now;

                                                accountHasChangedValues = true;

                                                break;
                                            }
                                        case "complete selling":
                                            {
                                                CheckUpdateItemAndSlot(slot.Number);

                                                // Reset the pricingRule to 0.
                                                slot.PricingRule = 0;

                                                // Collect the cash from the slot.
                                                CollectSlot(slot);

                                                couchPortal.WriteSale(new Sale(currentAccount.Username, slot.ItemName, slot.BoughtFor, slot.SoldFor, slot.SoldFor - slot.BoughtFor, slot.GetItem().Tier, DateTime.Now));

                                                // Update the slotState
                                                slot.SlotState = "empty";

                                                // Check if the account has items available.
                                                // If not, the slot remains empty.
                                                if (currentAccount.GetAvailableItems().Count > 0)
                                                {
                                                    // Get item to buy + update slot value's: itemName, Value.
                                                    Item itemToBuy = GetItemToBuy(slot);

                                                    // Check if itemToBuy is not null. If not continue, else it means a cooldown has been set.
                                                    if (itemToBuy != null)
                                                    {
                                                        slot.ItemName = itemToBuy.Name;

                                                        // Update the slotValue or currentBuyPrice if necessary.
                                                        CheckUpdateItemAndSlot(slot.Number);

                                                        // Buy item
                                                        BuyItem(slot);

                                                        // Update the slotState
                                                        slot.SlotState = "buying";

                                                        // Update the slot's time
                                                        slot.Time = DateTime.Now;
                                                    }
                                                }

                                                accountHasChangedValues = true;

                                                break;
                                            }
                                        case "complete":
                                            {
                                                // This state can be read from the source of the RuneScape Companion web app due to a bug on their behalf.
                                                // Workaround, manually change the state based on what the slot state was before.
                                                if (oldSlotState == "buying")
                                                {
                                                    slot.SlotState = "complete buying";
                                                    goto case "complete buying";
                                                }
                                                else if (oldSlotState == "selling")
                                                {
                                                    slot.SlotState = "complete selling";
                                                    goto case "complete selling";
                                                }

                                                break;
                                            }
                                        default:
                                            break;
                                    }

                                    if (accountHasChangedValues == true)
                                    {
                                        OpenBank();
                                        OpenGrandExchange();
                                        currentAccount.MoneyPouchValue = GetMoneyPouchValue();

                                        currentAccount.SlotsValue = GetSlotsValue();

                                        currentAccount.TotalValue = currentAccount.MoneyPouchValue + currentAccount.SlotsValue;
                                        UpdateAccount(currentAccount);
                                        accountHasChangedValues = false;
                                    }
                                }
                            }
                        }
                    }
                }
            } while (true);
        }

        private void TakeScreenShot()
        {
            Screenshot ss = ((ITakesScreenshot)currentDriver).GetScreenshot();
            ss.SaveAsFile("../screenshot.jpg", ScreenshotImageFormat.Jpeg);
        }

        private void UpdateAccount(Account account)
        {
            couchPortal.UpdateAccount(account);
        }

        private void UpdateItem(Item item)
        {
            couchPortal.UpdateItem(item);
        }

    }
}
