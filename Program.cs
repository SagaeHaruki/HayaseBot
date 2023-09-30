using DSharpPlus; // Use Discord Sharp
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using HayaseBot.commands;
using HayaseBot.config;
using System.Threading.Tasks;
using System;
using System.Threading.Channels;

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


            // Bot Token ID Finder
            var config_Haruki = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = caster_haruki.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            // Ready Client to Start
            /* !!! IMPORTANT READY MUST BE FIRST OR THE CLIENT WILL NOT RUN !!!*/
            Client = new DiscordClient(config_Haruki);
            Client.Ready += Client_Ready;

            // Event Handler
            Client.MessageCreated += Event_Handler;

            // FIX LATER
            // Client.VoiceStateUpdated += VoiceChannel_Handler;

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
            Commands.RegisterCommands<HarukiCommands>();

            // Connect the Client
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        // Voice Chat Logger (FIX NEXT)
        /*
        private static async Task VoiceChannel_Handler(DiscordClient sender, VoiceStateUpdateEventArgs channel_phase)
        {

                var userID = channel_phase.User.Id;
                var userName = channel_phase.User.Username;
                var channelID = channel_phase.Channel.Name;
                if (channel_phase.Before == null && channel_phase.Channel.Name == channelID)
                {
                    Console.WriteLine("USER ID >> " + userID + " | USERNAME >> " + userName + " | ChannelID Connected >> " + channelID +" | TIME >> " + DateTime.Now);
                }
        }
        */

        // Message Logger
        private static async Task Event_Handler(DiscordClient sender, MessageCreateEventArgs event_phase)
        {
            var userID = event_phase.Author.Id;
            var userName = event_phase.Author.Username;
            var channelID = event_phase.Channel.Name;
            if (userID == 1033001102687346718)
            {
                // Nothing happens if the user ID is the bot (or this will loop)
            }
            else 
            {
                Console.WriteLine("USER ID >> " + userID + " | USERNAME >> " + userName + " | ChannelID " + channelID +" | TIME >> " + DateTime.Now);
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
