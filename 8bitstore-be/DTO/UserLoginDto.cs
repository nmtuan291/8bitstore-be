﻿using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO
{
    public class UserLoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
