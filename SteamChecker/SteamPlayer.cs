using System.Text.Json.Serialization;

namespace SteamChecker;

public class ResponseWrapper
{
    [JsonPropertyName("response")]
    public required SteamPlayersContainer? Response { get; set; }
}

public class SteamPlayersContainer
{
    [JsonPropertyName("players")]
    public required List<SteamPlayer>? Players { get; set; }
}

public class SteamPlayer
{
    [JsonPropertyName("steamid")]
    public required string SteamId { get; set; }
    
    [JsonPropertyName("personaname")]
    public required string PersonaName { get; set; }
    
    [JsonPropertyName("personastate")]
    public int PersonaState { get; set; }
    
    [JsonPropertyName("gameextrainfo")]
    public string? GameExtraInfo { get; set; }
}