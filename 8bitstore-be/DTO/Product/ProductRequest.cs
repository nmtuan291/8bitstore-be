namespace _8bitstore_be.DTO.Product
{
    public class ProductRequest
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? SortByName { get; set; }
        public int? SortByPrice { get; set; }
        public int? SortByDate { get; set; }
        public int? SortByWeeklySales { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public int? Top { get; set; }
        public ICollection<string>? Manufacturer { get; set; }
        public ICollection<string>? Genres { get; set; }
        public ICollection<string>? Type { get; set; }
        public ICollection<string>? Platforms { get; set; }
        public int Page  { get; set; }
    }
}
