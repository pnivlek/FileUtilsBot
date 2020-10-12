using System;
using System.Collections.Generic;
using System.Text;

namespace SmileBot.Core.Services.Database.Models
{
    public class Guild : DbEntity
    {
        public ulong GuildId { get; set; }

        public string Prefix { get; set; } = null;

        public bool DeleteMessageOnCommand { get; set; }
        public int DeleteMessageOnCommandTimer { get; set; }
    }
}