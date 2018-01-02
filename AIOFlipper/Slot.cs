using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Slot(long number, string itemName, string slotState, DateTime time, long value, long boughtFor, long soldFor)
        {
            Number = number;
            ItemName = itemName;
            SlotState = slotState;
            Time = time;
            Value = value;
            BoughtFor = boughtFor;
            SoldFor = soldFor;
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
