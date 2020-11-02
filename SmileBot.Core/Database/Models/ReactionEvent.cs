namespace SmileBot.Core.Database.Models
{
    public class ReactionEvent : DbEntity
    {
        public ulong EmojiId { get; set; }
        public string EmojiName { get; set; }
        public ulong MessageId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong GuildId { get; set; }
        public ulong ReactorUserId { get; set; }
    }
}
