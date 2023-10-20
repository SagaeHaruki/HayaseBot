using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HayaseBot.personalCMD
{
    public class PersonalCMD : BaseCommandModule
    {
        private Random random = new Random();

        [Command("adform")]
        [Description("ADMIN COMMAND ONLY")]
        public async Task AdminForm(CommandContext ctx)
        {
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var user = ctx.User.Id;
            if (user == 817577444805836831)
            {
                AdminForm adminForm = new AdminForm();
                adminForm.ShowDialog();
                return;
            }
            else 
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "You cannot use this command",
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
        }
    }
}
