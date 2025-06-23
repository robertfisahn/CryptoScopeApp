namespace CryptoScopeAPI.Helpers
{
    public static class CoinGeckoHelper
    {
        public static async Task<T> GetFromApiAsync<T>(HttpClient httpClient, string url, CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(
                    $"API error ({(int)response.StatusCode}): {errorContent}",
                    null,
                    response.StatusCode
                );
            }

            try
            {
                var data = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken) 
                    ?? throw new HttpRequestException("CoinGecko returned empty or invalid response.", null, response.StatusCode);
                return data;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Error deserializing CoinGecko response.", ex, response.StatusCode);
            }
        }
    }
}
