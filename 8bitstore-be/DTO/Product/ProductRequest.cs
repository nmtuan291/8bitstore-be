using System.Collections.Generic;

namespace _8bitstore_be.DTO.Product
{
    public class ProductRequest
    {
        public required string ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<string>? Manufacturer { get; set; }
        public List<string>? Genres { get; set; }
        public List<string>? Type { get; set; }
        public List<string>? Platforms { get; set; }
        public int? SortByName { get; set; }
        public int? SortByPrice { get; set; }
        public int? SortByDate { get; set; }
        public int? SortByWeeklySales { get; set; }
        public int? Top { get; set; }
        public int Page { get; set; } = 1;
    }
}
