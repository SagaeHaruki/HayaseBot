using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using DSharpPlus;
using DSharpPlus.Entities;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HayaseBot.PrivateCommands
{
    public class pvCommand : BaseCommandModule
    {
        [Command("notify")]
        public async Task notifCommand(CommandContext ctx, [RemainingText] string message)
        {
            var Client = ctx.Client;
            // OOf i wonder what this do
            var channelid = await Client.GetChannelAsync(1173578575002222622);
            var command_User = ctx.User.Username;
            var channel_name = ctx.Channel.Name;
            var Guild_name = ctx.Guild.Name;

            SpeechSynthesizer speecher = new SpeechSynthesizer();
            speecher.Speak("You've recieved a message from:. " + command_User + "." + message);

            // Embeds the message send by a user
            var embed1 = new DiscordEmbedBuilder
            {
                Title = "Notify Command Logger",
                Description = "**User:** " + command_User + "\n**Guild Name:** " + Guild_name + "\n**Channel Name:** " + channel_name,
                Color = DiscordColor.Green,
                Timestamp = DateTime.UtcNow
            };
            await channelid.SendMessageAsync(embed1);
        }
    }
}
