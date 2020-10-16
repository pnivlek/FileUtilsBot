using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using NLog;
using SmileBot.Core.Services;

namespace SmileBot.Modules.Fun.Services
{
    public class FunService : ISmileService
    {
        private readonly Logger _log;
        private readonly CommandHandler _cmd;
        private readonly SmileBot _bot;

        public FunService(CommandHandler cmd, SmileBot bot)
        {
            _cmd = cmd;
            _bot = bot;
            _log = LogManager.GetCurrentClassLogger();
        }
    }
}