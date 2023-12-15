using Discord;

namespace IslamReasearchBot.Logger
{
    public interface ILogger
    {
        public Task Log(LogMessage message);
    }
}
