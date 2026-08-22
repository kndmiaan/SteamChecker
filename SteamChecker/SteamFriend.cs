using System.Text.Json.Serialization;

namespace SteamChecker;

public class FriendsListWrapper
{
    [JsonPropertyName("friendslist")]
    public SteamFriendsContainer FriendsList { get; set; }
}

public class SteamFriendsContainer
{
    [JsonPropertyName("friends")]
    public List<SteamFriend> Friends { get; set; }
}

public class SteamFriend
{
    [JsonPropertyName("steamid")]
    public string SteamId { get; set; }
}