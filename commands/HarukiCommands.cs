using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HayaseBot.commands
{
    public class HarukiCommands : BaseCommandModule
    {
        // Command to use
        [Command("ping")]

        // Run The Command
        public async Task PingCmd(CommandContext ctx)
        {
            var userID = ctx.User.Id;
            var userName = ctx.User.Username;
            
            await ctx.Channel.SendMessageAsync("pong");
            Console.Write("UserID: | " + userID + " | UserName | " + userName + " | Used ping Command | Time: " + DateTime.Now);
        }
    }
}
