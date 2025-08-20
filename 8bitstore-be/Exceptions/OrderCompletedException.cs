namespace _8bitstore_be.Exceptions;

public class OrderCompletedException : Exception
{
    public OrderCompletedException(string orderId) 
        : base($"Cannot change order {orderId} status") { }
}