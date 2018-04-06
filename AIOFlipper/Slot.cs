using System;
using Newtonsoft.Json;

namespace AIOFlipper
{
    public partial class Slot
    {
        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("itemName")]
        public string ItemName { get; set; }

        [JsonProperty("slotState")]
        public string SlotState { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("boughtFor")]
        public long BoughtFor { get; set; }

        [JsonProperty("soldFor")]
        public long SoldFor { get; set; }

        [JsonProperty("buyLimitTracker")]
        public long BuyLimitTracker { get; set; }

        [JsonProperty("pricingRule")]
        public long PricingRule { get; set; }

        public Slot(long number, string itemName, string slotState, DateTime time, long value, long boughtFor, long soldFor, long buyLimitTracker, long pricingRule)
        {
            Number = number;
            ItemName = itemName;
            SlotState = slotState;
            Time = time;
            Value = value;
            BoughtFor = boughtFor;
            SoldFor = soldFor;
            BuyLimitTracker = buyLimitTracker;
            PricingRule = pricingRule;
        }

        public Item GetItem()
        {
            foreach (Item item in Program.Items)
            {
                if (item.Name == ItemName)
                    return item;
            }

            return null;
        }
    }
}
