using System;
using System.Runtime.CompilerServices;
using Discord.Commands;
using SmileBot.Core.Services.Impl;

namespace SmileBot.Common.Attributes
{
    [AttributeUsage (AttributeTargets.Method)]
    public class DescriptionAttribute : SummaryAttribute
    {
        public DescriptionAttribute ([CallerMemberName] string memberName = "") : base (Localization.LoadCommand (memberName.ToLowerInvariant ()).Desc) { }
    }
}