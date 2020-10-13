using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SmileBot.Common.Attributes;
using SmileBot.Core._Extensions;
using SmileBot.Modules.Fun.Services;

namespace SmileBot.Modules.Fun
{
    public class Fun : SmileHighLevelModule<FunService>
    {
        [SmileCommand, Usage, Description, Aliases]
        public async Task EightBall ([Leftover] string question = null)
        {
            if (string.IsNullOrWhiteSpace (question))
                return;

            await ReplyAsync ("Unfinished.");
        }
    }
}