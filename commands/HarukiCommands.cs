using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HayaseBot.commands
{
    public class HarukiCommands : BaseCommandModule
    {
        // Ping Command
        [Command("ping")]

        // Run The Command
        public async Task PingCmd(CommandContext ctx)
        {
            // Logger
            var userID = ctx.User.Id;
            var userName = ctx.User.Username;
            
            await ctx.Channel.SendMessageAsync("pong");

            // Console
            Console.WriteLine("USER ID >> " + userID + " | USERNAME >> " + userName + " |   PING COMMAND   | TIME >> " + DateTime.Now);
        }

        // Server Info Command
        [Command("info")]
        public async Task ServerCmd(CommandContext ctx)
        {
            // Logger
            var userID = ctx.User.Id;
            var userName = ctx.User.Username;

            var msg_toSend = new DiscordEmbedBuilder
            {
                Title = "Server Info",
                Description = "<Description Here>",
                Color = DiscordColor.Red
            };
            await ctx.Channel.SendMessageAsync(embed: msg_toSend);

            // Console
            Console.WriteLine("USER ID >> " + userID + " | USERNAME >> " + userName + " |   SERVER INFO COMMAND   | TIME >> " + DateTime.Now);
        }
    }
}
