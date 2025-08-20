namespace _8bitstore_be.Exceptions;

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(string orderId) 
        : base($"Order with ID '{orderId}' was not found.") { }
}