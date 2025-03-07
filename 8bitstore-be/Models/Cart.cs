﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace _8bitstore_be.Models
{
    public class Cart
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }   

}
