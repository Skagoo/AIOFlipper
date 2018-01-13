using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AIOFlipper
{
    public partial class Sale
    {
        public static Sale FromJson(string json) => JsonConvert.DeserializeObject<Sale>(json, Converter.Settings);
    }

    public partial class Sale
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("itemName")]
        public string ItemName { get; set; }

        [JsonProperty("boughtFor")]
        public long BoughtFor { get; set; }

        [JsonProperty("soldFor")]
        public long SoldFor { get; set; }

        [JsonProperty("profit")]
        public long Profit { get; set; }

        [JsonProperty("tier")]
        public long Tier { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        public Sale(string username, string itemName, long boughtFor, long soldFor, long profit, long tier, DateTime date)
        {
            Username = username;
            ItemName = itemName;
            BoughtFor = boughtFor;
            SoldFor = soldFor;
            Profit = profit;
            Tier = tier;
            Date = date;
        }
    }
}
