using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace _8bitstore_be.Models
{
    public class User: IdentityUser
    {
        public string FullName { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? District { get; set; }

        public string? SubDistrict { get; set; }
    }
}
