using CryptoScopeAPI.Services;
using CryptoScopeAPI.Services.Synchronizers;
using CryptoScopeAPI.Tests.Unit.Fixtures;
using CryptoScopeAPI.Dtos.CoinGecko;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CryptoScopeAPI.Tests.Unit.Synchronizers;

public class CoinListSynchronizerTests : IClassFixture<GetCoinsFixture>
{
    private readonly GetCoinsFixture _fixture;

    public CoinListSynchronizerTests(GetCoinsFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetCoins();
    }

    [Fact]
    public async Task SyncAsync_ReplacesExistingCoins()
    {
        var response1 = new List<CoinListGeckoResponse>
        {
            new()
            {
                Id = "bitcoin",
                Name = "Bitcoin",
                Symbol = "btc",
                Image = "btc.png",
                CurrentPrice = 30000,
                MarketCap = 1_000_000,
                PriceChangePercentage24h = 1.1
            }
        };

        var response2 = new List<CoinListGeckoResponse>
        {
            new()
            {
                Id = "bitcoin",
                Name = "Bitcoin",
                Symbol = "btc",
                Image = "btc.png",
                CurrentPrice = 31000,
                MarketCap = 2_000_000,
                PriceChangePercentage24h = 2.5
            }
        };

        var mockClient = new Mock<ICoinGeckoClient>();
        mockClient.SetupSequence(x => x.GetTopMarketCoinsAsync())
                  .ReturnsAsync(response1)
                  .ReturnsAsync(response2);

        var mockLogger = new Mock<ILogger<CoinListSynchronizer>>();

        var synchronizer = new CoinListSynchronizer(
            _fixture.Context,
            mockClient.Object,
            _fixture.Mapper,
            mockLogger.Object
        );

        // Act – 1st sync
        await synchronizer.SyncAsync(CancellationToken.None);
        var coinAfterFirstSync = _fixture.Context.Coins.FirstOrDefault(c => c.CoinId == "bitcoin");

        // Assert – 1st
        coinAfterFirstSync.Should().NotBeNull();
        coinAfterFirstSync!.CurrentPriceUsd.Should().Be(30000);

        // Act – 2nd sync (updated data)
        await synchronizer.SyncAsync(CancellationToken.None);
        var coinAfterSecondSync = _fixture.Context.Coins.FirstOrDefault(c => c.CoinId == "bitcoin");

        // Assert – 2nd
        coinAfterSecondSync.Should().NotBeNull();
        coinAfterSecondSync!.CurrentPriceUsd.Should().Be(31000);
        coinAfterSecondSync.MarketCapUsd.Should().Be(2_000_000);
    }
}
