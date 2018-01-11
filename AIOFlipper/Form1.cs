using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json;
using Transitions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AIOFlipper
{
    public partial class Form1 : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private Account activeAccount;

        Panel accountInfoPanel;
        Panel accountItemSlotsPanel;
        AccountNameTagUC accountNameTag;

        public Form1()
        {
            InitializeComponent();

            // Get first currentAccount from list.
            activeAccount = Program.Accounts[0];
            DisplayAccountContent(activeAccount);
        }

        // Solves form flickering issue
        // Source: https://stackoverflow.com/questions/8046560/how-to-stop-flickering-c-sharp-winforms
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        // Makes the form movable without a border.
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        // Minimizes the window.
        private void buttonMinimizeForm_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Closes the window.
        private void buttonCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Enables the menu animation timer.
        private void buttonMenu_Click(object sender, EventArgs e)
        {
            timerMenuSlideInOut.Enabled = true;
        }

        // Opens/Closes the menu with an animation.
        private void timerMenuSlideInOut_Tick(object sender, EventArgs e)
        {
            if (int.Parse(timerMenuSlideInOut.Tag.ToString()) == 0)
            {
                if (buttonAccounts.Location.Y > int.Parse(buttonAccounts.Tag.ToString().Split(';')[0]))
                {
                    buttonAccounts.Location = new Point(buttonAccounts.Location.X + 15, buttonAccounts.Location.Y - 50);
                    buttonDashboard.Location = new Point(buttonDashboard.Location.X + 15, buttonDashboard.Location.Y - 50);
                    buttonSettings.Location = new Point(buttonSettings.Location.X + 15, buttonSettings.Location.Y - 50);
                }
                else
                {
                    timerMenuSlideInOut.Enabled = false;
                    timerMenuSlideInOut.Tag = 1;
                }
            }
            else
            {
                if (buttonAccounts.Location.Y < int.Parse(buttonAccounts.Tag.ToString().Split(';')[1]))
                {
                    buttonAccounts.Location = new Point(buttonAccounts.Location.X - 15, buttonAccounts.Location.Y + 50);
                    buttonDashboard.Location = new Point(buttonDashboard.Location.X - 15, buttonDashboard.Location.Y + 50);
                    buttonSettings.Location = new Point(buttonSettings.Location.X - 15, buttonSettings.Location.Y + 50);
                }
                else
                {
                    timerMenuSlideInOut.Enabled = false;
                    timerMenuSlideInOut.Tag = 0;
                }
            }
        }

        // Instanciates all accounts and opens up the first one in the list.
        private void buttonAccounts_Click(object sender, EventArgs e)
        {
            // Get first currentAccount from list.
            DisplayAccountContent(Program.Accounts[0]);

            // Optional - Hide the menu.
            timerMenuSlideInOut.Enabled = true;
        }

        // Handles all functionality to display currentAccount content via UserControls.
        private void DisplayAccountContent(Account account)
        {
            // Show the navigation buttons
            pictureBoxNavTab1.Visible = true;
            pictureBoxNavTab2.Visible = true;

            pictureBoxNavTab1.Image = Properties.Resources.radio_checked;
            pictureBoxNavTab2.Image = Properties.Resources.radio;

            pictureBoxNavTab1.Tag = "1";
            pictureBoxNavTab2.Tag = "0";

            // Hide Settings if open
            HideSettingsContent();

            // accountNameTag
            this.Controls.Remove(accountNameTag);
            accountNameTag = account.CreateAccountTag();
            this.Controls.Add(accountNameTag);

            // accountItemSlotsPanel
            this.Controls.Remove(accountItemSlotsPanel);
            accountItemSlotsPanel = account.CreateAccountItemSlotsPanel();
            this.Controls.Add(accountItemSlotsPanel);

            // accountInfoPanel
            this.Controls.Remove(accountInfoPanel);
            accountInfoPanel = account.CreateAccountInfoPanel();
            this.Controls.Add(accountInfoPanel);

        }

        private void pictureBoxNavTab1_Click(object sender, EventArgs e)
        {
            navTabClicked(sender, e);
        }

        private void pictureBoxNavTab2_Click(object sender, EventArgs e)
        {
            navTabClicked(sender, e);
        }

        private void navTabClicked(object sender, EventArgs e)
        {
            if (pictureBoxNavTab1.Tag.ToString() == "0" && (PictureBox)sender != pictureBoxNavTab2) // means pictureBoxNavTab1_Click (valid)
            {
                pictureBoxNavTab1.Image = Properties.Resources.radio_checked;
                pictureBoxNavTab2.Image = Properties.Resources.radio;

                pictureBoxNavTab1.Tag = "1";
                pictureBoxNavTab2.Tag = "0";

                ShowAccountItemSlots(activeAccount);
            }
            else if (pictureBoxNavTab2.Tag.ToString() != "1" && (PictureBox)sender != pictureBoxNavTab1) // means pictureBoxNavTab2_Click (valid)
            {
                pictureBoxNavTab1.Image = Properties.Resources.radio;
                pictureBoxNavTab2.Image = Properties.Resources.radio_checked;

                pictureBoxNavTab1.Tag = "0";
                pictureBoxNavTab2.Tag = "1";

                ShowAccountInfo(activeAccount);
            }
        }

        private void ShowAccountInfo(Account account)
        {
            accountInfoPanel.Location = new Point(0, accountInfoPanel.Location.Y);
            accountItemSlotsPanel.Location = new Point(2000, accountItemSlotsPanel.Location.Y);
        }

        private void ShowAccountItemSlots(Account account)
        {
            accountInfoPanel.Location = new Point(2000, accountInfoPanel.Location.Y);
            accountItemSlotsPanel.Location = new Point(425, accountItemSlotsPanel.Location.Y);
        }

        private void HideAccountInfo()
        {
            accountInfoPanel.Location = new Point(2000, accountInfoPanel.Location.Y);
            accountItemSlotsPanel.Location = new Point(2000, accountItemSlotsPanel.Location.Y);
        }

        private void HideAccountItemSlots()
        {
            accountInfoPanel.Location = new Point(2000, accountInfoPanel.Location.Y);
            accountItemSlotsPanel.Location = new Point(2000, accountItemSlotsPanel.Location.Y);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            // Optional - Hide the menu.
            timerMenuSlideInOut.Enabled = true;

            HideAccountInfo();
            HideAccountItemSlots();
            DisplaySettingsContent();
        }

        private void buttonFlipchatSubmission_Click(object sender, EventArgs e)
        {
            TextBox textBoxFlipChatSubmission = (TextBox)Controls.Find("textBoxFlipChatSubmission", true)[0];
            CheckBox checkBoxForce = (CheckBox)Controls.Find("checkBoxForce", true)[0];

            string flipchatMargins = textBoxFlipChatSubmission.Text.Replace(':', '-');

            string[] flipchatMarginsPricesRaw = flipchatMargins.Split('-');

            Queue<string> flipchatMarginsPrices = new Queue<string>();

            foreach (string price in flipchatMarginsPricesRaw)
            {
                if (Regex.Replace(price, @"\D", "") != "" && Regex.Replace(price, @"\D", "") != "2")
                {
                    flipchatMarginsPrices.Enqueue(Regex.Replace(price, @"\D", ""));
                }
            }

            Item[] items = Program.Items;
            foreach (Item item in items)
            {
                item.FlipchatBuyPrice = long.Parse(flipchatMarginsPrices.Dequeue()) * 1000;
                item.FlipchatSellPrice = long.Parse(flipchatMarginsPrices.Dequeue()) * 1000;

                if (checkBoxForce.Checked && item.Tier == 3)
                {
                    item.CurrentBuyPrice = item.FlipchatBuyPrice;
                    item.CurrentSellPrice = item.FlipchatSellPrice;
                }
            }

            Program.Items = items;
        }

        private void DisplaySettingsContent()
        {
            TextBox textBoxFlipChatSubmission = new TextBox()
            {
                Location = new Point(86, 65),
                Name = "textBoxFlipChatSubmission",
                Multiline = true,
                Size = new Size(1755, 836),
            };
            Controls.Add(textBoxFlipChatSubmission);

            Button buttonFlipChatSubmission = new Button()
            {
                Location = new Point(922, 907),
                Name = "buttonFlipChatSubmission",
                Size = new Size(75, 23),
                TabIndex = 9,
                Text = "button1",
                UseVisualStyleBackColor = true
            };
            Controls.Add(buttonFlipChatSubmission);

            CheckBox checkBoxForce = new CheckBox()
            {
                AutoSize = true,
                Location = new Point(1003, 911),
                Name = "checkBoxForce",
                Size = new Size(80, 17),
                TabIndex = 10,
                Text = "Force?",
                UseVisualStyleBackColor = true
            };
            Controls.Add(checkBoxForce);

            buttonFlipChatSubmission.Click += new EventHandler(buttonFlipchatSubmission_Click);
        }

        private void HideSettingsContent()
        {
            try
            {
                TextBox textBoxFlipChatSubmission = (TextBox)Controls.Find("textBoxFlipChatSubmission", true)[0];
                CheckBox checkBoxForce = (CheckBox)Controls.Find("checkBoxForce", true)[0];

                this.Controls.Remove(textBoxFlipChatSubmission);
                this.Controls.Remove(checkBoxForce);
            }
            catch (Exception)
            {
                // Controls were not present so do nothing
            }
        }
    }
}
