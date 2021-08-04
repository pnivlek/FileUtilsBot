using System.Collections.Generic;

namespace SmileBot.Core.Database.Models
{
    public class ReactionTrackSettings : DbEntity
    {
        public int GuildConfigId { get; set; }
        public GuildConfig GuildConfig { get; set; }

        public bool TrackEnabled { get; set; } = false;
        public List<ulong> IgnoredChannels { get; set; } = new List<ulong>();
    }
}
