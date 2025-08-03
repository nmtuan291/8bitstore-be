using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace _8bitstore_be.Models
{
    public class User: IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        
        [Required]
        public ICollection<Address> Addresses { get; set; }
    }
}
