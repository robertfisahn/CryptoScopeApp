using CryptoScopeAPI.Dtos.CoinGecko;
using CryptoScopeAPI.Helpers;

namespace CryptoScopeAPI.Services
{
    public class CoinGeckoClient(HttpClient _http) : ICoinGeckoClient
    {
        public async Task<List<CoinListGeckoResponse>> GetTopMarketCoinsAsync()
        {
            string url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=10&page=1&sparkline=false";
            return await CoinGeckoHelper.GetFromApiAsync<List<CoinListGeckoResponse>>(_http, url, CancellationToken.None);
        }

        public async Task<List<CoinSearchGeckoResponse>> GetSearchCoinsAsync()
        {
            string url = "https://api.coingecko.com/api/v3/coins/list";
            return await CoinGeckoHelper.GetFromApiAsync<List<CoinSearchGeckoResponse>>(_http, url, CancellationToken.None);
        }

        public async Task<CoinDetailsGeckoResponse> GetCoinDetailsAsync(string id, CancellationToken cancellationToken)
        {
            string url = $"https://api.coingecko.com/api/v3/coins/{id}";
            return await CoinGeckoHelper.GetFromApiAsync<CoinDetailsGeckoResponse>(_http, url, cancellationToken);
        }

        public async Task<CoinMarketChartGeckoResponse> GetCoinMarketChartAsync(string id, string days, CancellationToken cancellationToken)
        {
            string url = $"https://api.coingecko.com/api/v3/coins/{id}/market_chart?vs_currency=usd&days={days}";
            return await CoinGeckoHelper.GetFromApiAsync<CoinMarketChartGeckoResponse>(_http, url, cancellationToken);
        }
    }
}