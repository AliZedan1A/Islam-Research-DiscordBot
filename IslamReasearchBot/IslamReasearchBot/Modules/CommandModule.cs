using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using IslamReasearchBot;
using IslamReasearchBot.DataModules;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Channels;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;
using Discord.API;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace IslamReasearchBot.Modules
{

   

    public class CommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        public async Task<bool> CheckAdmin(ulong id ,ulong ServerID)
        {
            Database db = new Database();
            var z = db.ServersAdmins.ToList().FindAll(x => x.ServerId == ServerID);
            if (z.SingleOrDefault(x => x.UserId == id)==null) {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckResarchOwner(int id)
        {
            Database db =new Database();
            var z= db.ServerLog.SingleOrDefault(x=>x.Id== id);
            if (z == null)
            {
                return false;
            }
            if(z.UserSheredID == Context.User.Id)
            {
                return true;
            }
            return false;
        }
        [ComponentInteraction("select1")]
        public async Task select1(string[] selections)
        {
           
            Database db = new Database();
          var id =  selections.First();
            foreach (var selection in selections)
            {
                db.ServersCatigory.Add(new CatigoryIdsModule() { ServerId = Context.Guild.Id, CatigoryId = ulong.Parse(selection) });
            }

            db.ServersConfig.Add(new ServerConfigrationModuel() { IsActive = true ,ServerID=Context.Guild.Id });
            db.ServersAdmins.Add(new AdminsModule() { ServerId = Context.Guild.Id, UserId = Context.Guild.OwnerId });

            

            await db.SaveChangesAsync();
            await RespondAsync("تم تفعيل سيرفرك",ephemeral:true);

        }


        public async Task<ComponentBuilder> ShowUsedCatigory(string idName, ulong ServerId)
        {
            Database db = new Database();
            var menuCatigory = new SelectMenuBuilder()
.WithPlaceholder("Select an option")
.WithCustomId(idName)
.WithMinValues(1)
.WithMaxValues(1);
            foreach (var item in db.ServersCatigory.ToList().FindAll(x => x.ServerId == ServerId))
            {
                if (Context.Guild.CategoryChannels.SingleOrDefault(x => x.Id == item.CatigoryId )is null)
                {
                    continue;
                }
                else
                {
                    var catigory = Context.Guild.CategoryChannels.SingleOrDefault(x => x.Id == item.CatigoryId);
                    menuCatigory.AddOption(catigory.Name, catigory.Id.ToString());

                }

            }
            var builder = new ComponentBuilder()
.WithSelectMenu(menuCatigory);

            return builder;
        }

        public async Task<ComponentBuilder> ShowOpendCatigory(string idName, ulong ServerID)
        {
            Database db = new Database();
            var menuCatigory = new SelectMenuBuilder()
.WithPlaceholder("Select an option")
.WithCustomId(idName)
.WithMinValues(1)
.WithMaxValues(Context.Guild.CategoryChannels.Count);
            int num = 0;

            foreach (var item in Context.Guild.CategoryChannels)
            {
                if(db.ServersCatigory.ToList().FindAll(x=>x.ServerId== ServerID).SingleOrDefault(x=>x.CatigoryId==item.Id)  is null)
                {
                    Console.WriteLine(item.Id);
                    menuCatigory.AddOption(item.Name, item.Id.ToString());

                }
                else
                {
                    continue;

                }
            }

            var builder = new ComponentBuilder()
    .WithSelectMenu(menuCatigory);

            return builder;
        }
        public async Task<ComponentBuilder> OpenCatigory(string idName, ulong CatigoryID)
        {
            var menuCatigory = new SelectMenuBuilder()
.WithPlaceholder("Select an option")
.WithCustomId(idName)
.WithMinValues(1)
.WithMaxValues(1);
            int num = 0;

           
                if (Context.Guild.CategoryChannels.SingleOrDefault(x=>x.Id==CatigoryID )is null)
                {
                await RespondAsync("لم يتم ايجاد الكاتيجوري", ephemeral: true);


                }
                else
                {
                foreach(var item in  Context.Guild.CategoryChannels.SingleOrDefault(x => x.Id == CatigoryID).Channels)
                {
                    Context.Guild.CategoryChannels.SingleOrDefault(x => x.Id == CatigoryID);
                menuCatigory.AddOption(item.Name, item.Id.ToString());

                }


            }


            var builder = new ComponentBuilder()
    .WithSelectMenu(menuCatigory);

            return builder;

        }


        [SlashCommand("getallinfrmntion", "يرسل رسالة نصية فيها جميع الابحاث المرتبطة بسيرفر")]

        public async Task info()
        {
            ulong guildOwnerId = Context.Guild.OwnerId;
            if(Context.User.Id != guildOwnerId)
            {
                await RespondAsync("انت لا تملك صلاحيات لأستخدام هذا الامر", ephemeral: true);
                return;
            }
            if (CheckServeAction().Result == false)
            {
                await RespondAsync("الخادم غير مفعل!\n يجب على مالك الخادم تفعيله", ephemeral: true);
                return;
            }

            Database db = new Database();
            var infos = db.ServerLog.ToList().FindAll(x => x.ServerID == Context.Guild.Id);

            if (infos is null || infos.Count == 0)
            {
                await RespondAsync("لم يتم الاستعلام عن اي معلومات في هذا الخادم", ephemeral: true);
                return;
            }
            foreach (var item in infos)
            {
                Console.WriteLine("d");
                var embed = new EmbedBuilder
                {
                    Title = $"البحث المنشور في تاريخ {item.PublishDate}",
                    Description = $"{item.Title}",
                    Color = new Color(255, 0, 0),
                    ThumbnailUrl = "https://i0.wp.com/democraticac.de/wp-content/uploads/2016/06/%D8%A7%D9%84%D8%A8%D8%AD%D8%AB-%D8%A7%D9%84%D8%B9%D9%84%D9%85%D9%8A.jpg",

                    Fields =
            {
                new EmbedFieldBuilder
                {
                    Name = "محتوى البحث",
                    Value = item.Content,
                },

            },
                    Footer = new EmbedFooterBuilder
                    {
                        Text = $"بحث {Context.Client.GetUser((ulong)item.UserSheredID).Username}"
                    }
                };
                try
                {
                    await Context.Guild.Owner.SendMessageAsync(embed: embed.Build());

                }
                catch
                {
                    await RespondAsync("حصل مشكلة ما , يرجى التواصل مع المطور", ephemeral: true);
                    return;
                }
            }
            await RespondAsync("تم ارسال الابحاث على الخاص", ephemeral: true);




        }
        [SlashCommand("editresarch", "تعديل البحث")]

        public async Task EditResarch(int id, string Content = null, string Title = null, string Role = null)
        {
            if (Content is null && Title is null && Role is null)
            {
                await RespondAsync("انت لم تضع شيء لتعديله", ephemeral: true);
                return;
            }
            Database db = new Database();
            if (db.ServerLog.SingleOrDefault(x => x.Id == id) is null)
            {
                await RespondAsync("خطأ في الرقم التعريفي", ephemeral: true);

                return;
            }
            try
            {
                List<string> edtinglist = new List<string>();
                var Resarch = db.ServerLog.SingleOrDefault(x => x.Id == id);
                if (Content != null) { Resarch.Content = Content; edtinglist.Add("Content"); }
                if (Title != null) { Resarch.Title = Title; edtinglist.Add("Title"); }
                if (Role != null) { Resarch.RoleName = Role; edtinglist.Add("Role"); }
                string response = string.Empty;
                foreach( var item in edtinglist )
                {
                    response = response + " , ";
                    response = response + item;

                }
                db.SaveChanges();
                await RespondAsync($"تم تعديل \n {response} ", ephemeral: true);
            }
            catch(Exception ex) 
            {
                await RespondAsync("حصل خطأ اثناء عملية الحفظ تم ارسال الخطأ للمطور  , نعتذر عن الخطأ", ephemeral: true);
                var ali = await Context.Client.GetUserAsync(519943222668296222);
                string MesasgeToSend = $"An Error at {DateTime.UtcNow}\n Error : \n {ex.Message} ";
                await ali.SendMessageAsync(MesasgeToSend);
            }

        }
        public static List<ListIdsObj> ListIds = new List<ListIdsObj>();
        [SlashCommand("publish", "نشر البحث")]
        
        public async Task publish(int id)
        {
            Database db = new Database();
            if (db.ServerLog.SingleOrDefault(x => x.Id == id) is null)
            {
                await RespondAsync("خطأ في الرقم التعريفي", ephemeral: true);

                return;
            }

            if (CheckServeAction().Result == false)
            {
                await RespondAsync("الخادم غير مفعل!\n يجب على مالك الخادم تفعيله", ephemeral:true);
                return;
            }

            if (CheckResarchOwner(id).Result == true)
            {

            }
            else
            {
                await RespondAsync("هذا البحث ليس ملكك ! لا يمكنك نشره",ephemeral:true);
            }
            var Session = ListIds.SingleOrDefault(x => x.UserID == Context.User.Id);

           
            var Resarch = db.ServerLog.SingleOrDefault(x => x.Id == id);

            if (Resarch.UserSheredID != Context.User.Id)
            {
                await RespondAsync("هذا البحث ليس ملكك", ephemeral: true);

            }
            if (Session is null)
            {
                ListIdsObj listIdsObj = new ListIdsObj();
                listIdsObj.ReshID = id;
                listIdsObj.UserID = Context.User.Id;
                ListIds.Add(listIdsObj);

            }
            var z = ShowUsedCatigory("select2", Context.Guild.Id);
            await RespondAsync(ephemeral: true, components: z.Result.Build());



        }
        [RequireOwner]
        [SlashCommand("addnewcatigory", "اضافة رومات جديدة")]
        public async Task addcat()
        {
            if (CheckServeAction().Result == false)
            {
                await RespondAsync("الخادم غير مفعل!\n يجب على مالك الخادم تفعيله", ephemeral:true);
                return;
            }

            Database db = new Database();
            var menuCatigory = new SelectMenuBuilder()
.WithPlaceholder("Select an option")
.WithCustomId("select5")
.WithMinValues(1)
.WithMaxValues(1);
            int num = 0;

            foreach (var item in Context.Guild.CategoryChannels)
            {
                if (db.ServersCatigory.ToList().FindAll(x => x.ServerId == Context.Guild.Id).SingleOrDefault(x => x.CatigoryId == item.Id) is null)
                {
                    Console.WriteLine(item.Id);
                    menuCatigory.AddOption(item.Name, item.Id.ToString());

                }
                else
                {
                    continue;

                }
            }

            var builder = new ComponentBuilder()
    .WithSelectMenu(menuCatigory);

            await RespondAsync(ephemeral: true, components: builder.Build());
               return;
            
        }
        [ComponentInteraction("select5")]
        public async Task select5(string[] selections)
        {
            Database db = new Database();
            var id = selections.First();
            foreach (var selection in selections)
            {
                db.ServersCatigory.Add(new CatigoryIdsModule() { ServerId = Context.Guild.Id, CatigoryId = ulong.Parse(selection) });
            }
            await db.SaveChangesAsync();
            await RespondAsync("تم اضافة الرومات",ephemeral:true);
        }

            [SlashCommand("addresarch", "اضافة بحث")]
        public async Task MakeResearch()
        {
            if (CheckServeAction().Result == false)
            {
                await RespondAsync("الخادم غير مفعل!\n يجب على مالك الخادم تفعيله", ephemeral: true);
                return;
            }

            if (CheckAdmin(Context.User.Id,Context.Guild.Id).Result == true)
            {

            }
            else
            {
                await RespondAsync("يجب ان تكون من الادارة الخاصة بنشر الابحاث لنشر بحث",ephemeral:true);
            }

            var mb = new ModalBuilder()
.WithTitle("اضافة بحث")
.WithCustomId("res1")
.AddTextInput("ادخل عنوان البحث", "GetTitle", placeholder: "ResarchTitle", maxLength: 100, minLength: 1)
.AddTextInput("ادخل نص البحث", "GetContent", TextInputStyle.Paragraph, placeholder: "ResarchContent", maxLength: 2000)
.AddTextInput("ادخل الرتبة التي سيتم ذكرها(اختياري)ه", "Role",required:false, placeholder: "Minshion", maxLength: 100)



;

            await Context.Interaction.RespondWithModalAsync(mb.Build());


        }

       


    [ComponentInteraction("select2")]
        public async Task select2(string[] selections)
        {


            var z =  OpenCatigory("select3",ulong.Parse( selections.First()));
            await RespondAsync(ephemeral: true, components: z.Result.Build());


        }
        private string GetFileNameFromUrl(string url)
        {
            Uri uri = new Uri(url);
            string fileName = uri.Segments[uri.Segments.Length - 1];
            return fileName;
        }

        [ComponentInteraction("select3")]
        public async Task select3(string[] selections)
        {
            
            var Session = ListIds.SingleOrDefault(x=>x.UserID == Context.User.Id);
            if(Session == null)
            {
               await RespondAsync("انتهت صلاحية الامر يرجى اعادة كتابته",ephemeral:true) ;
            }
            Database db = new Database();
            if (db.ServerLog.SingleOrDefault(x => x.Id == Session.ReshID) is null)
            {
             await RespondAsync("خطأ في الرقم التعريفي", ephemeral: true);

                return;
            }
            var Resarch = db.ServerLog.SingleOrDefault(x => x.Id == Session.ReshID);
            if(Resarch.UserSheredID!=Context.User.Id)
            {
                await RespondAsync("هذا البحث ليس ملكك", ephemeral: true);

            }
            Resarch.ChannelID =ulong.Parse(selections.First());
            await db.SaveChangesAsync ();
            var sentMessage = await Context.Channel.SendMessageAsync("جار نشر البحث");



            var resh = db.ServerLog.SingleOrDefault(x => x.Id == Session.ReshID);
            var embedBuilder =  new EmbedBuilder();
            embedBuilder.WithTitle(@$"{resh.Title}");
            embedBuilder.WithDescription("**"+resh.Content+"**");
            EmbedFieldBuilder embedFieldBuilder = new EmbedFieldBuilder();
            embedFieldBuilder.WithName($"ناشر البحث ");
            embedFieldBuilder.WithValue(Context.Guild.Users.SingleOrDefault(x=>x.Id== resh.UserSheredID).Mention);
            embedFieldBuilder.IsInline = true;
            embedBuilder.WithColor(Color.DarkPurple);
            embedBuilder.WithFields(embedFieldBuilder);
          
                embedBuilder.WithThumbnailUrl(Context.Guild.IconUrl);

                embedBuilder.Timestamp = db.ServerLog.SingleOrDefault(x => x.Id == Session.ReshID).PublishDate;
            var f = Context.Guild.Roles.SingleOrDefault(x => x.Name == resh.RoleName);
            if(f is null)
            {
                await Context.Guild.GetTextChannel(ulong.Parse(selections.First())).SendMessageAsync(embed: embedBuilder.Build());

            }
            else
            {
                await Context.Guild.GetTextChannel(ulong.Parse(selections.First())).SendMessageAsync(text:@$"{f.Mention}", embed: embedBuilder.Build());

            }
            Resarch.IsPublish = true;
                await db.SaveChangesAsync();
            if (db.ResarchImages.ToList().FindAll(x => x.ResarchID == Session.ReshID) == null)
            {
                goto f;
            }

            foreach (var item in db.ResarchImages.ToList().FindAll(x => x.ResarchID == Session.ReshID))
            {

               

                using (var client = new WebClient())
                {
                    try
                    {
                        string pattern = @"\.(jpeg|jpg|gif|png)$";

                        if (!Regex.IsMatch(item.ImageURL.Split('?')[0], pattern, RegexOptions.IgnoreCase))
                        {
                            await Context.User.SendMessageAsync($@"{item.ImageURL} هذا الرابط غير صالح");
                            db.Remove(item);
                           await  db.SaveChangesAsync();
                            goto l;
                        }
                        var imageData = await client.DownloadDataTaskAsync(item.ImageURL);
                        var attachmentStream = new MemoryStream(imageData);
                        await Context.Guild.GetTextChannel(ulong.Parse(selections.First())).SendFileAsync(stream: attachmentStream, GetFileNameFromUrl(item.ImageURL), item.ImageTitle);
                        l:;

                    }
                    catch
                    {
                        await Context.User.SendMessageAsync($@"{item.ImageURL} هذا الرابط غير صالح");
                    }
                }


            }
        f:
            ListIds.Remove(Session);
           await  sentMessage.ModifyAsync(x=>x.Content= $"تم نشر البحث الخاص ب {Context.User.Mention}");

        }
        [RequireUserPermission(GuildPermission.Administrator)]//admin
        [SlashCommand("showalladmins", "عرض كل الادارة")]
        public async Task ShowAllAdmins()
        {
            if (CheckServeAction().Result == false)
            {
                await RespondAsync("الخادم غير مفعل!\n يجب على مالك الخادم تفعيله", ephemeral: true);
                return;
            }
            Database db = new Database();
            var admins = db.ServersAdmins.ToList().FindAll(x => x.ServerId == Context.Guild.Id);
            if(admins.Count == 0)
            {
                await RespondAsync("لا يوجد ادارة في الخادم الخاص بك",ephemeral: true);
            }
            List< EmbedFieldBuilder > filds = new List< EmbedFieldBuilder >();
            foreach (var item in admins)
            {

                var username = Context.Guild.GetUser(item.UserId).Username;
                string fildshow="غير موجود في السيرفر";
                if(username != null )
                {
                    fildshow = username;

                }
                filds.Add(new EmbedFieldBuilder
                {
                    Name = item.UserId.ToString(),
                    Value = fildshow,
                });

            }

            var embed = new EmbedBuilder
            {
                Title = "ألأدارة",
                Description = "ألأدارة الخاصة في الخادم",
                Color = new Color(255, 0, 0), 
                ThumbnailUrl = "https://png.pngtree.com/element_pic/17/08/04/3528c203fea0c31986221c51e598a6d4.jpg",

            Fields = filds,



                Footer = new EmbedFooterBuilder
                {
                    Text = "test"
                }
            };
            await RespondAsync(embed:embed.Build(),ephemeral:true);

        }
        [RequireUserPermission(GuildPermission.Administrator)]//admin
        [SlashCommand("removeadmin", "اضافة ادارة")]
        public async Task RemoveAdmin(SocketUser user)
        {
            if (CheckServeAction().Result == false)
            {
                await RespondAsync("الخادم غير مفعل!\n يجب على مالك الخادم تفعيله", ephemeral: true);
                return;
            }
            Database db = new Database();
            var admin = db.ServersAdmins.ToList().FindAll(x => x.ServerId == Context.Guild.Id).SingleOrDefault(x => x.UserId == user.Id);
            if (admin == null)
            {
                await RespondAsync("هذا الادمن غير موجود", ephemeral: true);
                return;

            }
            else
            {
                try
                {
                    db.ServersAdmins.Remove(admin);
                    db.SaveChanges();
                    await RespondAsync($"تم ازالة  الاداري صاحب الرقم التعريفي : \n {admin.UserId}", ephemeral: true);

                }
                catch(Exception ex)
                {
                    await RespondAsync("حصل خطأ اثناء عملية الحفظ تم ارسال الخطأ للمطور  , نعتذر عن الخطأ", ephemeral: true);
                    var ali = await Context.Client.GetUserAsync(519943222668296222);
                    string MesasgeToSend = $"An Error at {DateTime.UtcNow}\n Error : \n {ex.Message} ";
                    await ali.SendMessageAsync(MesasgeToSend);


                }

            }

        }
        [RequireUserPermission(GuildPermission.Administrator)]//admin
        [SlashCommand("addadmin", "اضافة ادارة")]
        public async Task addadmin(SocketUser user)
        {
            if (CheckServeAction().Result == false)
            {
                await RespondAsync("الخادم غير مفعل!\n يجب على مالك الخادم تفعيله", ephemeral: true);
                return;
            }

            Database db = new Database();
            if(db.ServersAdmins.ToList().FindAll(x=>x.ServerId==Context.Guild.Id).SingleOrDefault(x=>x.UserId==user.Id)!=null)
            {
                await RespondAsync("هذا الادمن موجود", ephemeral: true);
                return;

            }
            db.ServersAdmins.Add(new AdminsModule() { ServerId = Context.Guild.Id, UserId = user.Id });
            await db.SaveChangesAsync();
            await RespondAsync("تم اضافة الادمن",ephemeral: true);
        }

        [SlashCommand("removeresarchimages", "حذف صور البحث")]
        public async Task RemoveImage(int ResarchID)
        {
            Database db = new Database();

            if (CheckServeAction().Result == false)
            {
                await RespondAsync("الخادم غير مفعل!\n يجب على مالك  الخادم تفعيله", ephemeral: true);
                return;
            }
            if (CheckResarchOwner(ResarchID).Result == true)
            {
               var list=  db.ResarchImages.ToList().FindAll(x=>x.ResarchID==ResarchID);
                foreach(var item in list)
                {
                    db.ResarchImages.Remove(item);
                }
                await RespondAsync("تم حذف جميع الصور الخاصة ببحثك", ephemeral: true);
            }
            else
            {
                await RespondAsync("هذا البحث ليس ملكك ! لا حذف صور منه");
            }

        }
            [SlashCommand("addresarchimage", "اضافة صورة للبحث")]
        public async Task AddImage(int ResarchID,string URL,string ImageTitle)
        {
            if(CheckServeAction().Result == false)
            {
               await RespondAsync("الخادم غير مفعل!\n يجب على مالك  الخادم تفعيله", ephemeral: true);
                return;
            }
            if (CheckResarchOwner(ResarchID).Result == true)
            {

            }
            else
            {
                await RespondAsync("هذا البحث ليس ملكك ! لا يمكنك اضافة صور له");
            }
            string pattern = @"\.(jpeg|jpg|gif|png)$";

            if (!Regex.IsMatch(URL, pattern, RegexOptions.IgnoreCase))
            {
                await RespondAsync("الرابط غير صالح",ephemeral:true);
                return;
            }

            ImagesModualDB f = new ImagesModualDB();

            f.ImageURL= URL;    
            f.ResarchID= ResarchID;
            f.ImageTitle= ImageTitle;
            Database db = new Database();
            db.ResarchImages.Add(f);
            await db.SaveChangesAsync();
            await RespondAsync(@$"تم اضافة الصورة", ephemeral: true) ;
        }

        public async Task<bool> CheckServeAction()
        {

            Database db = new Database();
            if (db.ServersConfig.FirstOrDefault(x => x.ServerID == Context.Guild.Id) is null)
            {
                return false;
            }
            return true;
                
        }
        [SlashCommand("removeresearch", "حذف بحث معين")]
        public async Task removeresearch(int researchid)
        {
           var z= CheckResarchOwner(researchid);
            if(z.Result==false)
            {
               await RespondAsync(ephemeral: true,text:"هذا البحث ليس ملكك!");
                return;
            }
            Database db = new Database();
            db.Remove( db.ServerLog.SingleOrDefault(x=>x.Id==researchid));
            await db.SaveChangesAsync();
            await RespondAsync(ephemeral:true,text:"تم حذف البحث");

        }
            [SlashCommand("myresearch", "الاستعلام عن ابحاثك")]
        public async Task getalluserresh()
        {
            Database db = new Database();
             var list = db.ServerLog.ToList().FindAll(x => x.UserSheredID == Context.User.Id);
            if(list==null)
            {
               await RespondAsync("انت لا تملك ابحاث !",ephemeral: true);
                return;
            }
            var embedBuilder = new EmbedBuilder();
            embedBuilder.WithTitle(@$"قائمة ألأبحاث الخاصة بك");
            embedBuilder.WithDescription(@$"عدد ابحاثك : {list.Count}
سيتم عرض الابحاث العنوان وتحته الرقم التعريفي");
            embedBuilder.WithColor(Color.Red);
            embedBuilder.WithThumbnailUrl("https://png.pngtree.com/png-clipart/20210713/ourmid/pngtree-cool-free-islamic-decorations-png-image_3585054.jpg");

            foreach (var item in list) {

                embedBuilder.AddField(new EmbedFieldBuilder()
                {
                    Name= "العنوان "+item.Title,
                    Value= "الرقم التعريفي "+ item.Id.ToString(),
                });


            }

            await RespondAsync(embed: embedBuilder.Build(), ephemeral: true);

        }
     
            [RequireUserPermission(GuildPermission.Administrator)]//Owner
        [SlashCommand("activeserver", "فعل سيرفرك")]
        public async Task activeserver()
        {
            Database db = new Database();
            if ( db.ServersConfig.FirstOrDefault(x => x.ServerID == Context.Guild.Id) is null)
            {
                await RespondAsync(ephemeral:true,components: ShowOpendCatigory("select1", Context.Guild.Id).Result.Build());
                return;
            }
            var server = db.ServersConfig.SingleOrDefault(x => x.ServerID == Context.Guild.Id);

            if (server.IsActive = true)
            {
                await RespondAsync("الخادم  مفعل !",ephemeral: true);
            }
            else
            {
                await RespondAsync(ephemeral: true, components: ShowOpendCatigory("select1", Context.Guild.Id).Result.Build());

            }


        }

    }
    public class ListIdsObj
    {
        public ulong UserID { get; set; }
        public int ReshID { get; set; }
    }
        public class ResarchModelWithImage : IModal
    {
        public string Title => "اضافة بحث";
        [InputLabel("ادخل عنوان البحث")]
        [ModalTextInput("greeting_input", TextInputStyle.Short, placeholder: "ResarchTitle", maxLength: 30)]
        public string ResarchTitle { get; set; }
        [InputLabel("ادخل نص البحث")]
        [ModalTextInput("greeting_APIKEY", TextInputStyle.Paragraph, placeholder: "ResarchContent", maxLength: 2000)]
        public string ResarchContent { get; set; }
        
        [InputLabel("اضافة صور")]
        [ModalTextInput("greeting_Gmail", TextInputStyle.Paragraph, placeholder: "resarchImages", maxLength: 2000)]
        public string resarchImages { get; set; }

        

    }
   
}
