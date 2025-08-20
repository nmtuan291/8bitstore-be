namespace _8bitstore_be.Exceptions;

public class ProductNotFoundException: Exception
{
    public ProductNotFoundException(string productId)
        : base($"Product with id {productId} was not found") { }
}