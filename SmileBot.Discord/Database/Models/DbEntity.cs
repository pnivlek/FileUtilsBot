using System;
using System.ComponentModel.DataAnnotations;

namespace SmileBot.Discord.Database.Models
{
    public class DbEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
    }
}