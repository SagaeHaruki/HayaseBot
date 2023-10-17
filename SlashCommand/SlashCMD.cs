﻿using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HayaseBot.SlashCommand
{
    internal class SlashCMD : ApplicationCommandModule
    {

        [SlashCommand("give", "Give a user money Command")]
        public async Task TestingCommand(InteractionContext ctx)
        {
            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Testing"
            };

            await ctx.CreateResponseAsync(embedMessage);
        }
    }
}
