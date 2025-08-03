namespace _8bitstore_be.DTO.User;

public class AddressDto
{
    public Guid? Id { get; set; }
    public required string City { get; set; }
    public required string District { get; set; }
    public required string Ward { get; set; }
    public required string AddressDetail { get; set; }
    public required string Recipent { get; set; }
    public required bool IsDefault { get; set; }
    public required string RecipentPhone { get; set; }
}