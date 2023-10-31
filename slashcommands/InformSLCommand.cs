using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HayaseBot.slashcommands
{
    public class InformSLCommand : ApplicationCommandModule
    {
        private Random random = new Random();

        [SlashCommand("help", "Shows the help command")]
        public async Task SlashCommand(InteractionContext ctx)
        {
            // Colors Embed
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var Page1_Btn = new DiscordButtonComponent(ButtonStyle.Success, "Page2", ">>");
            var Page1_Btn2 = new DiscordButtonComponent(ButtonStyle.Success, "Page1", "<<");


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


            await ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent(null).AddEmbed(embeder).AddComponents(Page1_Btn2, Page1_Btn));
            return;
        }
    }
}
