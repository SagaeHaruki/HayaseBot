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

        // Ping Command
        [Command("ping")]

        // Run The Command
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
            // Console
            Console.WriteLine("> USERNAME >> " + userName + " |   PING COMMAND   | TIME >> " + DateTime.Now);
            return;
        }

        // Server Info Command
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

            // Console
            Console.WriteLine("> USERNAME >> " + userName + " |   SERVER INFO COMMAND   | TIME >> " + DateTime.Now);
        }
    }
}
