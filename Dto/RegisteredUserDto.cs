using System;
using System.ComponentModel.DataAnnotations;

namespace SocialSite.API.Dto
{
    public class RegisteredUserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string RePassword { get; set; }
    }
}