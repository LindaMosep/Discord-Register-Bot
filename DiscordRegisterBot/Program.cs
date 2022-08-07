using Discord;                //discord
using Discord.Commands;       //discord
using Discord.WebSocket;     //discord
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace DiscordRegisterBot
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>


        #region Values
        public static DiscordSocketClient _client;
        public static CommandService _commands;
        public static IServiceProvider _services;
        public static bool isLogged = false;
        #endregion
        static IGuild server;

        #region Update
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());


        }

        public static async Task MainAsync()
        {

            Console.WriteLine("Started");
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();
            _client.Log += Log;
            var token = "";
            var cfg = new DiscordSocketConfig();
            cfg.GatewayIntents |= GatewayIntents.GuildMembers;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            _client.MessageReceived += MessageRecieve;

            await RegisterCommandAsync();
            server =_client.GetGuild(0);

            _client.UserJoined += Client_UserJoined;

            _client.MessageReceived += GiveBaseRoles;

            _client.MessageReceived += CreateTicket;


            _client.ChannelCreated += _client_ChannelCreated;

            _client.ButtonExecuted += JoinChannelButton;
            await Task.Delay(-1);

        }

        private static Task JoinChannelButton(SocketMessageComponent e)
        {
         //   if(e.Data.CustomId.Contains(e.User.Id.ToString()))
         //   {
         //      var url = e.Data.CustomId.Substring(e.User.Id.ToString().Length + 1);
         //       var psi = new ProcessStartInfo
         //       {
         //          
         //         
         //           FileName = "Discord.exe",
         //           UseShellExecute = true
         //       };
         //       Process.Start(psi);
         //   }

            return Task.CompletedTask;
        }

        private static Task _client_ChannelCreated(SocketChannel e)
        {
            var channel = e as SocketTextChannel;
            if (channel != null)
            {
                if (channel.CategoryId == 915241317989253121)
                {
    #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    DestroyTimer(channel);
    #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
            return Task.CompletedTask;
        }

        private static async Task MyMenuHandler(SocketMessageComponent e)
        {
            //   Console.WriteLine("A");
            //  if(e.Channel.Id == 915232814537015367)
            //   {
            //
            //       if(e.User.Id == m.Author.Id)
            //       {
            //           var text = string.Join(", ", e.Data.Values);
            //
            //           Console.WriteLine(text);
            //           //await e.FollowupAsync("u selected " + e.Data.Values);
            //
            //       }
            //
            //   }
            //

            await Task.Delay(1);

        }

        private static async Task CreateTicket(SocketMessage e)
        {
            if (e.Channel.Id == 915232814537015367)
            {
                if (!e.Author.IsBot)
                {
                    if (e.Content == ".t")
                    {
                        var chnl = e.Channel as SocketGuildChannel;
       

                        var role = chnl.Guild.Roles.ToList().Find(m => m.Id == 915242713836847116) as IRole;
                        var newchnltemp = await chnl.Guild.CreateTextChannelAsync("ticket_" + e.Author.Discriminator, prop => prop.CategoryId = 915241317989253121);


                        var category = await newchnltemp.GetCategoryAsync();
                        await newchnltemp.AddPermissionOverwriteAsync(e.Author as IUser, category.GetPermissionOverwrite(role).Value);


                        var user = e.Author as IGuildUser;


                        try

                        {
                         
                            var invitelink = await newchnltemp.CreateInviteAsync();
                            var Button = new ComponentBuilder().WithButton("Join channel ",null, ButtonStyle.Link, null,invitelink.Url);
                        
                       
                          var message = await e.Channel.SendMessageAsync("Join channel with using this button! " + e.Author.Mention, false, null, null, null, new MessageReference(e.Id), component: Button.Build());
                          
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                         
                        }


                        await Task.Delay(1);

                    }

                }

            }


          



        }

        public static async Task DestroyTimer(SocketGuildChannel m)
        {
            bool ismessagecreated = false;


            _client.MessageReceived += async (e) =>
            {
                if (e.Channel.Id == m.Id)
                {
                    
                    ismessagecreated = true;
                    await Task.Delay(1);
                }
            };

            while (!ismessagecreated)
            {

                await Task.Delay(3 * 1000);

                if (ismessagecreated)
                {

                    ismessagecreated = false;

                }
                else
                {
                  
                    await m.DeleteAsync();
     
                    Task.CompletedTask.Wait();
                }
            }



            Task.CompletedTask.Wait();




        }



        public static CancellationTokenSource tokenSource = new CancellationTokenSource();

        private static async Task GiveBaseRoles(SocketMessage e)
        {
            if (e.Channel.Id == 910683443128569877)
            {

                if (e.Content.StartsWith(".k ") || e.Content.StartsWith(".e "))
                {

                    var chnl = e.Channel as SocketGuildChannel;
                    var author = e.Author as SocketGuildUser;

                    var man = chnl.Guild.Roles.ToList().Find(m => m.Id == 910683360744058940);
                    var woman = chnl.Guild.Roles.ToList().Find(m => m.Id == 910683359074713630);
                    var citizen = chnl.Guild.Roles.ToList().Find(m => m.Id == 910683364602830929);
                    var registerchnl = chnl.Guild.GetTextChannel(910683508098359296);
                    if (author.Roles.ToList().Contains(author.Roles.ToList().Find(m => m.Id == 910683369338200114)) || author.GuildPermissions.ManageRoles)
                    {
                        if (e.MentionedUsers.Count > 0)
                        {
                            if (e.Content.StartsWith(".e "))
                            {


                                for (int i = 0; i < e.MentionedUsers.Count; i++)
                                {

                                    var user = e.MentionedUsers.ToList()[i] as SocketGuildUser;

                                   
                                    if (!user.Roles.Contains(man))
                                    {

                                        if (user.Roles.Contains(woman))
                                        {
                                            Console.WriteLine("A");
                                            await user.RemoveRoleAsync(woman);


                                        }

                                        if (user.Roles.Contains(citizen))
                                        {
                                            await user.RemoveRoleAsync(citizen);
                                        }
                                        var embedbuilder = new EmbedBuilder().WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                                            .WithTitle(user.Nickname)
                                            .WithColor(woman.Color)
                                            .WithDescription("**" + user.Mention + " registered with" + man.Mention + "**")
                                            .AddField("Account created at: ", user.CreatedAt.ToString("dd/MM/yyyy"));



                                        var embed = embedbuilder.Build();
                                        await user.AddRoleAsync(man);
                                        await registerchnl.SendMessageAsync(MentionUtils.MentionUser(user.Id)+ " registered with " + MentionUtils.MentionRole(man.Id), false, embed);
                                    }
                                    else
                                    {
                                        SendMessageWithReply(MentionUtils.MentionUser(user.Id) + " has already own " +MentionUtils.MentionRole(man.Id)+ " role.", e);
                                    }

                                }
                            }
                            else if (e.Content.StartsWith(".k "))
                            {

                                for (int i = 0; i < e.MentionedUsers.Count; i++)
                                {
                                    

                                    var user = e.MentionedUsers.ToList()[i] as SocketGuildUser;
                                    if (!user.Roles.Contains(woman))
                                    {
                                        if (user.Roles.Contains(man))
                                        {

                                            await user.RemoveRoleAsync(man);


                                        }

                                        if (user.Roles.Contains(citizen))
                                        {
                                            await user.RemoveRoleAsync(citizen);
                                        }
                                        await user.AddRoleAsync(woman);

                                        var embedbuilder = new EmbedBuilder().WithTitle(user.Username + user.Discriminator);
                                  
                                 
                       
                                        await registerchnl.SendMessageAsync(MentionUtils.MentionUser(user.Id)+ " registered with " + MentionUtils.MentionRole(woman.Id),false ,embedbuilder.Build());

                                    }
                                    else
                                    {
                                        SendMessageWithReply(MentionUtils.MentionUser(user.Id) + " has already own " + MentionUtils.MentionRole(woman.Id) + " role.", e);
                                    }


                                }

                            }

                        }
                        else
                        {

                            SendMessageWithReply("There is no mentioned user in this message.", e);
                        }

                    }
                    else
                    {
                        SendMessageWithReply("You have no permission to do this.", e);
                    }


                }


                await Task.Delay(1);

            }
        }


        public static async void SendMessageWithReply(string msg, SocketMessage e, ComponentBuilder k = null)
        {
            if (k != null)
            {
                await e.Channel.SendMessageAsync(msg, false, null, null, null, new MessageReference(e.Id), component: k.Build());
            }
            else
            {
                await e.Channel.SendMessageAsync(msg, false, null, null, null, new MessageReference(e.Id));
            }

        }

        private async static Task Client_UserJoined(SocketGuildUser e)
        {
            var user = e.Guild.GetUser(e.Id);
            var role = e.Guild.Roles.ToList().Find(m => m.Id == 910683364602830929);
            IGuildUser a = e.Guild.GetUser(e.Id);
            Console.WriteLine(a.Username);
            Console.WriteLine(role.Name);
            try
            {
                await a.AddRoleAsync(role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }





        #endregion


        #region Discord Not Necessary Tasks
        static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        public static async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }
        public static async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);



            int argpos = 0;
            if (message.HasStringPrefix("_", ref argpos))
            {
                var result = await _commands.ExecuteAsync(context, argpos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }
        #endregion


        #region Discord Main Tasks
        static Task MessageRecieve(SocketMessage e)
        {  //
           // if(e.Content.StartsWith('_'))
           // {
           //     if(e.Content.Contains("play"))
           //     {
           //         var video = youtube.GetAllVideos(link);
           //         var Targetaudio = video.Where(r => r.AudioFormat != AudioFormat.Unknown).Select(r => r);
           //         var audio = Targetaudio.ToList()[0];
           //         Console.WriteLine("XD");
           //       
           //     }
           //
           // }
           //

            return Task.CompletedTask;
        }
        #endregion


    }
}
