using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IslamReasearchBot.Logger;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IslamReasearchBot
{
    public class PrefixHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;


        public PrefixHandler(DiscordSocketClient client, CommandService commands, IConfigurationRoot config)
        {
            _commands = commands;
            _client = client;
            _config = config;
        }

        public async Task InitializeAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
        }
        public void AddModule<T>()
        {
            _commands.AddModuleAsync<T>(null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {

            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            if(message.Content=="بوت هل انت مسلم؟")
            {
                await message.ReplyAsync("نعم الحمدلله");
            }   
            int argPos = 0;
            SocketGuildUser socketGuildUser = message.Author as SocketGuildUser;
            if (!(message.HasStringPrefix("^", ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }
    }
}
