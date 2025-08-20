namespace _8bitstore_be.Exceptions;

public class ProductQuantityException : Exception
{
    public ProductQuantityException(string productId)
        : base($"Product with id {productId} does not have enough quantity") { }
}