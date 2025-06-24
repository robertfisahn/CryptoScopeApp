using AutoMapper;
using CryptoScopeAPI.Mappings;
using CryptoScopeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoScopeAPI.Tests.Unit.Fixtures;

public class GetCoinsFixture
{
    public AppDbContext Context { get; }
    public IMapper Mapper { get; }

    public GetCoinsFixture()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb_" + Guid.NewGuid())
            .Options;

        Context = new AppDbContext(options);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<CoinMappingProfile>());
        Mapper = config.CreateMapper();

        SeedCoins();
    }

    private void SeedCoins()
    {
        Context.Coins.AddRange(
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

        Context.SaveChanges();
    }


    public void ResetCoins()
    {
        Context.Coins.RemoveRange(Context.Coins);
        SeedCoins();
    }
}
