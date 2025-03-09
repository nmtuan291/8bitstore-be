using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.User
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string District {  get; set; }

        [Required]
        public string SubDistrict { get; set; }

        [Required]
        public string FullName { get; set; }


        [Required]
        public string PhoneNumber { get; set; }

    }
}