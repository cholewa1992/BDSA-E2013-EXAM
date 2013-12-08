using System.ComponentModel.DataAnnotations;
using Storage;

namespace FlatFileStorage.Entities
{
    public class UserAcc : IEntityDto
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
