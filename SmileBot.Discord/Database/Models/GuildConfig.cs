namespace SmileBot.Core.Database.Models
{
    public class GuildConfig : DbEntity
    {
        public ulong GuildId { get; set; }

        public string Prefix { get; set; } = null;

        public bool DeleteMessageOnCommand { get; set; }
        public int DeleteMessageOnCommandTimer { get; set; }

        public ReactionTrackSettings ReactionTrackSettings { get; set; } = new ReactionTrackSettings();
    }
}
