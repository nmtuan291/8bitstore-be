namespace _8bitstore_be.DTO
{
    public class ProductDto
    {
        public int StockNum { get; set; }
        public int Price { get; set; }
        public string Manufacturer { get; set; }
        public ICollection<string> Platform { get; set; }
        public ICollection<string> Type { get; set; }
        public ICollection<string> Genre { get; set; }
        public string Description { get; set; }
        public string ProductName { get; set; }
        public string ImgUrl { get; set; }
        public DateTime ImportDate { get; set; }
    }
}