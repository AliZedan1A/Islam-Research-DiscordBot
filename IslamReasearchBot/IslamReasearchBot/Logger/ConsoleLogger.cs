using Discord;


namespace IslamReasearchBot.Logger
{
    public class ConsoleLogger : Logger
    {
        public override async Task Log(LogMessage message)
        {
            Task.Run(() => LogToConsoleAsync(this, message));
        }

        private async Task LogToConsoleAsync<T>(T logger, LogMessage message) where T : ILogger
        {
            Console.WriteLine($"guid:{_guid} : " + message);
        }
    }
}
