using System.Collections.Generic;

namespace _8bitstore_be.DTO.User
{
    public class UserDto
    {
        public string? Id { get; set; }
        public required string UserName { get; set; }
        public required List<AddressDto> Addresses { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
    }
}