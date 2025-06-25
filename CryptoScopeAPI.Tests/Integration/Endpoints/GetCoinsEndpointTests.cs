using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Tests.Integration.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace CryptoScopeAPI.Tests.Integration.Endpoints
{
    public class GetCoinsEndpointTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
    {
        [Fact]
        public async Task GetCoins_ReturnsOk_WithCoinList()
        {
            var response = await _client.GetAsync("/api/coins");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var coins = await response.Content.ReadFromJsonAsync<List<CoinListDto>>();
            coins.Should().NotBeNull();
            coins.Should().NotBeEmpty();
        }
    }
}
