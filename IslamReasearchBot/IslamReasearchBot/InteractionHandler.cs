using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using IslamReasearchBot.DataModules;
using Newtonsoft.Json;
using System.Reflection;

namespace IslamReasearchBot
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;

        public InteractionHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.ModalSubmitted += async modal =>
            {
                List<SocketMessageComponentData> components =
                    modal.Data.Components.ToList();
                
                string Title = components
                    .First(x => x.CustomId == "GetTitle").Value;
                string Content = components
                    .First(x => x.CustomId == "GetContent").Value;
                string Role = components
    .First(x => x.CustomId == "Role").Value;


                string message = @$" Title :{Title}, \n {Content}";
                ServersLog Resh = new ServersLog();
                Resh.ServerID =  modal.GuildId;
                Resh.PublishDate = DateTime.UtcNow;
                Resh.Content = Content;
                Resh.Title = Title;
                Resh.ChannelID = 1;
                Resh.IsPublish= false;
                Resh.UserSheredID = modal.User.Id;
                Resh.RoleName = Role ;
                Database db = new Database();
                db.ServerLog.Add(Resh);
                await db.SaveChangesAsync();
                var id = db.ServerLog.ToList().FindAll(x => x.ServerID == modal.GuildId).Last().Id;
                var embedBuilder = new EmbedBuilder();
                string text = @$"
تم اضافة بحثك الى قاعدة البيانات الرقم التعريفي للبحث هو ***{id}***
    ";

                embedBuilder.WithTitle(@$"تم اضافة البحث");
                EmbedFieldBuilder embedFieldBuilder2 = new EmbedFieldBuilder();
                embedFieldBuilder2.WithName($"/addresarchimage قم بكتابة الامر  ");
                embedFieldBuilder2.WithValue("يمكن اضافة صور للبحث عبر كتابته");

                EmbedFieldBuilder embedFieldBuilder = new EmbedFieldBuilder();
                embedFieldBuilder.WithName($"/publishقم بكتابة الامر  ");
                embedFieldBuilder.WithValue("وضع الرقم التعريفي  من اجل نشر البحث الخاص بك");
                embedFieldBuilder.IsInline = true;
                embedBuilder.AddField(embedFieldBuilder);
                embedBuilder.AddField(embedFieldBuilder2);
                embedBuilder.WithDescription("**"+ text + "**");
                embedBuilder.WithColor(Color.Red);
                embedBuilder.WithThumbnailUrl("https://png.pngtree.com/png-clipart/20210713/ourmid/pngtree-cool-free-islamic-decorations-png-image_3585054.jpg");
                await modal.RespondAsync(embed: embedBuilder.Build(), ephemeral: true);

            }
            ;
            _client.InteractionCreated += HandleInteraction;

            _commands.SlashCommandExecuted += SlashCommandExecuted;
            _commands.ContextCommandExecuted += ContextCommandExecuted;
            _commands.ComponentCommandExecuted += ComponentCommandExecuted;
        }

        private Task ComponentCommandExecuted(ComponentCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            return Task.CompletedTask;
        }

        private Task ContextCommandExecuted(ContextCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            return Task.CompletedTask;
        }
        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                var ctx = new SocketInteractionContext(_client, arg);
                await _commands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                if (arg.Type == InteractionType.ApplicationCommand)
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}
