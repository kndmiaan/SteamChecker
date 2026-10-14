namespace SteamChecker.Core
{
    public static class TxtReader
    {
        private static string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        public static async Task<Dictionary<string, string>> ReadKeyValue(string fileName)
        {
            string path = Path.Combine(baseDir, fileName);
            using StreamReader reader = new(path, true);

            Dictionary<string, string> dict = new();

            while (await reader.ReadLineAsync() is { } line)
            {
                string[] splittedLine = line.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries);
                if (splittedLine.Length != 2) throw new FormatException($"{fileName}: Incorrect 'key value' format!");
                else dict[splittedLine[0]] = splittedLine[1];
            }

            return dict;
        }
    }
}
