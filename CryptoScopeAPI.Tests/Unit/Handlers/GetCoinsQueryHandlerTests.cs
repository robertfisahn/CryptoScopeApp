using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Exceptions;
using CryptoScopeAPI.Features.GetCoins;
using CryptoScopeAPI.Tests.Unit.Fixtures;
using FluentAssertions;

namespace CryptoScopeAPI.Tests.Unit.Handlers;

public class GetCoinsQueryHandlerTests : IClassFixture<GetCoinsFixture>
{
    private readonly GetCoinsFixture _fixture;

    public GetCoinsQueryHandlerTests(GetCoinsFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetCoins();
    }

    [Fact]
    public async Task Handle_ReturnsMappedCoins()
    {
        var handler = new GetCoinsQueryHandler(_fixture.Context, _fixture.Mapper);

        var result = await handler.Handle(new GetCoinsQuery(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Bitcoin");
        result[1].Symbol.Should().Be("eth");
    }

    [Fact]
    public async Task Handle_ReturnsListOfCoins_WhenCoinsExist()
    {
        var fixture = new GetCoinsFixture();
        var handler = new GetCoinsQueryHandler(fixture.Context, fixture.Mapper);
        var query = new GetCoinsQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<List<CoinListDto>>();
        result.Count.Should().Be(fixture.Context.Coins.Count());
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoCoinsExist()
    {
        var fixture = new GetCoinsFixture();
        fixture.Context.Coins.RemoveRange(fixture.Context.Coins);
        fixture.Context.SaveChanges();

        var handler = new GetCoinsQueryHandler(fixture.Context, fixture.Mapper);
        var query = new GetCoinsQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
