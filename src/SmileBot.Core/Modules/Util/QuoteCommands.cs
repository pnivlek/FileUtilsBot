using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SmileBot.Common.Attributes;
using SmileBot.Core.Services;
using SmileBot.Core.Services.Database.Models;

namespace SmileBot.Modules.Util
{
    public partial class Util : SmileHighLevelModule
    {
        [Group]
        public class QuoteCommands : SmileSubmodule
        {

            private readonly DbService _db;
            private readonly DiscordSocketClient _client;

            public QuoteCommands(DbService db, DiscordSocketClient client)
            {
                _db = db;
                _client = client;
            }

            [SmileCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            public async Task QuoteAdd(string keyword, [Leftover] string quote)
            {
                if (string.IsNullOrWhiteSpace(keyword) || string.IsNullOrWhiteSpace(quote))
                    return;

                using(var uow = _db.GetDbContext())
                {
                    uow.Quotes.Add(new Quote
                    {
                        GuildId = ctx.Guild.Id,
                            Keyword = keyword,
                            AuthorId = ctx.Message.Author.Id,
                            Author = ctx.Message.Author.Username,
                            Text = quote
                    });
                    uow.SaveChanges();
                }

                await ctx.Message.Channel.SendMessageAsync("Quote added.");
            }

            [SmileCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            public async Task QuoteDelete(int id)
            {
                using(var uow = _db.GetDbContext())
                {
                    Quote quote = uow.Quotes.GetById(id);
                    if (await SendQuote(ctx, quote))
                    {
                        {
                            uow.Quotes.Remove(id);
                            uow.SaveChanges();
                        }
                        await ReplyAsync("Quote removed.");
                    }
                }
            }

            [SmileCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            public async Task QuoteGet(int id)
            {
                using(var uow = _db.GetDbContext())
                {
                    Quote quote = uow.Quotes.GetById(id);
                    await SendQuote(ctx, quote);
                }
            }

            [SmileCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [Priority(0)]
            public async Task QuoteGetRandom()
            {
                using(var uow = _db.GetDbContext())
                {
                    Quote quote = await uow.Quotes.GetRandomFromGuild(ctx.Guild.Id);
                    await SendQuote(ctx, quote);
                }
            }

            [SmileCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [Priority(1)]
            public async Task QuoteGetRandom(string keyword)
            {
                using(var uow = _db.GetDbContext())
                {
                    Quote quote = await uow.Quotes.GetRandomFromGuildByKeyword(ctx.Guild.Id, keyword);
                    await SendQuote(ctx, quote);
                }
            }

            private async Task<bool> SendQuote(ICommandContext ctx, Quote quote)
            {
                if (quote?.GuildId != ctx.Guild.Id)
                {
                    await ctx.Channel.SendMessageAsync("Quote not found.");
                    return false;
                }
                else
                {
                    var embed = new EmbedBuilder()
                        .WithFooter(
                            new EmbedFooterBuilder()
                            .WithText("Added by " + quote.Author))
                        .WithFields(
                            new EmbedFieldBuilder()
                            .WithName("Quote")
                            .WithValue(quote.Text))
                        .WithTimestamp(
                            new DateTimeOffset((quote.DateCreated ?? DateTime.Now).ToLocalTime()));
                    await ctx.Channel.SendMessageAsync(embed: embed.Build());
                    return true;
                }
            }
        }
    }
}