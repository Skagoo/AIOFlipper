using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace AIOFlipper
{
    public partial class Account
    {
        public static Account[] FromJson(string json) => JsonConvert.DeserializeObject<Account[]>(json, Converter.Settings);
    }

    public partial class Account
    {
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

        public Account(string username, string email, string password, string authKey, bool isActive, long world, DateTime cooldownUntil, DateTime startTime, long moneyPouchValue, long slotsValue, long totalValue, long maxTier, string tabReference, Slot[] slots)
        {
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
    }
}
