# SteamChecker

**SteamChecker** is a command-line utility that notifies you about a player's activity on Steam. As soon as the player launches a game, the console will immediately notify you!

## Requirements
- [Steam API Key](https://steamcommunity.com/dev/apikey)
- [.NET SDK 10.0 or higher](https://dotnet.microsoft.com/download)

## Installation and Setup
1. Clone the repository:
```bash
git clone https://github.com/kndmiaan/SteamChecker.git
cd SteamChecker
```

2. Build the project:
```bash
dotnet build --configuration Release
```

## Configuration
1. Open the `.env.example` file in the project folder, insert your Steam API key, and then rename the file to `.env`.
2. In the same folder, locate `steamIds.txt` and enter the Steam ID and player name in the format: `SteamID Name` (separated by a space).

## Running the Application
```bash
dotnet run --project SteamChecker.csproj
```
Or navigate to the binary files directory:
```bash
cd SteamChecker/bin/Release/net10.0/
```
And run the executable file:
- **Windows:** `SteamChecker.exe`
- **Linux/macOS:** `./SteamChecker`
