using System.ComponentModel.DataAnnotations;

namespace FileManagement.Authentication.Models
{
    public class LoginToken
    {
        [Key]
        public Guid Id { get; set; }
        public string token { get; set; }
        public string UserId { get; set; }
    }
}
