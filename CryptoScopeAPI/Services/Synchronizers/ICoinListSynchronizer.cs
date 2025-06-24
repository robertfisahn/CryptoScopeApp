namespace CryptoScopeAPI.Services.Synchronizers
{
    public interface ICoinListSynchronizer
    {
        Task SyncAsync(CancellationToken cancellationToken = default);
    }
}