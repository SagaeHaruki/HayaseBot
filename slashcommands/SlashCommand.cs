using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HayaseBot.slashcommands
{
    public class SlashCommand : ApplicationCommandModule
    {
        [SlashCommand("test", "this is a test command")]
        public async Task TestSlash(InteractionContext ctx)
        {
            var embed1 = new DiscordEmbedBuilder
            {
                Title = "Test Command",
                Timestamp = DateTime.UtcNow,
            };
            await ctx.CreateResponseAsync(embed1);
        }
    }
}
