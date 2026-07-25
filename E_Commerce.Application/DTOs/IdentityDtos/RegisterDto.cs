using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.IdentityDtos
{
    public class RegisterDto
    {
        // email , password, display name, phone number
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = default!;
        [Required(ErrorMessage = "DisplayName is Required")]
        public string DisplayName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string? PhoneNumber { get; set; }
    }
}
