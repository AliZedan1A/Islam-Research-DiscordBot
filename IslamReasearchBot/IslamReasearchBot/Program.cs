using Discord.Interactions;
using Discord.WebSocket;
using IslamReasearchBot.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Discord.Commands;
using Discord;
using System.Reflection;
using System.Diagnostics;

namespace IslamReasearchBot
{

    public class program
    {
        public DiscordSocketClient _client;

        public static Task Main(string[] args) => new program().MainAsync();


        public async Task MainAsync()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .Build();
            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>

            services
            .AddSingleton(config)
            .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All,
                LogGatewayIntentWarnings = false,
                AlwaysDownloadUsers = true,
                LogLevel = LogSeverity.Debug,
                UseInteractionSnowflakeDate =false
               
            }))
            
            .AddTransient<ConsoleLogger>()
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<InteractionHandler>()
            .AddSingleton(x => new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Debug,
                DefaultRunMode = Discord.Commands.RunMode.Async
            }))
            .AddSingleton<PrefixHandler>())
            .Build();

            await RunAsync(host);
        }
        public async Task RunAsync(IHost host)
        {
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            var commands = provider.GetRequiredService<InteractionService>();
            _client = provider.GetRequiredService<DiscordSocketClient>();
            var config = provider.GetRequiredService<IConfigurationRoot>();

            await provider.GetRequiredService<InteractionHandler>().InitializeAsync();

            var prefixCommands = provider.GetRequiredService<PrefixHandler>();
            prefixCommands.AddModule<IslamReasearchBot.Modules.PrefixModule>();
            await prefixCommands.InitializeAsync();


            _client.Log += _ => provider.GetRequiredService<ConsoleLogger>().Log(_);


            commands.Log += _ => provider.GetRequiredService<ConsoleLogger>().Log(_);

            _client.Ready += async () =>
            {

                if (IsDebug())
                    await commands.RegisterCommandsGloballyAsync(false);
                else
                    await commands.RegisterCommandsGloballyAsync(false);
            };


            await _client.LoginAsync(Discord.TokenType.Bot, "token");
            await _client.StartAsync();
            _client.UserJoined += serverjoined;
            await Task.Delay(-1);
        }
        public async Task serverjoined(SocketGuildUser user)
        {
            Console.WriteLine(user.Id);
        }

        static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
