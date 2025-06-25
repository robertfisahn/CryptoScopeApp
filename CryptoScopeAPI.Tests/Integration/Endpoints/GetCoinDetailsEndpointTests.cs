using System.Net;
using System.Net.Http.Json;
using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Tests.Integration.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CryptoScopeAPI.Tests.Integration.Endpoints
{
    public class GetCoinDetailsEndpointTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
    {
        [Fact]
        public async Task GetCoinDetails_ReturnsOk_WithExpectedCoin()
        {
            var response = await _client.GetAsync("/api/coins/bitcoin");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var coin = await response.Content.ReadFromJsonAsync<CoinDetailsDto>();
            coin.Should().NotBeNull();
            coin!.Id.Should().Be("bitcoin");
            coin.Name.Should().Be("Bitcoin");
            coin.Symbol.Should().Be("btc");
        }
    }
}
