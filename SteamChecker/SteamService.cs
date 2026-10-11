using System.Text.Json;

namespace SteamChecker
{
    internal class SteamService
    {
        private static readonly HttpClient _httpClient = new();
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly string _steamApi;

        public SteamService(string steamApi)
        {
            _steamApi = steamApi;
        }

        public async Task<List<SteamPlayer>> GetPlayerSummariesAsync(params string[] steamIds)
        {
            if (steamIds.Length == 0)
            {
                return new List<SteamPlayer>();
            }

            var ids = string.Join(",", steamIds);
            string body = await _httpClient.GetStringAsync(
                $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_steamApi}&steamids={ids}");

            ResponseWrapper? response = JsonSerializer.Deserialize<ResponseWrapper>(body, _options);

            if (response is null) throw new InvalidOperationException("Deserialization returned a null response");

            return response.Response.Players;
        }
    }
}
