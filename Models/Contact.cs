using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Contact
    {
        public int ID { get; set; }

        [Display(Name = "Contact Name")]
        [Required(ErrorMessage = "You cannot leave the contact name blank.")]
        [StringLength(255, ErrorMessage = "Contact name cannot be more than 255 characters long.")]
        public string ContactName { get; set; } = "";

        [StringLength(255, ErrorMessage = "Title cannot be more than 255 characters long.")]
        public string? Title { get; set; }

        [StringLength(255, ErrorMessage = "Department cannot be more than 255 characters long.")]
        public string? Department { get; set; }

        [StringLength(255, ErrorMessage = "Email cannot be more than 255 characters long.")]
        [DataType(DataType.EmailAddress)]
        public string? EMail { get; set; }

        [RegularExpression("^\\d{10,20}$", ErrorMessage = "Please enter a valid phone number with 10 to 20 digits (digits only, no spaces).")] //left it from 10 to 20, in casse they will have some international phones, but no less than 10, so they won't leave an invalid one
        [DataType(DataType.PhoneNumber)]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "The phone number must be between 10 and 20 digits long.")]
        public string? Phone { get; set; }

        [Display(Name = "Linked In")]
        [StringLength(255, ErrorMessage = "Linked In cannot be more than 255 characters long.")]
        public string? LinkedinUrl { get; set; }

        [Display(Name = "VIP")]
        public bool IsVIP { get; set; }

        public ICollection<ContactOrganization> ContactOrganizations { get; set; } = new HashSet<ContactOrganization>();
    }
}