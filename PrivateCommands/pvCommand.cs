using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace HayaseBot.PrivateCommands
{
    public class pvCommand : BaseCommandModule
    {
        [Command("notify")]
        public async Task notifCommand(CommandContext ctx, [RemainingText] string message)
        {
            var command_User = ctx.User.Username;

            SpeechSynthesizer speecher = new SpeechSynthesizer();
            speecher.Speak("You've recieved a message from:.. " + command_User + ".." + message);
        }
    }
}
