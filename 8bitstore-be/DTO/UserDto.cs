using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string FullName { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}