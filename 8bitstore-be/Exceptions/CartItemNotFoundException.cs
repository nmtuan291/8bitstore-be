namespace _8bitstore_be.Exceptions;

public class CartItemNotFoundException : Exception
{
    public CartItemNotFoundException() : base("Cart item not found") { }
}