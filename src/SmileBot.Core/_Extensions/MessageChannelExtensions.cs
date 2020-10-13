using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace SmileBot.Core._Extensions
{
    public static class MessageChannelExtensions
    {
        public static Task<IUserMessage> EmbedAsync (this IMessageChannel ch, EmbedBuilder embed, string msg = "") => ch.SendMessageAsync (msg, embed : embed.Build (),
            options : new RequestOptions () { RetryMode = RetryMode.AlwaysRetry });
    }
}