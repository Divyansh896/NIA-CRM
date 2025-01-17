using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Cancellation
    {
        public int ID { get; set; }

        [Display(Name = "Cancellation Date")]
        [Required(ErrorMessage = "Cancellation date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CancellationDate { get; set; }

        public string? CancellationNote { get; set; }

        public int MemberID { get; set; }
        public Member Member { get; set; }
    }
}