using Newtonsoft.Json;

namespace AIOFlipper
{
    public partial class Item
    {
        public static Item[] FromJson(string json) => JsonConvert.DeserializeObject<Item[]>(json, Converter.Settings);
    }

    public partial class Item
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("flipchatBuyPrice")]
        public long FlipchatBuyPrice { get; set; }

        [JsonProperty("flipchatSellPrice")]
        public long FlipchatSellPrice { get; set; }

        [JsonProperty("currentBuyPrice")]
        public long CurrentBuyPrice { get; set; }

        [JsonProperty("currentSellPrice")]
        public long CurrentSellPrice { get; set; }

        [JsonProperty("priceIncrementValue")]
        public long PriceIncrementValue { get; set; }

        [JsonProperty("priceDecrementValue")]
        public long PriceDecrementValue { get; set; }

        [JsonProperty("minimalMargin")]
        public long MinimalMargin { get; set; }

        [JsonProperty("buyLimit")]
        public long BuyLimit { get; set; }

        [JsonProperty("tier")]
        public long Tier { get; set; }

        [JsonProperty("itemImageUrl")]
        public string ItemImageUrl { get; set; }

        public Item(string name, long flipchatBuyPrice, long flipchatSellPrice, long currentBuyPrice, long currentSellPrice, long priceIncrementValue, long priceDecrementValue, long minimalMargin, long buyLimit, long tier, string itemImageUrl)
        {
            Name = name;
            FlipchatBuyPrice = flipchatBuyPrice;
            FlipchatSellPrice = flipchatSellPrice;
            CurrentBuyPrice = currentBuyPrice;
            CurrentSellPrice = currentSellPrice;
            PriceIncrementValue = priceIncrementValue;
            PriceDecrementValue = priceDecrementValue;
            MinimalMargin = minimalMargin;
            BuyLimit = buyLimit;
            Tier = tier;
            ItemImageUrl = itemImageUrl;
        }
    }
}
