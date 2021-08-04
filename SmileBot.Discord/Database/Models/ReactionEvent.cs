namespace SmileBot.Core.Database.Models
{
    public class ReactionEvent : DbEntity
    {
        public ulong EmoteId { get; set; }
        public string EmoteName { get; set; }
        public ulong MessageId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong GuildId { get; set; }
        public bool FromOutsideGuild { get; set; }
        public ulong ReactorUserId { get; set; }
    }
}
