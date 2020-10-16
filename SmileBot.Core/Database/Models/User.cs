namespace SmileBot.Core.Services.Database.Models
{
    public class User : DbEntity
    {
        public ulong UserId { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string AvatarId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User u ?
                u.UserId == UserId :
                false;
        }

        public override int GetHashCode() => UserId.GetHashCode();

        public override string ToString() =>
            Username + "#" + Discriminator;
    }
}