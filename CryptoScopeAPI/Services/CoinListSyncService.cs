using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Services.Synchronizers;
using Microsoft.Extensions.Options;

namespace CryptoScopeAPI.Services;

public class CoinListSyncService(
    ICoinListSynchronizer synchronizer,
    ILogger<CoinListSyncService> logger,
    IOptions<CoinSyncSettings> settings) : BackgroundService
{
    private readonly CoinSyncSettings _syncSettings = settings.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await synchronizer.SyncAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to sync coin list");
            }

            await Task.Delay(TimeSpan.FromSeconds(_syncSettings.TopListRefreshSeconds), stoppingToken);
        }
    }
}