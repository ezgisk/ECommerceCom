using System.ComponentModel.DataAnnotations;

namespace ECommerceCom.WepApi.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
