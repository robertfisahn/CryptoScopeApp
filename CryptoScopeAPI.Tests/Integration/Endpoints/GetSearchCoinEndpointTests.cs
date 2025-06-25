using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Tests.Integration.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace CryptoScopeAPI.Tests.Integration.Endpoints
{
    public class GetSearchCoinEndpointTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
    {
        [Fact]
        public async Task GetSearchCoins_ReturnsOk_WithList()
        {
            var response = await _client.GetAsync("/api/coins/search");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var coins = await response.Content.ReadFromJsonAsync<List<SearchCoinDto>>();
            coins.Should().NotBeNull();
            coins.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetSearchCoins_ReturnsEmptyList_WhenNoData()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.SearchCoins.RemoveRange(db.SearchCoins);
            await db.SaveChangesAsync();

            var response = await _client.GetAsync("/api/coins/search");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var coins = await response.Content.ReadFromJsonAsync<List<SearchCoinDto>>();
            coins.Should().NotBeNull();
            coins.Should().BeEmpty();
        }
    }
}
