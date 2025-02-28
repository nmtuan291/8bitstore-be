using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.User
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
    }
}