﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace HayaseBot.commands
{
    public class ModerationCommand : BaseCommandModule
    {
        // This will be used for Embed Color Randomizer
        private Random random = new Random();

        /*
         * Kick Command does what it says
         */

        [Command("kick")]
        [Description("Kick a user from the Server")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task KickCommand(CommandContext ctx, DiscordMember target, [RemainingText] string reason = null)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            if (target == null)
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "Please specity a member to ban!",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            else
            {
                // User to ban and command user 
                var targetPerms = target.PermissionsIn(ctx.Channel);
                var isABot = target.Id;
                var cmdUser = ctx.User.Id;
                var targetName = target.DisplayName;
                if (targetPerms.HasPermission(Permissions.Administrator))
                {

                    // If user doesn't have admin permissions

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "You cannot kick this user with Administrator Permission!",
                        Color = randomCol,
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            Text = DateTime.Now.ToString("hh:mm tt"),
                            IconUrl = null
                        }
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
                else if (cmdUser == target.Id)
                {

                    // if user tried to kick themselves

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "You cannot kick Yourself!",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow,
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
                else if (isABot == 1033001102687346718)
                {

                    // This part if the user tried to kick the bot

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "You cannot kick ME!",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
                else
                {

                    // If user successfully kicked the user

                    await target.RemoveAsync(reason); // Kicks the user

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "The User " + target.DisplayName + " is Kicked by " + ctx.User.Username + " from the Server! \nReason: " + reason,
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
            }
        }

        /*
         * Ban command does what it says
         */

        [Command("ban")]
        [Description("Ban a user from the Server")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task BanCommand(CommandContext ctx, DiscordMember target = null, [RemainingText] string reason = null)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            if (target == null)
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "Please specity a member to ban!",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            else
            {
                // User to ban and command user 
                var targetPerms = target.PermissionsIn(ctx.Channel);
                var isABot = target.Id;
                var cmdUser = ctx.User.Id;
                if (targetPerms.HasPermission(Permissions.Administrator))
                {
                    // If user have administration

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "You cannot ban this user with Administrator Permission!",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
                else if (cmdUser == target.Id)
                {

                    // if user tried to ban themselves

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "You cannot ban Yourself!",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
                else if (isABot == 1033001102687346718)
                {

                    // This part if the user tried to ban the bot

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "You cannot ban ME!",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
                else
                {

                    // If user successfully banned the user

                    await target.BanAsync(reason: reason); // Ban' the user

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "The User " + target.DisplayName + " is banned by " + ctx.User.Username + " from the Server! \nReason: " + reason,
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
            }
        }
        /*
         * Timeout Command
         */
        [Command("timeout")]
        [Description("Timeout a member from the Server")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task TimeoutCommand(CommandContext ctx, DiscordMember target = null, [RemainingText] string reason = null)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);



            if (target == null)
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "Please specity a member to ban!",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            else
            {
                var targetPerms = target.PermissionsIn(ctx.Channel);
                var isABot = target.Id;
                var cmdUser = ctx.User.Id;

                if (targetPerms.HasPermission(Permissions.Administrator))
                {
                    // If user have administration

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "You cannot ban this user with Administrator Permission!",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
                else if (cmdUser == target.Id)
                {
                    // If user tried to Timout themselve

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "You cannot ban Yourself!",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
                else if (isABot == 1033001102687346718)
                {
                    // If the user tried to timeout the bot
                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "You cannot ban ME!",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
                else
                {                  
                    var button1 = new DiscordButtonComponent(ButtonStyle.Success, "60s", "60sec");
                    var interactivity = ctx.Client.GetInteractivity();

                    // Send an embedded Message
                    var embeder = new DiscordEmbedBuilder
                    {
                        Title = "Timeout Command!",
                        Description = "User: " + target.Username + "\nReason: " + reason,
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow,
                    };

                    var message = new DiscordMessageBuilder()
                        .AddEmbed(embeder)
                        .AddComponents(button1);


                    // Convert message builder to normal message
                    var messenger = await ctx.RespondAsync(message);

                    // Check if button is sued within 15 seconds or the command fill failed
                    var resulter = await interactivity.WaitForButtonAsync(messenger, ctx.User, TimeSpan.FromSeconds(15));

                    if(resulter.Result.User.Id != cmdUser)
                    {
                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = "Failed to use Timeout Command",
                            Description = "Other User Tried to press the button",
                            Color = randomCol,
                            Timestamp = DateTime.UtcNow
                        };
                        var message2 = new DiscordMessageBuilder()
                            .WithEmbed(embed1);
                        await messenger.ModifyAsync(message2);
                    }

                    if (resulter.Result.Id == "60s")
                    {
                        await target.TimeoutAsync(DateTimeOffset.Now.AddSeconds(60), reason);

                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = "User has been timed out!",
                            Description = "User: " + target.Username + "\nTime: 60 Seconds" + "\nReason: " + reason,
                            Color = randomCol,
                            Timestamp = DateTime.UtcNow
                        };
                        var message2 = new DiscordMessageBuilder()
                            .WithEmbed(embed1);
                        await messenger.ModifyAsync(message2);
                    }

                    if (resulter.TimedOut)
                    {
                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = "Failed to timeout user!",
                            Description = "You ran out of time",
                            Color = randomCol,
                            Timestamp = DateTime.UtcNow
                        };
                        var message2 = new DiscordMessageBuilder()
                            .WithEmbed(embed1);
                        await messenger.ModifyAsync(message2);
                        return;
                    }
                }
            }
        }

        [Command("timoutlist")]
        [Description("List of the time for timeout command")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task TOList(CommandContext ctx)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);
            var embed1 = new DiscordEmbedBuilder
            {
                Title = "List of Timout (Time)",
                Description = "Usage: *>timeout <@username> <time> <reason>*",
                Color = randomCol,
                Timestamp = DateTime.UtcNow
            };
            await ctx.RespondAsync(embed1);
            return;

        }

        /*
         * Clear Command (Max 14 days of message)
         */
        [Command("clear")]
        [Description("Clear the messages in a channel")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task ClearChat(CommandContext ctx,[RemainingText] int amt = 0)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            // Command User
            var userCmd = ctx.User.Username;
            var botID = ctx.Guild.CurrentMember.PermissionsIn(ctx.Channel);

            // Message Timespan
            var messageCount = await ctx.Channel.GetMessagesAsync(amt + 1);

            try
            {
                if (!botID.HasPermission(Permissions.ManageMessages))
                {
                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "I don't have any permission to clear the chat",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }

                // Check if amt is between 1 or 100
                if (amt == 0)
                {
                    var embed3 = new DiscordEmbedBuilder
                    {
                        Title = "Enter between 1 - 100 amount to be cleared!",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };

                    await ctx.RespondAsync(embed3);
                }
                else if (amt < 1 || amt > 100)
                {
                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "I can only Clear 1 - 100 messages and no longer than 14 days",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
                else
                {
                    await ctx.Channel.DeleteMessagesAsync(messageCount);

                    await Task.Delay(3000);
                    var embed3 = new DiscordEmbedBuilder
                    {
                        Title = "Chat cleared by " + userCmd,
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };

                    await ctx.RespondAsync(embed3);
                    return;
                }
                return;
            }
            catch
            {
                var embed3 = new DiscordEmbedBuilder
                {
                    Title = "Message is more than 14days old",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.RespondAsync(embed3);
                return;
            }
        }
    }
}
