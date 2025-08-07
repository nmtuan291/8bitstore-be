using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.User
{
    public class UserDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required ICollection<AddressDto> Addresses { get; set; } 
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
    }
}