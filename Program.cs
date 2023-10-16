using DSharpPlus; // Use Discord Sharp
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using HayaseBot.commands;
using HayaseBot.config;
using System.Threading.Tasks;
using System;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using DSharpPlus.SlashCommands;
using HayaseBot.slash_cmd;
using System.Runtime.InteropServices.ComTypes;
using HayaseBot.slash_cmd;
using HayaseBot.commands;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Attributes;

namespace HayaseBot
{
    internal class Program
    {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }

        // Bot Startup
        static async Task Main(string[] args)
        {
            var caster_haruki = new caster_reader();
            await caster_haruki.readCaster();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Warning);
            });

            // Bot Token ID Finder
            var config_Haruki = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = caster_haruki.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LoggerFactory = loggerFactory
            };

            // Ready Client to Start
            /* !!! IMPORTANT READY MUST BE FIRST OR THE CLIENT WILL NOT RUN !!!*/
            Client = new DiscordClient(config_Haruki);
            Client.Ready += Client_Ready;


            // Event Handler
            Client.MessageCreated += Event_Handler;

            // FIX LATER
            Client.VoiceStateUpdated += VoiceChannel_Handler;

            // Bot Command Prefix Finder
            var config_Prefix = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { caster_haruki.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = true
            };

            // Ready Client Command
            Commands = Client.UseCommandsNext(config_Prefix);

            // Command Error
            Commands.CommandErrored += CommandHandler;
            var SlashCommandsBld = Client.UseSlashCommands();

            // Normal Commands Only 
            Commands.RegisterCommands<BasicCommands>();
            Commands.RegisterCommands<ModerationCommand>();
            Commands.RegisterCommands<GameCommands>();
            Commands.RegisterCommands<SearchCmd>();
            Commands.RegisterCommands<BankCommands>();

            // Slash Commands Only
            SlashCommandsBld.RegisterCommands<Hayase>();

            // Connect the Client
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        // Command Cooldown Cat
        private static async Task CommandHandler(CommandsNextExtension sender, CommandErrorEventArgs err)
        {
            Random random = new Random();

            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var userName = err.Context.User.Username;

            if (err.Exception is ChecksFailedException exception)
            {
                string timeLeft = string.Empty;
                foreach (var check in exception.FailedChecks)
                {
                    var cmdCooldown = (CooldownAttribute)check;
                    timeLeft = cmdCooldown.GetRemainingCooldown(err.Context).TotalSeconds.ToString();
                }
                int timeSec = (int)Math.Floor(Convert.ToDecimal(timeLeft));

                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "Command on cooldown!! " + timeSec + "s",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await err.Context.RespondAsync(embed1);
                Console.WriteLine("[TIME]: " + DateTime.Now + "  |  USERNAME >>  " + userName + "  |  Used a Command but was unable to due to command cooldowns");
            }
        }
        // Voice Chat Logger (FIX NEXT)

        private static async Task VoiceChannel_Handler(DiscordClient sender, VoiceStateUpdateEventArgs channel_phase)
        {
            var userName = channel_phase.User.Username;
            var GuildName = channel_phase.Guild.Name;

            // Logs everything
            if (channel_phase.Channel == null)
            {
                Console.WriteLine("[TIME]: " + DateTime.Now + "  |  USERNAME:  " + userName + "  |  GUILD NAME: " + GuildName + "  |  Left the Channel " + channel_phase.Channel);
                return;
            }
            else if(channel_phase.Channel != null)
            {
                Console.WriteLine("[TIME]: " + DateTime.Now + "  |  USERNAME:  " + userName + "  |  GUILD NAME: " + GuildName + "  |  Joined the Channel >> " + channel_phase.Channel.Name);
                return;
            }
        }

        // Message Logger
        private static async Task Event_Handler(DiscordClient sender, MessageCreateEventArgs event_phase)
        {
            var userID = event_phase.Author.Id;
            var GuildName = event_phase.Guild.Name;
            var userName = event_phase.Author.Username;
            var channelName = event_phase.Channel.Name;
            var message_sent = event_phase.Message.Content;
            if (userID == 1033001102687346718)
            {
                // Nothing happens if the user ID is the bot (or this will loop)
            }
            else 
            { 
                Console.WriteLine("[TIME]: " + DateTime.Now + "  |  USERNAME:  " + userName + "  |  GUILD NAME: " + GuildName + "  |  ChannelName: " + channelName + "  |  Message Sent:  " + message_sent);
            }

        }

        // Stating the Bot Client
        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            Console.WriteLine("Engine has Started");
            return Task.CompletedTask;
        }
    }
}
