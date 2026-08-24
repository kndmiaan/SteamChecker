# Steam Checker

A console application that checks a player’s current status on Steam at regular intervals and displays new information about which game they are playing.

The program requires the user to provide the SteamID of the players whose status it will check every 5 seconds.
The console will only output information if the player has entered a NEW game. The console does not notify when a player leaves a game.

---

### IMPORTANT ❗
The program also requires a Steam API key. You can get one [here](https://steamcommunity.com/dev/apikey).
Find the [.env.example](SteamChecker/.env.example) file and enter your key. After that, be sure to rename the file to .env.
