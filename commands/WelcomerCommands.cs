using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HayaseBot.commands
{
    public class WelcomerCommands : BaseCommandModule
    {
        private Random random = new Random();

        [Command("greet")]
        public async Task GreetingsCommand(CommandContext ctx, DiscordMember target = null, [RemainingText] string message = "hello")
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var cmdUser = ctx.User.Username;
            var guildName = ctx.Guild.Name;
            var cmdUserId = ctx.User.Id;

            if (target == null)
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "You need to specify a member to greet!",
                    Description = "Usage: ***>greet <@username> <message>***",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            else if (cmdUserId == target.Id)
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "Sorry but you cannot greet yourself!",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.Channel.SendMessageAsync(embed1);
                return;
            }
            else if (message == "hello")
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = cmdUser + " is greeting " + target.Username,
                    Description = "Welcome to " + guildName + " please enjoy your stay!",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.Channel.SendMessageAsync("Hey! " + target.Mention);
                await ctx.Channel.SendMessageAsync(embed1);
                return;
            }
            else
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = cmdUser + " is greeting " + target.Username,
                    Description = message,
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.Channel.SendMessageAsync("Hey! " + target.Mention);
                await ctx.Channel.SendMessageAsync(embed1);
                return;
            }
        }
    }
}
