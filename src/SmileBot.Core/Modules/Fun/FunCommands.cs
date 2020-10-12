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
        public async Task EightBall([Leftover] string question = null)
        {
            if (string.IsNullOrWhiteSpace(question))
                return;

            await ctx.Channel.EmbedAsync(
                new EmbedBuilder().WithColor(Color.Green)
                .WithDescription(ctx.User.ToString()).AddField(efb => efb.WithName("hi").WithValue("test").WithIsInline(false)));
        }
    }
}