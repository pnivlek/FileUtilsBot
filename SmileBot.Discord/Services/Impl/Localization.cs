using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SmileBot.Common;

namespace SmileBot.Discord.Services.Impl
{
    public class Localization : ILocalization
    {
        private static readonly Dictionary<string, CommandData> _commandData =
            JsonConvert.DeserializeObject<Dictionary<string, CommandData>>(File.ReadAllText("./Data/Commands/commands.json"));

        public Localization()
        {
            // TODO whenever I do localization (probably never).
        }

        public static CommandData LoadCommand(string commandKey)
        {
            _commandData.TryGetValue(commandKey, out CommandData toReturn);

            if (toReturn == null)
                return new CommandData
                {
                    Cmd = commandKey,
                    Desc = commandKey,
                    Usage = new[] { commandKey },
                };

            return toReturn;
        }
    }
}