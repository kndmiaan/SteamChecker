using SteamChecker;

var gameExtraInfoDict = new Dictionary<string, string?>();

DotNetEnv.Env.Load(); // подгрузка данных из '.env'.
var steamService = new SteamService();

using CancellationTokenSource cts = new CancellationTokenSource();

// начало программы.
var steamIds = await SetPlayersToCheck();
if (steamIds.Count == 0)
{
    Console.WriteLine("Вы ничего не ввели! Мне незачем работать. Покеда!");
    return;
}
else
{
    Console.WriteLine("Я запущен! Погнали чекать...");
}

Task waitOffTask = Task.Run(() =>
{
    Console.ReadLine();
    cts.Cancel();
});

TimeSpan interval = TimeSpan.FromSeconds(5);
using PeriodicTimer timer = new PeriodicTimer(interval);

try
{
    while (await timer.WaitForNextTickAsync(cts.Token))
    {
        Console.WriteLine("Поиск...");
        await CheckSteamPlayersStatus(steamIds);
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

async Task<List<string>> SetPlayersToCheck()
{
    List<string> steamIds = new();
    
    Console.WriteLine("Ввод SteamID игроков, которых вы хотите проверять.");

    while (true)
    {
        Console.WriteLine("*введите 'выход' как закончите ввод игроков...*");
        
        Console.Write("SteamID: ");
        string? steamId = Console.ReadLine();
        
        if (string.IsNullOrEmpty(steamId))
        {
            Console.WriteLine("Введите хоть что-то!");
            continue;
        }
        else if (steamId.ToLower() == "выход")
        {
            break;
        }

        if (steamIds.Contains(steamId))
        {
            Console.WriteLine("Этот игрок уже добавлен!");
            continue;
        }
        
        try
        {
            SteamPlayer? steamPlayer = (await steamService.GetSteamPlayer(steamId)).FirstOrDefault();
            if (steamPlayer is null)
            {
                Console.WriteLine("Игрока с таким SteamID не существует!");
            }
            else
            {
                steamIds.Add(steamId);
                Console.WriteLine($"{steamPlayer.PersonaName} добавлен!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Что-то пошло не так...");
            Console.WriteLine(e.Message);
        }
    }
    
    Console.WriteLine("Ввод игроков закончен!");
    return steamIds;
}

async Task CheckSteamPlayersStatus(List<string> steamIds)
{
    var anybodyNewPlayed = false;
    var steamPlayers = await steamService.GetSteamPlayer(string.Join(",", steamIds));

    foreach (var steamPlayer in steamPlayers)
    {
        if (gameExtraInfoDict.TryGetValue(steamPlayer.SteamId, out string? previousGame))
        {
            if (steamPlayer.GameExtraInfo is null)
            {
                gameExtraInfoDict[steamPlayer.SteamId] = null;
            }
            else if (previousGame != steamPlayer.GameExtraInfo)
            {
                Console.WriteLine($"{steamPlayer.PersonaName} играет в {steamPlayer.GameExtraInfo}!");
                gameExtraInfoDict[steamPlayer.SteamId] = steamPlayer.GameExtraInfo;
                
                anybodyNewPlayed = true;
            }
        }
        else
        {
            gameExtraInfoDict.Add(steamPlayer.SteamId, steamPlayer.GameExtraInfo);
            
            if (steamPlayer.GameExtraInfo != null)
            {
                Console.WriteLine($"{steamPlayer.PersonaName} играет в {steamPlayer.GameExtraInfo}!");
                anybodyNewPlayed = true;    
            }
        }
    }

    if (!anybodyNewPlayed)
    {
        Console.WriteLine("Никто не запустил новую игру.");
    }
}