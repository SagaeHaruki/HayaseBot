using DSharpPlus;
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
                Timestamp = DateTime.UtcNow,
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

            var userName = ctx.User.Username;
            var ServerOwner = ctx.Guild.Owner;

            var msg_toSend = new DiscordEmbedBuilder
            {
                Title = "Bot Information",
                Description = "Hello i'm hayase bot created by: hayase_haruki",
                Color = randomCol,
                Timestamp = DateTime.UtcNow,
            };
            msg_toSend.AddField("IDE", "Visual Code 2022", inline: true);
            msg_toSend.AddField("Language", "C#", inline: true);
            msg_toSend.AddField("Database", "MSSql", inline: true);
            msg_toSend.AddField("Source Code", "Click [here](https://github.com/SagaeHaruki/HayaseBot)!", inline: true);
            msg_toSend.AddField("Developer", "hayase_haurki", inline: true);
            msg_toSend.AddField("Github", "My [Github](https://github.com/SagaeHaruki)", inline: true);
            msg_toSend.WithFooter("This bot is from the winter wonderland!");
            await ctx.RespondAsync(msg_toSend);
        }

        /*
         * Server info commmand
         */
        [Command("serverinfo")]
        public async Task ServerInfCMD(CommandContext ctx)
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
            msg_toSend.AddField("Owner:", guildOwner, inline: true);
            msg_toSend.AddField("Role Count:", guildRoleCount.ToString(), inline: true);
            msg_toSend.AddField("Member Count:", guildCurrentMem.ToString(), inline: true);
            msg_toSend.AddField("Text Channels:", guildTextChannels.ToString(), inline: true);
            msg_toSend.AddField("Voice Channels:", guildVoiceChanCount.ToString(), inline: true);
            msg_toSend.AddField("Bot Count:", botCount.ToString(), inline: true);
            await ctx.RespondAsync(msg_toSend);
        }

        /*
         * User Profile Command
         */

        [Command("profile")]
        public async Task ProfileCommand(CommandContext ctx, [RemainingText] DiscordMember target = null)
        {
            // Colors Embed
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);


            // User Info
            var userPhoto = ctx.User.AvatarUrl;
            var userName = ctx.User.Username;
            var userJoin = ctx.Member.JoinedAt;
            UserStatus userPresence = ctx.Member.Presence?.Status ?? UserStatus.Offline;
            var userId = ctx.User.Id;
            var userBot = ctx.User.IsBot;

            // If user didn't specify a target
            if (target == null)
            {
                var msg_toSend = new DiscordEmbedBuilder
                {
                    Title = "Profile Checker",
                    Description = "Username: " + userName,
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow,
                };
                msg_toSend.WithThumbnail(userPhoto);
                msg_toSend.AddField("Server Join Date:", userJoin.ToString("dd/mm/yyyy"), inline: true);
                msg_toSend.AddField("Current activity:", userPresence.ToString(), inline: true);
                msg_toSend.AddField("User ID:", userId.ToString(), inline: true);
                msg_toSend.AddField("Is a Bot:", userBot.ToString(), inline: true);
                await ctx.RespondAsync(msg_toSend);
                return;
            }
            // If the user didn't specify a target
            else if (target != null)
            {
                // Target Info
                var targetPhoto = target.AvatarUrl;
                var targetName = target.Username;
                var targetJoin = target.JoinedAt;
                UserStatus targetPresence = target.Presence?.Status ?? UserStatus.Offline;
                var targetId = target.Id;
                var targetBot = target.IsBot;

                var msg_toSend = new DiscordEmbedBuilder
                {
                    Title = "Profile Checker",
                    Description = "Username: " + targetName,
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow,
                };
                msg_toSend.WithThumbnail(targetPhoto);
                msg_toSend.AddField("Server Join Date:", targetJoin.ToString("dd/mm/yyyy"), inline: true);
                msg_toSend.AddField("Current activity:", targetPresence.ToString(), inline: true);
                msg_toSend.AddField("User ID:", targetId.ToString(), inline: true);
                msg_toSend.AddField("Is a Bot:", targetBot.ToString(), inline: true);
                await ctx.RespondAsync(msg_toSend);
                return;
            }
        }
    }
}
