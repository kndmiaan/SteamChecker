using System.Text.Json;

namespace SteamChecker;

public class SteamService
{
    // в документации он всегда статичный (из-за того, что лучше использовать один и тот же HttpClient).
    private static readonly HttpClient HttpClient = new();
    
    private readonly string _steamApiKey = Environment.GetEnvironmentVariable("STEAM_API_KEY") 
                                           ?? throw new InvalidOperationException("steamApiKey is null");
    
    public async Task<List<SteamPlayer>> GetSteamPlayer(params string[] steamId)
    {
        if (steamId.Length == 0)
            return new List<SteamPlayer>();
        
        string steamIds = string.Join(",", steamId);
        
        string? body = await HttpClient.GetStringAsync(
            $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_steamApiKey}&steamids={steamIds}");
        
        ResponseWrapper? response = JsonSerializer.Deserialize<ResponseWrapper>(body);
        List<SteamPlayer>? steamPlayers = response?.Response?.Players;
        
        if (steamPlayers is null)
            throw new HttpRequestException("no response or players not found");
        return steamPlayers;
    }

    // public async Task<List<SteamFriend>> GetFriendList(string steamId)
    // {
    //     string? body = await HttpClient.GetStringAsync($"https://api.steampowered.com/ISteamUser/GetFriendList/v0001/?key={_apiKey}&steamid={steamId}&relationship=friend");
    //     
    //     FriendsListWrapper? friendsList = JsonSerializer.Deserialize<FriendsListWrapper>(body);
    //     List<SteamFriend>? steamFriends = friendsList?.FriendsList?.Friends;
    //     
    //     if (steamFriends is null) throw new Exception("no response or friends not found");
    //     return steamFriends;
    // }
}