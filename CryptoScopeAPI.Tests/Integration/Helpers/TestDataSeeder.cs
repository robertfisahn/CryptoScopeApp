using CryptoScopeAPI.Models;

namespace CryptoScopeAPI.Tests.Integration.Helpers;

public static class TestDataSeeder
{
    public static void Seed(AppDbContext db)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        SeedCoinDetails(db);
        SeedTop10Coins(db);
        SeedCoinMarketChart(db);
        SeedSearchCoins(db);
    }
    public static void SeedCoinDetails(AppDbContext db)
    {
        db.CoinDetails.Add(new CoinDetails
        {
            CoinId = "bitcoin",
            Name = "Bitcoin",
            Symbol = "btc",
            ImageThumb = "...",
            ImageSmall = "...",
            ImageLarge = "...",
            CurrentPriceUsd = 65000m,
            MarketCapUsd = 1300000000000m,
            PriceChangePercentage24h = 0.3,
            LastUpdated = DateTime.UtcNow
        });

        db.SaveChanges();
    }
    public static void SeedCoinMarketChart(AppDbContext db)
    {
        db.CoinMarketCharts.Add(new CoinMarketChart
        {
            CoinId = "bitcoin",
            TimeRange = "7",
            PricesJson = "[[1625097600000, 33500.0], [1625184000000, 34000.0]]",
            MarketCapsJson = "[[1625097600000, 600000000000], [1625184000000, 620000000000]]",
            TotalVolumesJson = "[[1625097600000, 30000000000], [1625184000000, 32000000000]]",
            LastUpdated = DateTime.UtcNow
        });

        db.SaveChanges();
    }
    public static void SeedSearchCoins(AppDbContext db)
    {
        db.SearchCoins.AddRange(
            new SearchCoin { CoinId = "bitcoin", Name = "Bitcoin", Symbol = "btc" },
            new SearchCoin { CoinId = "ethereum", Name = "Ethereum", Symbol = "eth" },
            new SearchCoin { CoinId = "dogecoin", Name = "Dogecoin", Symbol = "doge" }
        );
        db.SaveChanges();
    }
    private static void SeedTop10Coins(AppDbContext db)
    {

        db.Coins.AddRange(
            new Coin
            {
                CoinId = "bitcoin",
                Name = "Bitcoin",
                Symbol = "btc",
                ImageUrl = "https://assets.coingecko.com/coins/images/1/large/bitcoin.png",
                CurrentPriceUsd = 65000m,
                MarketCapUsd = 1300000000000m,
                PriceChangePercentage24h = -0.5,
                LastUpdated = DateTime.UtcNow
            },
            new Coin
            {
                CoinId = "ethereum",
                Name = "Ethereum",
                Symbol = "eth",
                ImageUrl = "https://assets.coingecko.com/coins/images/279/large/ethereum.png",
                CurrentPriceUsd = 3500m,
                MarketCapUsd = 420000000000m,
                PriceChangePercentage24h = 1.2,
                LastUpdated = DateTime.UtcNow
            },
            new Coin
            {
                CoinId = "tether",
                Name = "Tether",
                Symbol = "usdt",
                ImageUrl = "https://assets.coingecko.com/coins/images/325/large/Tether.png",
                CurrentPriceUsd = 1m,
                MarketCapUsd = 110000000000m,
                PriceChangePercentage24h = 0,
                LastUpdated = DateTime.UtcNow
            },
            new Coin
            {
                CoinId = "binancecoin",
                Name = "BNB",
                Symbol = "bnb",
                ImageUrl = "https://assets.coingecko.com/coins/images/825/large/bnb.png",
                CurrentPriceUsd = 600m,
                MarketCapUsd = 92000000000m,
                PriceChangePercentage24h = -0.3,
                LastUpdated = DateTime.UtcNow
            },
            new Coin
            {
                CoinId = "solana",
                Name = "Solana",
                Symbol = "sol",
                ImageUrl = "https://assets.coingecko.com/coins/images/4128/large/solana.png",
                CurrentPriceUsd = 140m,
                MarketCapUsd = 62000000000m,
                PriceChangePercentage24h = 2.1,
                LastUpdated = DateTime.UtcNow
            },
            new Coin
            {
                CoinId = "ripple",
                Name = "XRP",
                Symbol = "xrp",
                ImageUrl = "https://assets.coingecko.com/coins/images/44/large/xrp-symbol-white-128.png",
                CurrentPriceUsd = 0.52m,
                MarketCapUsd = 28000000000m,
                PriceChangePercentage24h = -1.7,
                LastUpdated = DateTime.UtcNow
            },
            new Coin
            {
                CoinId = "usd-coin",
                Name = "USDC",
                Symbol = "usdc",
                ImageUrl = "https://assets.coingecko.com/coins/images/6319/large/USD_Coin_icon.png",
                CurrentPriceUsd = 1.00m,
                MarketCapUsd = 32000000000m,
                PriceChangePercentage24h = 0.0,
                LastUpdated = DateTime.UtcNow
            },
            new Coin
            {
                CoinId = "dogecoin",
                Name = "Dogecoin",
                Symbol = "doge",
                ImageUrl = "https://assets.coingecko.com/coins/images/5/large/dogecoin.png",
                CurrentPriceUsd = 0.16m,
                MarketCapUsd = 23000000000m,
                PriceChangePercentage24h = 0.9,
                LastUpdated = DateTime.UtcNow
            },
            new Coin
            {
                CoinId = "cardano",
                Name = "Cardano",
                Symbol = "ada",
                ImageUrl = "https://assets.coingecko.com/coins/images/975/large/cardano.png",
                CurrentPriceUsd = 0.42m,
                MarketCapUsd = 15000000000m,
                PriceChangePercentage24h = -0.2,
                LastUpdated = DateTime.UtcNow
            },
            new Coin
            {
                CoinId = "avalanche-2",
                Name = "Avalanche",
                Symbol = "avax",
                ImageUrl = "https://assets.coingecko.com/coins/images/12559/large/coin-round-red.png",
                CurrentPriceUsd = 30.25m,
                MarketCapUsd = 11000000000m,
                PriceChangePercentage24h = 1.5,
                LastUpdated = DateTime.UtcNow
            }
        );
        db.SaveChanges();
    }
}
