using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AIOFlipper
{
    public partial class ItemSlotUC : UserControl
    {
        private Slot slot;

        public ItemSlotUC(Slot slot)
        {
            InitializeComponent();

            this.slot = slot;

            if (slot.SlotState == "empty")
            {
                labelItemName.Text = "Empty slot";
                labelItemPriceBuy.Text = "";
                labelItemPriceSell.Text = "";
                labelItemPriceCurrent.Text = "";

                pictureBoxItemIcon.Image = null;
            }
            else
            {
                labelItemName.Text = this.slot.ItemName;

                Item itemFromSlot = this.slot.GetItem();
                labelItemPriceBuy.Text = "Buy price: " + string.Format("{0:n0}", itemFromSlot.FlipchatBuyPrice);
                labelItemPriceSell.Text = "Sell price: " + string.Format("{0:n0}", itemFromSlot.FlipchatSellPrice);

                if (slot.SlotState == "buying" || slot.SlotState == "complete buying")
                    labelItemPriceCurrent.Text = "Buying: " + string.Format("{0:n0}", slot.Value);

                else if (slot.SlotState == "selling" || slot.SlotState == "complete selling")
                    labelItemPriceCurrent.Text = "Selling: " + string.Format("{0:n0}", slot.Value);

                else
                    labelItemPriceCurrent.Text = "Selling: " + string.Format("{0:n0}", slot.Value);

                pictureBoxItemIcon.ImageLocation = itemFromSlot.ItemImageUrl;
            }
        }

        public ItemSlotUC()
        {
            InitializeComponent();
            slot = null;
        }

        public Slot Slot
        {
            get
            {
                return slot;
            }

            set
            {
                slot = value;
            }
        }
    }
}
