namespace _8bitstore_be.DTO.Product;

public class PaginatedResult
{
    public IEnumerable<ProductDto> Products { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}