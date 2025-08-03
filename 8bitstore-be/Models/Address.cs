using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Nest;

namespace _8bitstore_be.Models;

public class Address
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string District { get; set; }
    
    [Required]
    public string Ward { get; set; }
    
    [Required]
    public string AddressDetail { get; set; }
    
    [Required]
    public string Recipent { get; set; }
    
    [Required]
    public bool IsDefault { get; set; }

    [Required]
    public string RecipentPhone { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
}