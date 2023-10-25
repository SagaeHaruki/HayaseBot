﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.VoiceNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace HayaseBot.commands
{
    public class BasicCommands : BaseCommandModule
    {
        private Random random = new Random();

        /*
         * Simple ping command
         */
        [Command("ping")]
        public async Task PingCmd(CommandContext ctx)
        {
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            // Logger
            var userName = ctx.User.Username;
            var ping = ctx.Client.Ping;

            var embed1 = new DiscordEmbedBuilder
            {
                Title = "Pong!!",
                Description = "Current API Latency is: " + ping +"ms",
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

        /*
         * Bot info Command
         */
        [Command("info")]
        public async Task ServerCmd(CommandContext ctx)
        {
            // Colors Embed
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            // Logger
            var userName = ctx.User.Username;
            var ServerOwner = ctx.Guild.Owner;

            var msg_toSend = new DiscordEmbedBuilder
            {
                Title = "Server Info",
                Description = "<Description Here>",
                Color = randomCol,
            };
            await ctx.Channel.SendMessageAsync(embed: msg_toSend);
        }

        /*
         * Server info commmand
         */
        [Command("serverinfo")]
        public async Task ServerInfCMD(CommandContext ctx)
        {
            try
            {

                // Colors Embed
                int red = random.Next(256);
                int green = random.Next(256);
                int blue = random.Next(256);
                DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

                var guildID = ctx.Guild.Id;
                var guildPhoto = ctx.Guild.IconUrl;
                var guildName = ctx.Guild.Name;
                var guildOwner = ctx.Guild.Owner.Username;
                var guildCurrentMem = ctx.Guild.MemberCount;
                var guildRoleCount = ctx.Guild.Roles.Count;
                var guildTextChannels = ctx.Guild.Channels.Count;
                var guildVoiceChannels = ctx.Guild.Channels;
                int guildVoiceChanCount = guildVoiceChannels.Values.Count(c => c.Type == DSharpPlus.ChannelType.Voice);
                int botCount = 0;
                foreach (var member in ctx.Guild.Members)
                {
                    if (member.Value.IsBot)
                    {
                        botCount++;
                    }
                }

                var msg_toSend = new DiscordEmbedBuilder
                {
                    Title = "Server Info",
                    Description = guildName,
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow,
                };
                msg_toSend.WithThumbnail(guildPhoto);
                msg_toSend.AddField("Owner", guildOwner, inline: true);
                msg_toSend.AddField("Role Count", guildRoleCount.ToString(), inline: true);
                msg_toSend.AddField("Member Count", guildCurrentMem.ToString(), inline: true);
                msg_toSend.AddField("Text Channels", guildTextChannels.ToString(), inline: true);
                msg_toSend.AddField("Voice Channels", guildVoiceChanCount.ToString(), inline: true);
                msg_toSend.AddField("Bots", botCount.ToString(), inline: true);
                await ctx.RespondAsync(msg_toSend);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
