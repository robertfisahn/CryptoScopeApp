using AutoMapper;
using CryptoScopeAPI.Models;

namespace CryptoScopeAPI.Services.Synchronizers;

public class CoinListSynchronizer(AppDbContext _db, ICoinGeckoClient _client, IMapper _mapper, ILogger<CoinListSynchronizer> _logger) : ICoinListSynchronizer
{
    public async Task SyncAsync(CancellationToken cancellationToken)
    {
        var freshCoins = await _client.GetTopMarketCoinsAsync();
        var mappedCoins = _mapper.Map<List<Coin>>(freshCoins);

        _db.Coins.RemoveRange(_db.Coins);
        await _db.Coins.AddRangeAsync(mappedCoins, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Top coin list synced at {Time}", DateTime.UtcNow);
    }
}
