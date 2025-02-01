using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Middle Name cannot be longer than 50 characters.")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [StringLength(10, ErrorMessage = "Title cannot be longer than 10 characters.")]
        public string? Title { get; set; }

        [StringLength(100, ErrorMessage = "Department cannot be longer than 100 characters.")]
        public string? Department { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters.")]
        public string? Phone { get; set; }

        [Url(ErrorMessage = "Invalid LinkedIn URL.")]
        [StringLength(200, ErrorMessage = "LinkedIn URL cannot be longer than 200 characters.")]
        public string? LinkedInUrl { get; set; }

        public bool IsVip { get; set; } = false;
        
        public ICollection<ContactNote> ContactNotes { get; set; } = new List<ContactNote>();
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();

        public int MemberId { get; set; }  // Foreign key to Member
        public Member Member { get; set; } // Navigation property to Member

        [Display(Name = "Contact Name")]
        public string Summary
        {
            get
            {
                return FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }
    }
}
