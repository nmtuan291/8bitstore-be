namespace _8bitstore_be.Exceptions;

public class UserNotFoundException: Exception
{
    public UserNotFoundException(string userId) 
        : base($"User with ID '{userId}' was not found.") { }
}