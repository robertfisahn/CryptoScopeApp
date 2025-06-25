using System.Net;
using System.Net.Http.Json;
using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Tests.Integration.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CryptoScopeAPI.Tests.Integration.Endpoints;

public class GetCoinMarketChartEndpointTests(WebApplicationFactory<Program> factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetCoinMarketChart_ReturnsOk_WithExpectedData()
    {
        var response = await _client.GetAsync("/api/coins/bitcoin/market_chart?days=7");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var chart = await response.Content.ReadFromJsonAsync<CoinMarketChartDto>();
        chart.Should().NotBeNull();
        chart!.Prices.Should().NotBeEmpty();
        chart.MarketCaps.Should().NotBeEmpty();
        chart.TotalVolumes.Should().NotBeEmpty();
    }
}