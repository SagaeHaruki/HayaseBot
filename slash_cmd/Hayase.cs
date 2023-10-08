using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HayaseBot.slash_cmd
{
    public class Hayase : ApplicationCommandModule
    {
        [SlashCommand("test", "Testing onlyhere")]
        public async Task TestCommand(InteractionContext ctx)
        {
            Console.WriteLine("CMD!!");
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Fetching Reply"));
            var msg_toSend = new DiscordEmbedBuilder
            {
                Title = "Server Info",
                Description = "<Description Here>",
            };
            await ctx.Channel.SendMessageAsync(embed: msg_toSend);
            return;
        }
    }
}
