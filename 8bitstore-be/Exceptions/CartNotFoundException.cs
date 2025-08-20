namespace _8bitstore_be.Exceptions;

public class CartNotFoundException : Exception
{
    public CartNotFoundException() 
        : base("Cart not found") { }
}