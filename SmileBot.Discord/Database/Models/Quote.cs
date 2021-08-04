using System.Collections.Generic;
namespace SmileBot.Core.Database.Models
{
    public class Quote : DbEntity
    {
        public ulong GuildId { get; set; }
        public string Keyword { get; set; }
        public ulong AuthorId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Quote q ?
                q.Text.Equals(Text) &&
                q.AuthorId == AuthorId &&
                q.GuildId == GuildId :
                false;
        }

        public override int GetHashCode() => (Text + AuthorId.ToString() + GuildId.ToString()).GetHashCode();

        public override string ToString() => Text;
    }
}