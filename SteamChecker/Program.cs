// 76561198766495481 - TOVARISCH OUUI
// 76561198737847080 - OUIKZ
// 76561198983532025 - VoSdUh
// 76561198739546386 - VoSdUhDS (KZ)
// 76561199023190441 - froks
// 76561199104543540 - Hamster44
// 76561199173118241 - Ishtar_1518
// 76561199749895389 - Ishtar_1517 (KZ)
// 76561199238917698 - Nion
// 76561199419663340 - Inn0_0kentiy

using SteamChecker;

var steamIdsDict = new Dictionary<string, string>()
{
    { "76561198766495481", "Дима" },
    { "76561198737847080", "Дима-казах" },
    { "76561198983532025", "Ваня" },
    { "76561198739546386", "Ваня-казах" },
    { "76561199023190441", "Макс" },
    { "76561199104543540", "Юра" },
    { "76561199173118241", "Влад" },
    { "76561199749895389", "Влад-казах" },
    { "76561199238917698", "Никита" },
    { "76561199419663340", "Миша" }
};

var gameExtraInfoDict = new Dictionary<string, string?>();

DotNetEnv.Env.Load();
var steamService = new SteamService();

using CancellationTokenSource cts = new CancellationTokenSource();

TimeSpan interval = TimeSpan.FromSeconds(5);
using PeriodicTimer timer = new PeriodicTimer(interval);

Console.WriteLine("Я запущен! Погнали чекать полбовчан...");

Task waitOffTask = Task.Run(() =>
{
    Console.ReadLine();
    cts.Cancel();
});

try
{
    while (await timer.WaitForNextTickAsync(cts.Token))
    {
        Console.WriteLine("Поиск...");
        await CheckSteamPlayersStatus(steamIdsDict.Keys.ToArray());
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Работа остановлена!");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

Console.WriteLine("Программа завершена. Покеда!");

async Task CheckSteamPlayersStatus(params string[] steamIds)
{
    var anybodyNewPlayed = false;
    
    var steamPlayers = await steamService.GetSteamPlayer(steamIds);

    if (gameExtraInfoDict.Count == 0)
    {
        foreach (var steamPlayer in steamPlayers)
        {
            gameExtraInfoDict.Add(steamPlayer.SteamId, steamPlayer.GameExtraInfo);
            
            if (steamPlayer.GameExtraInfo != null)
            {
                Console.WriteLine($"{steamIdsDict[steamPlayer.SteamId]} играет в {steamPlayer.GameExtraInfo}!");
                anybodyNewPlayed = true;
            }
        }
    }
    else
    {
        foreach (var steamPlayer in steamPlayers)
        {
            if (steamPlayer.GameExtraInfo is null)
            {
                gameExtraInfoDict[steamPlayer.SteamId] = null;
            }
            else if (gameExtraInfoDict[steamPlayer.SteamId] != steamPlayer.GameExtraInfo)
            {
                Console.WriteLine($"{steamIdsDict[steamPlayer.SteamId]} играет в {steamPlayer.GameExtraInfo}!");
                gameExtraInfoDict[steamPlayer.SteamId] = steamPlayer.GameExtraInfo;
                
                anybodyNewPlayed = true;
            }
        }
    }

    if (!anybodyNewPlayed)
    {
        Console.WriteLine("Никто не запустил игру.");
    }
}



// SteamPlayer steamPlayer = await steamService.GetSteamPlayer("76561198766495481");
// Console.WriteLine($"{steamPlayer.PersonaName}: {steamPlayer.SteamId}");

// // чисто чтобы найти SteamID полбовчан. удалить.
// List<SteamFriend> steamFriends = await steamService.GetFriendList("76561198737847080");
// foreach (var steamFriend in steamFriends)
// {
//     var player = await steamService.GetSteamPlayer(steamFriend.SteamId);
//     Console.WriteLine($"{player.SteamId} - {player.PersonaName}");
// }