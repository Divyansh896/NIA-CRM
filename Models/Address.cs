using NIA_CRM.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Address
{
    [Key]
    public int Id { get; set; } // Primary Key

    [ForeignKey(nameof(Member))]
    [Required(ErrorMessage = "Member is required.")]
    [Display(Name = "Member")]
    public int MemberId { get; set; }

    [Required(ErrorMessage = "Address Line 1 is required.")]
    [MaxLength(255, ErrorMessage = "Address Line 1 cannot exceed 255 characters.")]
    [Display(Name = "Address Line 1")]
    public string AddressLine1 { get; set; }

    [MaxLength(255, ErrorMessage = "Address Line 2 cannot exceed 255 characters.")]
    [Display(Name = "Address Line 2")]
    public string? AddressLine2 { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
    [Display(Name = "City")]
    public string City { get; set; }

    [MaxLength(100, ErrorMessage = "State/Province cannot exceed 100 characters.")]
    [Display(Name = "State/Province")]
    public string? StateProvince { get; set; }

    [MaxLength(20, ErrorMessage = "Postal Code cannot exceed 20 characters.")]
    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    [MaxLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
    [Display(Name = "Country")]
    public string Country { get; set; }

    // Navigation Property
    [Display(Name = "Member")]
    public Member Member { get; set; }
}
