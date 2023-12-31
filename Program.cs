﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands.EventArgs;
using HayaseBot.commands;
using HayaseBot.slashcommands;
using HayaseBot.config;
using HayaseBot.PrivateCommands;
using Microsoft.Extensions.Logging;
using System;
using System.Media;
using System.Threading.Tasks;

namespace HayaseBot
{
    internal class Program
    {
        public static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }

        // Bot Startup
        static async Task Main(string[] args)
        {
            // This will read the token and prefix
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

            // Interactivity default timeout    
            var interactivity = Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            // Event Handlers
            Client.Ready += Client_Ready;
            Client.ComponentInteractionCreated += HelpComamndButton;
            Client.MessageCreated += Event_Handler;
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
            var SLConfig = Client.UseSlashCommands();

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
            Commands.RegisterCommands<pvCommand>();

            // Slash Commands
            SLConfig.RegisterCommands<SlashCommand>();
            SLConfig.RegisterCommands<InformSLCommand>();
            SLConfig.RegisterCommands<GameSlashCommand>();
            SLConfig.SlashCommandErrored += SlashCommandsHandler;

            foreach (var cmdss in SLConfig.RegisteredCommands.Count.ToString())
            {
                Console.WriteLine(cmdss);
            }

            // Connect the Client
            Client.ConnectAsync().GetAwaiter().GetResult();
            await Task.Delay(-1);
        }

