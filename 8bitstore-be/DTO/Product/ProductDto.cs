using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.Product
{
    public class ProductDto
    {
        public string ProductId { get; set; }
        public int StockNum { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
        public ICollection<string> Platform { get; set; }
        public string Type { get; set; }
        public ICollection<string> Genre { get; set; }
        public string Description { get; set; }
        public string ProductName { get; set; }
        public ICollection<string> ImgUrl { get; set; }
        public DateTime ImportDate { get; set; }
        public double Weight { get; set; }
        public string? Color { get; set; }
        public string? Dimension { get; set; }
        public string? InternalStorage { get; set; }
    }
}