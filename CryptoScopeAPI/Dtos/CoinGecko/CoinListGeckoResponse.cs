﻿using System.Text.Json.Serialization;

namespace CryptoScopeAPI.Dtos.CoinGecko
{
    public class CoinListGeckoResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = default!;

        [JsonPropertyName("image")]
        public string Image { get; set; } = default!;

        [JsonPropertyName("current_price")]
        public decimal CurrentPrice { get; set; }

        [JsonPropertyName("price_change_percentage_24h")]
        public double PriceChangePercentage24h { get; set; }

        [JsonPropertyName("market_cap")]
        public decimal MarketCap { get; set; }
    }
}