        // Stating the Bot Client
        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            Console.WriteLine("Engine has Started");
            return Task.CompletedTask;
        }

        private static async Task HelpComamndButton(DiscordClient sender, ComponentInteractionCreateEventArgs cice)
        {
            Random random = new Random();
            // Colors Embed
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            // Button For Page 1
            var Page1_Btn = new DiscordButtonComponent(ButtonStyle.Success, "Page2", ">>");
            var Page1_Btn2 = new DiscordButtonComponent(ButtonStyle.Success, "Page1", "<<");

            // Button For Page 2
            var Page2_Btn = new DiscordButtonComponent(ButtonStyle.Success, "Page3", ">>");
            var Page2_Btn2 = new DiscordButtonComponent(ButtonStyle.Success, "Page1", "<<");

            // Button For Page 3
            var Page3_Btn = new DiscordButtonComponent(ButtonStyle.Success, "Page4", ">>");
            var Page3_Btn2 = new DiscordButtonComponent(ButtonStyle.Success, "Page2", "<<");

            // Button For Page 4
            var Page4_Btn = new DiscordButtonComponent(ButtonStyle.Success, "Page5", ">>");
            var Page4_Btn2 = new DiscordButtonComponent(ButtonStyle.Success, "Page3", "<<");

            /*
             * Page 1 Help Command
             */

            if (cice.Interaction.Data.CustomId == "Page1")
            {
                var embeder = new DiscordEmbedBuilder
                {
                    Title = "Haruki Bot Commands!",
                    Description = "***Bot Source!***\n[Developer](https://github.com/SagaeHaruki) Developer Github \n[Github](https://github.com/SagaeHaruki/HayaseBot) Source Code\n***Bot Help Commands***",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow,
                }
                .AddField("**Help Command**", "*Page 1*", inline: false)
                .AddField(">kick <@username>", "Kick a member from the server", inline: false)
                .AddField(">ban <@username>", "Ban a member from the server", inline: false)
                .AddField(">clear <amount>", "Clear a channel (Max 100) Cannot clear messages more than 14 days", inline: false)
                .AddField(">timeout", "(Coming Soon)", inline: false);


                await cice.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(null).AddEmbed(embeder).AddComponents(Page1_Btn2, Page1_Btn));
                return;
            }

            /*
             * Page 2 Help Command
             */

            if (cice.Interaction.Data.CustomId == "Page2")
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
                .AddField("**Help Command**", "*Page 2*", inline: false)
                .AddField(">beg ", "Beg for Money", inline: false)
                .AddField(">bal <@username>", "Check your balance or others balance", inline: false)
                .AddField(">slotmachine", "Use the slotmachine (-1000) coins", inline: false)
                .AddField(">roll <amount>", "Roll the dice", inline: false);

                await cice.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(null).AddEmbed(embeder).AddComponents(Page2_Btn2, Page2_Btn));
                return;
            }

            /*
             * Page 3 Help Command
             */

            if (cice.Interaction.Data.CustomId == "Page3")
            {
                var embeder = new DiscordEmbedBuilder
                {
                    Title = "Haruki Bot Commands!",
                    Description = "***Bot Source!***\n[Developer](https://github.com/SagaeHaruki) Developer Github \n[Github](https://github.com/SagaeHaruki/HayaseBot) Source Code\n***Bot Help Commands***",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow,
                }
                .AddField("**Help Command**", "*Page 3*", inline: false)
                .AddField(">search <place>", "Search a place to get some money", inline: false)
                .AddField(">searchlist", "List of the places to search", inline: false)
                .AddField(">withdraw <amount>", "Withdraw money from the bank", inline: false)
                .AddField(">deposit <amount>", "Deposit money to the bank", inline: false);

                await cice.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(null).AddEmbed(embeder).AddComponents(Page3_Btn2, Page3_Btn));
                return;
            }

            /*
             * Page 4 Gelp Command
             */

            if (cice.Interaction.Data.CustomId == "Page4")
            {
                var embeder = new DiscordEmbedBuilder
                {
                    Title = "Haruki Bot Commands!",
                    Description = "***Bot Source!***\n[Developer](https://github.com/SagaeHaruki) Developer Github \n[Github](https://github.com/SagaeHaruki/HayaseBot) Source Code\n***Bot Help Commands***",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow,
                }
                .AddField("**Help Command**", "*Page 4*", inline: false)
                .AddField(">ping", "API Latency", inline: false)
                .AddField(">info", "Show bot information", inline: false)
                .AddField(">serverinfo", "Show Server information", inline: false)
                .AddField(">profile <@username>", "Show someone's username", inline: false);

                await cice.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(null).AddEmbed(embeder).AddComponents(Page4_Btn2));
                return;
            }
        }

        private static async Task SlashCommandsHandler(SlashCommandsExtension sender, SlashCommandErrorEventArgs errs)
        {
            Random random = new Random();


            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var userName = errs.Context.User.Username;

            /*
             * Slash Commands Cooldown
             */
            
            if (errs.Exception is SlashExecutionChecksFailedException exception)
            {
                string timeLeft = string.Empty;
                foreach (var check in exception.FailedChecks)
                {
                    var cmdCooldown = (SlashCooldownAttribute)check;
                    timeLeft = cmdCooldown.GetRemainingCooldown(errs.Context).TotalSeconds.ToString();
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
                await errs.Context.CreateResponseAsync(embed1);
                Console.WriteLine("[TIME]: " + DateTime.Now + "  |  USERNAME:  " + userName + "  |  Used a Slash Command but was unable to due to command cooldowns");
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

            /*
             * Commands Cooldown (Slash Command Not included)
             */

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
                    Timestamp = DateTime.UtcNow,
                };
                await err.Context.RespondAsync(embed1);
                Console.WriteLine("[TIME]: " + DateTime.Now + "  |  USERNAME:  " + userName + "  |  Used a Command but was unable to due to command cooldowns");
                return;
            }
        }

        // Voice Chat Logger

        private static async Task VoiceChannel_Handler(DiscordClient sender, VoiceStateUpdateEventArgs channel_phase)
        {
            var userName = channel_phase.User.Username;
            var GuildName = channel_phase.Guild.Name;

            // OOf i wonder what this do
            var channelid = await Client.GetChannelAsync(1173092273534287953);

            // Logs everything
            if (channel_phase.Channel == null)
            {
                // If the user left a Voice channel 
                // Another note that the channel the user left cannot be fetched it will turn into a null
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "Voice Channel Logger",
                    Description = "**User:** " + userName + "\n**Guild Name:** " + GuildName + "\n**Status:** Disconnected",
                    Color = DiscordColor.Red,
                    Timestamp = DateTime.UtcNow
                };
                await channelid.SendMessageAsync(embed1);
                Console.WriteLine("[TIME]: " + DateTime.Now + "  |  USERNAME:  " + userName + "  |  GUILD NAME: " + GuildName + "  |  Left the Channel " + channel_phase.Channel);
                return;
            }
            else if(channel_phase.Channel != null)
            {
                // If the user join a Voice channel anywhere
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "Voice Channel Logger",
                    Description = "**User:** " + userName + "\n**Guild Name:** " + GuildName + "\n**Channel Name:** " + channel_phase.Channel.Name + "\n**Status:** Connected",
                    Color = DiscordColor.Green,
                    Timestamp = DateTime.UtcNow
                };
                await channelid.SendMessageAsync(embed1);
                Console.WriteLine("[TIME]: " + DateTime.Now + "  |  USERNAME:  " + userName + "  |  GUILD NAME: " + GuildName + "  |  Joined the Channel >> " + channel_phase.Channel.Name);
                return;
            }
        }

        // Message Logger
        private static async Task Event_Handler(DiscordClient sender, MessageCreateEventArgs event_phase)
        {
            try
            {
                var userID = event_phase.Author.Id;
                var GuildName = event_phase.Guild.Name;
                var userName = event_phase.Author.Username;
                var channelName = event_phase.Channel.Name;
                var message_sent = event_phase.Message.Content;

                // OOf i wonder what this do
                var channelid = await Client.GetChannelAsync(1173092489029222490);

                if (userID == 1033001102687346718)
                {
                    // Nothing happens if the user ID is the bot (or this will loop)
                }
                else
                {
                    foreach (var mentionedUser in event_phase.MentionedUsers)
                    {
                        ulong mentionedUserId = mentionedUser.Id;

                        if (mentionedUserId == 817577444805836831)
                        {
                            SoundPlayer player = new SoundPlayer("Hoshino1.wav");
                            player.Play();
                        }
                    }

                    // Embeds the message send by a user
                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "Message Logger",
                        Description = "**User:** " + userName + "\n**Guild Name:** " + GuildName + "\n**Channel Name:** " + channelName + "\n**Message Content:**\n" + message_sent,
                        Color = DiscordColor.Green,
                        Timestamp = DateTime.UtcNow
                    };
                    await channelid.SendMessageAsync(embed1);

                    Console.WriteLine("[TIME]: " + DateTime.Now + "  |  USERNAME:  " + userName + "  |  GUILD NAME: " + GuildName + "  |  ChannelName: " + channelName + "  |  Message Sent:  " + message_sent);
                    return;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
