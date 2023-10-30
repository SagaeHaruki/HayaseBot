﻿using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using HayaseBot.commands;
using HayaseBot.config;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DSharpPlus.SlashCommands;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.DependencyInjection;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using HayaseBot.slashcommands;
using System.Linq;

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

            var services = new ServiceCollection()
            .AddSingleton<InteractivityExtension>()
            .BuildServiceProvider();

            // Ready Client to Start
            /* !!! IMPORTANT READY MUST BE FIRST OR THE CLIENT WILL NOT RUN !!!*/
            Client = new DiscordClient(config_Haruki);
            Client.Ready += Client_Ready;
            Client.ComponentInteractionCreated += ButtonPressResponse;


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
                EnableDefaultHelp = false
            };
            // Ready Client Command
            Commands = Client.UseCommandsNext(config_Prefix);
            var SlashCommandsConfig = Client.UseSlashCommands();

            // Command Error
            Commands.CommandErrored += CommandHandler;

            // Normal Commands Only 
            Commands.RegisterCommands<BasicCommands>();
            Commands.RegisterCommands<ModerationCommand>();
            Commands.RegisterCommands<GameCommands>();
            Commands.RegisterCommands<SearchCmd>();
            Commands.RegisterCommands<BankCommands>();
            Commands.RegisterCommands<GameAdminCommands>();
            Commands.RegisterCommands<WelcomerCommands>();

            SlashCommandsConfig.RegisterCommands<SlashCommand>();

            // Interactivity

            var interactivity = Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(30)
            });

            // Connect the Client
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task ButtonPressResponse(DiscordClient sender, ComponentInteractionCreateEventArgs cice)
        {
            Random random = new Random();
            // Colors Embed
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var button1 = new DiscordButtonComponent(ButtonStyle.Primary, "custom_id1", "Next");
            var button2 = new DiscordButtonComponent(ButtonStyle.Primary, "custom_id2", "Previous");

            // If user Selected the Next

            if (cice.Interaction.Data.CustomId == "custom_id1")
            {
                // Embed Message
                // Page 2 of the Commands
                var embeder = new DiscordEmbedBuilder
                {
                    Title = "Haruki Bot Commands!",
                    Description = "***Bot Source!***\n[Developer](https://github.com/SagaeHaruki) Developer Github \n[Github](https://github.com/SagaeHaruki/HayaseBot) Source Code\n***Bot Help Commands***",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow,
                }
                .AddField("**Help Command**", "Game Commands", inline: false)
                .AddField(">beg ", "Beg for Money", inline: false)
                .AddField(">bal <@username> **OR**\n>bal", "Check your balance or others balance", inline: false)
                .AddField(">slotmachine", "Use the slotmachine (-1000) coins", inline: false)
                .AddField(">roll <amount>", "Roll the dice", inline: false);

                var message = new DiscordMessageBuilder()
                    .WithEmbed(embeder)
                    .AddComponents(button1);

                await cice.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(null).AddEmbed(embeder).AddComponents(button2));
                return;
            }
            else if (cice.Interaction.Data.CustomId == "custom_id2")
            {
                // First Page (The Moderation Command)
                var embeder = new DiscordEmbedBuilder
                {
                    Title = "Haruki Bot Commands!",
                    Description = "***Bot Source!***\n[Developer](https://github.com/SagaeHaruki) Developer Github \n[Github](https://github.com/SagaeHaruki/HayaseBot) Source Code\n***Bot Help Commands***",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow,
                }
                    .AddField("**Help Command**", "Moderation", inline: false)
                    .AddField(">kick <@username>", "Kick a member from the server", inline: false)
                    .AddField(">ban <@username>", "Ban a member from the server", inline: false)
                    .AddField(">bank <@username>", "Ban a member from the server", inline: false);

                var message = new DiscordMessageBuilder()
                    .WithEmbed(embeder)
                    .AddComponents(button1);

                await cice.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(null).AddEmbed(embeder).AddComponents(button1));
                return;
            }
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
                Console.WriteLine("[TIME]: " + DateTime.Now + "  |  USERNAME:  " + userName + "  |  Used a Command but was unable to due to command cooldowns");
                return;
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
                return;
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
