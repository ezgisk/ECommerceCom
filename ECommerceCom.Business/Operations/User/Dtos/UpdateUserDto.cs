using ECommerceCom.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.User.Dtos
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Id gereklidir.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "FirstName gereklidir.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName gereklidir.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Role gereklidir.")]
        public UserRole Role { get; set; }
    }
}
