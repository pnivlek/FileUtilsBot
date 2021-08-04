using Discord.Addons.Interactive;
using Discord.Commands;
using NLog;
using SmileBot.Discord.Services;

namespace SmileBot.Modules
{
    public abstract class SmileHighLevelModule : ModuleBase
    {
        protected Logger _log { get; }

        public string ModuleTypeName { get; }
        public string LowerModuleTypeName { get; }

        public CommandHandler CmdHandler { get; set; }
        public string Prefix => ".";
        protected ICommandContext ctx => Context;

        protected SmileHighLevelModule(bool isTopLevelModule = true)
        {
            ModuleTypeName = isTopLevelModule ? this.GetType().Name : this.GetType().DeclaringType.Name;
            LowerModuleTypeName = ModuleTypeName.ToLowerInvariant();
            _log = LogManager.GetCurrentClassLogger();
        }
    }

    public abstract class SmileHighLevelModule<TService> : SmileHighLevelModule where TService : ISmileService
    {
        public TService _service { get; set; }

        protected SmileHighLevelModule(bool isTopLevel = true) : base(isTopLevel) { }
    }

    public abstract class SmileSubmodule : SmileHighLevelModule
    {
        protected SmileSubmodule() : base(false) { }
    }

    public abstract class SmileSubmodule<TService> : SmileHighLevelModule<TService> where TService : ISmileService
    {
        protected SmileSubmodule() : base(false) { }
    }
}