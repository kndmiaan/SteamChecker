using SteamChecker;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;

DotNetEnv.Env.TraversePath().Load();

var steamService = new SteamService(DotNetEnv.Env.GetString("STEAM_API_KEY"));
using var cts = new CancellationTokenSource();
var timeSpan = new TimeSpan(0, 0, 2);
using var timer = new PeriodicTimer(timeSpan);

var botToken = DotNetEnv.Env.GetString("BOT_TOKEN");
var bot = new TelegramBotClient(botToken, null, cts.Token);

var chatId = DotNetEnv.Env.GetString("CHAT_ID");

Dictionary<string, string> players;
try
{
    players = await TxtReader.ReadKeyValue("steamIds.txt");
}
catch (FileNotFoundException)
{
    Commands.Error("File 'steamIds.txt is not found (check in the folder 'SteamChecker')!");
    return;
}
catch (FormatException ex)
{
    Commands.Error(ex.Message);
    return;
}

if (players.Count == 0) Commands.Warning("You haven't entered any Steam ID in 'steamIds.txt'");

Dictionary<string, string?> lastGames = new Dictionary<string, string?>();
foreach (string id in players.Keys)
{
    lastGames[id] = null;
}

Console.WriteLine("SteamChecker has launched!");

Task waitOffTask = Task.Run(() =>
{
    Console.ReadLine();
    cts.Cancel();
});

try
{
    while (await timer.WaitForNextTickAsync(cts.Token))
    {
        try
        {
            await CheckPlayersStatusAsync();
        }
        catch (Exception ex) when (ex is HttpRequestException || ex is RequestException)
        {
            Commands.Error(ex.Message);
        }
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Program has finished. Bye!");
}


async Task CheckPlayersStatusAsync()
{
    if (players.Count == 0) return;

    List<SteamPlayer> steamPlayers = await steamService.GetPlayerSummariesAsync(players.Keys.ToArray());

    foreach (SteamPlayer player in steamPlayers)
    {
        if (player.GameExtraInfo is not null && lastGames[player.SteamId] != player.GameExtraInfo)
        {
            await bot.SendMessage(chatId, $"<b>{players[player.SteamId]}</b> играет в <b>{player.GameExtraInfo}</b>!", ParseMode.Html);
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}]: {players[player.SteamId]} играет в {player.GameExtraInfo}!");
        }

        lastGames[player.SteamId] = player.GameExtraInfo;
    }
}