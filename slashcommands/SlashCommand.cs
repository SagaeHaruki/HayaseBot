using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Threading.Tasks;

namespace HayaseBot.slashcommands
{
    public class SlashCommand : ApplicationCommandModule
    {
        [SlashCommand("test", "This is our first Slash Command")]
        public async Task TestSlashCommand(InteractionContext ctx, [Choice("Pre-Defined Text", "afhajfjafjdghldghlhg")]
                                                                   [Option("string", "Type in anything you want")] string text)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                                                            .WithContent("Starting Slash Command...."));

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = text,
            };

            await ctx.Channel.SendMessageAsync(embed: embedMessage);
        }

    }
}
