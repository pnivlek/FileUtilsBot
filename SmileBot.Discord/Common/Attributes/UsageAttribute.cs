using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Discord.Commands;
using Newtonsoft.Json;
using SmileBot.Core.Services.Impl;

namespace SmileBot.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class UsageAttribute : RemarksAttribute
    {
        public UsageAttribute([CallerMemberName] string memberName = "") : base(UsageAttribute.GetUsage(memberName)) { }

        private static string GetUsage(string memberName)
        {
            var usage = Localization.LoadCommand(memberName.ToLowerInvariant()).Usage;
            return JsonConvert.SerializeObject(usage);
        }
    }
}