using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Linq;

namespace AIOFlipper
{
    public partial class Account
    {
        public static Account FromJson(string json) => JsonConvert.DeserializeObject<Account>(json, Converter.Settings);
    }

    public partial class Account
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_rev")]
        public string Rev { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("authKey")]
        public string AuthKey { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("world")]
        public long World { get; set; }

        [JsonProperty("cooldownUntil")]
        public DateTime CooldownUntil { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("moneyPouchValue")]
        public long MoneyPouchValue { get; set; }

        [JsonProperty("slotsValue")]
        public long SlotsValue { get; set; }

        [JsonProperty("totalValue")]
        public long TotalValue { get; set; }

        [JsonProperty("maxTier")]
        public long MaxTier { get; set; }

        [JsonProperty("tabReference")]
        public string TabReference { get; set; }

        [JsonProperty("slots")]
        public Slot[] Slots { get; set; }

        [JsonProperty("lastItemBuys")]
        public string[] LastItemBuys { get; set; }

        public Account(string id, string rev, string username, string email, string password, string authKey, bool isActive, long world, DateTime cooldownUntil, DateTime startTime, long moneyPouchValue, long slotsValue, long totalValue, long maxTier, string tabReference, Slot[] slots, string[] lastItemBuys)
        {
            Id = id;
            Rev = rev;
            Username = username;
            Email = email;
            Password = password;
            AuthKey = authKey;
            IsActive = isActive;
            World = world;
            CooldownUntil = cooldownUntil;
            StartTime = startTime;
            MoneyPouchValue = moneyPouchValue;
            SlotsValue = slotsValue;
            TotalValue = totalValue;
            MaxTier = maxTier;
            TabReference = tabReference;
            Slots = slots;
            LastItemBuys = lastItemBuys;
        }

        public AccountNameTagUC CreateAccountTag()
        {
            AccountNameTagUC accountNameTag = new AccountNameTagUC(Username)
            {
                Location = new Point(1450, 11)
            };

            return accountNameTag;
        }

        public PictureBox CreateAccountCharacterImage()
        {
            PictureBox accountCharacterImage = new PictureBox
            {
                Image = Properties.Resources.Bandos_armour_equipped_male,
                //accountCharacterImage.Image = Properties.Resources.Third_age_melee_armour_equipped;
                SizeMode = PictureBoxSizeMode.AutoSize,
                BackColor = Color.Transparent,
                Location = new Point(1425, 68)
            };

            return accountCharacterImage;
        }

        public Panel CreateAccountItemSlotsPanel()
        {
            int x = -265;
            int y = 0;
            int i = 0;

            Panel panel = new Panel
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(425, 150)
            };

            foreach (Slot slot in Slots)
            {
                ItemSlotUC itemSlotUC = new ItemSlotUC(slot);

                if (i == 4)
                {
                    x = -265;
                    y = y + 328;
                }

                x = x + 265;
                itemSlotUC.Location = new Point(x, y);

                panel.Controls.Add(itemSlotUC);

                i++;
            }

            return panel;
        }

        public Panel CreateAccountInfoPanel()
        {
            Panel panel = new Panel
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(2000, 0)
            };

            panel.Controls.Add(CreateAccountCharacterImage());

            return panel;
        }

        public void UpdateLastBuy(Slot slot)
        {
            for (int i = 0; i < LastItemBuys.Length; i++)
            {
                string itemName = LastItemBuys[i].Split(';')[0];

                if (slot.ItemName == itemName)
                {
                    LastItemBuys[i] = itemName + ";" + DateTime.Now;
                    break;
                }
            }
        }

        public List<string> GetItemNamesInSlots()
        {
            List<string> itemNames = new List<string>();
            foreach (Slot slot in Slots)
            {
                // Check to see if the slot is not empty
                if (slot.SlotState != "empty")
                {
                    itemNames.Add(slot.ItemName);
                }
            }

            return itemNames;
        }

        public List<Item> GetAvailableItems()
        {
            List<Item> items = Program.Items;
            List<Item> availableItems = new List<Item>();
            for (int i = 0; i < LastItemBuys.Length; i++)
            {
                // Check if the date and time is not empty & check if the timestamp was longer than 4hours ago.
                if (LastItemBuys[i].Split(';')[1] != "" && (DateTime.Now - DateTime.Parse((LastItemBuys[i].Split(';')[1]))).TotalMinutes >= 240) //4h
                {
                    List<string> itemNamesInSlots = GetItemNamesInSlots();
                    // Check if the item is not already being bought by one of the slots
                    if (!itemNamesInSlots.Contains(LastItemBuys[i].Split(';')[0]))
                    {
                        for (int j = 0; j < items.Count; j++)
                        {
                            // Find the correct item in the list of items.
                            if (LastItemBuys[i].Split(';')[0] == items[j].Name)
                            {
                                // Add the item to the list of available items.
                                availableItems.Add(items[j]);
                                break;
                            }
                        }
                    }
                }
            }

            // Return the list of available items ordered by the current margin of the item, with the item with the highest margin comming first in the list.
            return (List<Item>)(availableItems.OrderByDescending(i => i.GetCurrentMargin()).ToList());
        }
    }
}
