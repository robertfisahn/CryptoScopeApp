using CryptoScopeAPI.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CryptoScopeAPI.Test.E2E;

public static class TestEndpoints
{
    public static void MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/test/coins", async (AppDbContext db, IMapper _mapper) =>
        {
            var coins = await db.Coins.ToListAsync();
            return Results.Ok(_mapper.Map<List<CoinListDto>>(coins));
        });

        app.MapPost("/api/test/reset", async (AppDbContext db) =>
        {
            await TestDbSeeder.ResetAsync(db);
            return Results.Ok("Reset complete");
        });

        app.MapDelete("/api/test/coins", async (AppDbContext db) =>
        {
            db.Coins.RemoveRange(db.Coins);
            await db.SaveChangesAsync();
            return Results.Ok("All coins deleted");
        });
    }
}
