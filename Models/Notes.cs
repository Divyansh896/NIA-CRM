using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Notes
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Note content is required.")]
        public string? NoteContent { get; set; }

        [Required(ErrorMessage = "Contact ID is required.")]
        public int ContactID { get; set; }

        // Navigation property to the Contact model
        public Contact? Contact { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now; // Automatically set to current date

    }
}
