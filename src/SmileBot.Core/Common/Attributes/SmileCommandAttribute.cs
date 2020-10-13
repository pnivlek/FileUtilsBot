using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Discord.Commands;
using SmileBot.Core.Services.Impl;

namespace SmileBot.Common.Attributes
{
    [AttributeUsage (AttributeTargets.Method)]
    public sealed class SmileCommandAttribute : CommandAttribute
    {
        public SmileCommandAttribute ([CallerMemberName] string memberName = "") : base (Localization.LoadCommand (memberName.ToLowerInvariant ()).Cmd.Split (' ') [0]) { }
    }
}