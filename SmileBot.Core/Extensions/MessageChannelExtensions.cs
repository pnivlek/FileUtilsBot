using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace SmileBot.Core.Extensions
{
    public static class MessageExtensions
    {
        public static Task<IUserMessage> EmbedAsync(this IMessageChannel ch, EmbedBuilder embed, string msg = "") => ch.SendMessageAsync(msg, embed: embed.Build(),
            options: new RequestOptions() { RetryMode = RetryMode.AlwaysRetry });
        public static Task<IUserMessage> EmbedAsync(this IUser user, EmbedBuilder embed, string msg = "") => user.SendMessageAsync(msg, embed: embed.Build(),
            options: new RequestOptions() { RetryMode = RetryMode.AlwaysRetry });
    }
}
