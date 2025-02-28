namespace _8bitstore_be.DTO.Product
{
    public class ProductRequest
    {
        public string? ProductId { get; set; }
        public int? SortByName { get; set; }
        public int? SortByPrice { get; set; }
        public int? SortByDate { get; set; }
    }
}
