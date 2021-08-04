using System;
using System.ComponentModel.DataAnnotations;

namespace SmileBot.Core.Database.Models
{
    public class DbEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
    }
}