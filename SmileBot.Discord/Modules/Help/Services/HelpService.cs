using NLog;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System.Linq;
using SmileBot.Core.Services;
using SmileBot.Core.Extensions;

namespace SmileBot.Modules.Help.Services
{
    public class HelpService : ISmileService
    {
        private readonly Logger _log;
        private readonly CommandHandler _cmd;
        private readonly SmileBot _bot;

        public HelpService(CommandHandler cmd, SmileBot bot)
        {
            _cmd = cmd;
            _bot = bot;
            _log = LogManager.GetCurrentClassLogger();
        }

        public static EmbedBuilder GetBotHelp()
        {
            return new EmbedBuilder()
                .WithTitle("SmileBot Help")
                .WithThumbnailUrl("https://i.imgur.com/Qh1UEWJ.png")
                .AddField("Basic help commands",
                          $"{Format.Code(".modules")} List all modules."
                          + $"\n{Format.Code(".h CommandName")} Shows the help page for a specific command."
                          + $"\n{Format.Code(".commands ModuleName")} Shows the commands for a specific module.")
                .WithFooter("Maintained by Yackback#3679");

        }

        public EmbedBuilder GetCommandHelp(CommandInfo cmd, IGuild gld)
        {
            var str = string.Join(" / ", cmd.Aliases.Select(a => Format.Bold(Format.Code(a))));
            return new EmbedBuilder()
                .AddField(str, cmd.FormattedSummary("."), true)
                .AddField("Usage:", cmd.CodeRemarks("."), false)
                .WithFooter($"Module: {cmd.Module.GetTopLevelModule().Name}")
                .WithOkColor();
        }
    }
}
