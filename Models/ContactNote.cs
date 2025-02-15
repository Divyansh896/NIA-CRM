using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class ContactNote
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Contact ID is required.")]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Note content is required.")]
        [StringLength(500, ErrorMessage = "Note cannot be longer than 500 characters.")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Creation date is required.")]
        public DateTime? CreatedAt { get; set; }

        public Contact? Contact { get; set; }
    }
}
