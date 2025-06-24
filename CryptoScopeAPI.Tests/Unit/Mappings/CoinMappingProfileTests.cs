using AutoMapper;
using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Dtos.CoinGecko;
using CryptoScopeAPI.Exceptions;
using CryptoScopeAPI.Features.GetCoins;
using CryptoScopeAPI.Mappings;
using CryptoScopeAPI.Models;
using CryptoScopeAPI.Tests.Unit.Fixtures;
using FluentAssertions;

namespace CryptoScopeAPI.Tests.Unit.Mappings;

public class CoinMappingProfileTests
{
    private readonly IMapper _mapper;

    public CoinMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CoinMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Coin_Maps_To_CoinListDto_Correctly()
    {
        var coin = new Coin
        {
            Id = 1,
            CoinId = "bitcoin",
            Name = "Bitcoin",
            Symbol = "btc",
            ImageUrl = "bitcoin.png",
            CurrentPriceUsd = 30000,
            MarketCapUsd = 500_000_000,
            PriceChangePercentage24h = 2.1
        };

        var dto = _mapper.Map<CoinListDto>(coin);

        dto.Id.Should().Be("bitcoin");
        dto.Image.Should().Be("bitcoin.png");
        dto.CurrentPrice.Should().Be(30000);
    }

    [Fact]
    public void CoinListGeckoResponse_Maps_To_Coin_Correctly()
    {
        var response = new CoinListGeckoResponse
        {
            Id = "bitcoin",
            Name = "Bitcoin",
            Symbol = "btc",
            Image = "bitcoin.png",
            CurrentPrice = 30000,
            MarketCap = 500_000_000,
            PriceChangePercentage24h = 2.1
        };

        var coin = _mapper.Map<Coin>(response);

        coin.CoinId.Should().Be("bitcoin");
        coin.Name.Should().Be("Bitcoin");
        coin.Symbol.Should().Be("btc");
        coin.ImageUrl.Should().Be("bitcoin.png");
        coin.CurrentPriceUsd.Should().Be(30000);
        coin.MarketCapUsd.Should().Be(500_000_000);
    }
}
