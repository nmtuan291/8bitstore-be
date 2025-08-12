using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class Product
    {
        [Key]
        public string ProductID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string Manufacturer { get; set; }

        public ICollection<string> Platform { get; set; }

        public string Type { get; set; }

        public ICollection<string> Genre { get; set; }

        public string Description { get; set; }

        public string ProductName { get; set; }

        public DateTime ImportDate { get; set; }

        public int StockNum { get; set; }

        public int WeeklySales { get; set; }

        public ICollection<string> ImgUrl { get; set; }

        public double Weight { get; set; }

        public string? Color { get; set; }

        public string? Dimension { get; set; }

        public string? InternalStorage { get; set; }
    }
}
