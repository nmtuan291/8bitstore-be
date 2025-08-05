using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Crypto.Generators;

namespace _8bitstore_be.Models
{
    public class Product
    {
        [Key]
        [Required]
        public string ProductID { get; set; }

        [Required]
        public int StockNum { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        public ICollection<string> Platform { get; set; }
        public string Type { get; set; }
        public ICollection<string> Genre { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ProductName { get; set; }
        
        [Required]
        public double Weight { get; set; }
        
        public ICollection<string> ImgUrl { get; set; }

        [Required]
        public DateTime ImportDate { get; set; }
        
        [Required]
        public int WeeklySales { get; set; }
        
        public string? Color { get; set; }
        public string? Dimension { get; set; }
        public string? InternalStorage { get; set; }
    }
}
