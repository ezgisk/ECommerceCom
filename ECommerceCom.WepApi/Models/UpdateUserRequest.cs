using ECommerceCom.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerceCom.WepApi.Models
{
    public class UpdateUserRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
