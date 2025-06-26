using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using CryptoScopeAPI.Services.Synchronizers;
using CryptoScopeAPI.Dtos;

namespace CryptoScopeAPI.Services;

public class CoinListSyncService(
    IServiceScopeFactory scopeFactory,
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
                using var scope = scopeFactory.CreateScope();

                var synchronizer = scope.ServiceProvider.GetRequiredService<ICoinListSynchronizer>();
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
