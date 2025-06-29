using CryptoScopeAPI.Models;

namespace CryptoScopeAPI.Test.E2E;

public static class TestDbSeeder
{
    public static async Task ResetAsync(AppDbContext db)
    {
        db.Coins.RemoveRange(db.Coins);

        db.Coins.AddRange(
            new Coin
            {
                CoinId = "bitcoin",
                Name = "Bitcoin",
                Symbol = "btc",
                ImageUrl = "bitcoin.png",
                CurrentPriceUsd = 30000,
                MarketCapUsd = 500_000_000,
                PriceChangePercentage24h = 1.5
            },
            new Coin
            {
                CoinId = "ethereum",
                Name = "Ethereum",
                Symbol = "eth",
                ImageUrl = "ethereum.png",
                CurrentPriceUsd = 2000,
                MarketCapUsd = 200_000_000,
                PriceChangePercentage24h = -0.8
            }
        );

        await db.SaveChangesAsync();
    }
}
