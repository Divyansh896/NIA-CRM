using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Address
    {
        public int ID { get; set; }

        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "You cannot leave the address line 1 blank.")]
        [StringLength(255, ErrorMessage = "Address line 1 cannot be more than 255 characters long.")]
        public string AddressLineOne { get; set; } = "";

        [Display(Name = "Address Line 2")]
        [StringLength(255, ErrorMessage = "Address line 2 cannot be more than 255 characters long.")]
        public string? AddressLineTwo { get; set; }

        [Required(ErrorMessage = "You cannot leave the city blank.")]
        [StringLength(100, ErrorMessage = "City cannot be more than 100 characters long.")]
        public string City { get; set; } = "";

        [Display(Name = "State/Province")]
        [StringLength(100, ErrorMessage = "State/Province cannot be more than 100 characters long.")]
        public string? StateProvince { get; set; }

        [Display(Name = "Postal Code")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z]\d[A-Za-z]\d$", ErrorMessage = "Please enter a valid postal code in the format L1A1A9 (no spaces).")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Postal code must be exactly 6 characters long.")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "You cannot leave the country blank.")]
        [StringLength(100, ErrorMessage = "Country cannot be more than 100 characters long.")]
        public string? Country { get; set; }

        public int MemberID { get; set; }
        public Member Member { get; set; }
    }
}