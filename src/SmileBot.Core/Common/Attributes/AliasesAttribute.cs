using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Discord.Commands;
using SmileBot.Core.Services.Impl;

namespace SmileBot.Common.Attributes
{
    [AttributeUsage (AttributeTargets.Method)]
    public sealed class AliasesAttribute : AliasAttribute
    {
        public AliasesAttribute ([CallerMemberName] string memberName = "") : base (Localization.LoadCommand (memberName.ToLowerInvariant ()).Cmd.Split (' ').Skip (1).ToArray ())
        {
            var _ = Localization.LoadCommand (memberName.ToLowerInvariant ()).Cmd.Split (' ').Skip (1).ToArray ();
        }
    }
}