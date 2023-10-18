using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HayaseBot.commands
{
    public class GameAdminCommands : BaseCommandModule
    {
        string sqlClientACC = "Data Source=HAYASEYUUKA\\SQLEXPRESS;Initial Catalog=DiscordStorage;Integrated Security=True";

        private Random random = new Random();

        [Command("give")]
        public async Task TestingCommand(CommandContext ctx, DiscordMember target = null, int amt = 0)
        {
            Console.WriteLine(target);

            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            if (target == null)
            {
                var embedMessage = new DiscordEmbedBuilder()
                {
                    Title = "Please Specify a target User!",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await ctx.RespondAsync(embedMessage);
                return;
            }
            else if (amt == 0)
            {
                var embedMessage = new DiscordEmbedBuilder()
                {
                    Title = "Please enter a decent amount!",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await ctx.RespondAsync(embedMessage);
                return;
            }
            else if (target.Id == 1033001102687346718)
            {
                var embedMessage = new DiscordEmbedBuilder()
                {
                    Title = "I'm a millionaire already i don't need that",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await ctx.RespondAsync(embedMessage);
                return;
            }
            else
            {
                var embedMessage = new DiscordEmbedBuilder()
                {
                    Title = "Given " + target.DisplayName + " user " + amt + " ",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await ctx.RespondAsync(embedMessage);
                return;
            }
        }
    }
}
