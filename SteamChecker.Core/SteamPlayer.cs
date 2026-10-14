namespace SteamChecker.Core;

public class ResponseWrapper
{
    public required SteamPlayersContainer Response { get; set; }
}

public class SteamPlayersContainer
{
    public required List<SteamPlayer> Players { get; set; }
}

public class SteamPlayer
{
    public required string SteamId { get; set; }
    //public required string PersonaName { get; set; }
    //public int PersonaState { get; set; }
    public string? GameExtraInfo { get; set; }
}