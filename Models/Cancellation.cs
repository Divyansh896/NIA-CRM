using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Cancellation
    {
        [Display(Name = "Time Since Cancellation")]
        public string? TimeSinceCancelled
        {
            get
            {
                if (CancellationDate == null) { return null; }
                DateTime today = DateTime.Today;
                int? years = today.Year - CancellationDate.Year
                    - ((today.Month < CancellationDate.Month ||
                        (today.Month == CancellationDate.Month && today.Day < CancellationDate.Day) ? 1 : 0));
                return years?.ToString() + " year(s) ago";
            }
        }
        public int ID { get; set; }

        [Display(Name = "Cancellation Date")]
        [Required(ErrorMessage = "Cancellation date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CancellationDate { get; set; }

        public bool Canceled {  get; set; }
        public string? CancellationNote { get; set; }

        public int MemberID { get; set; }
        public Member Member { get; set; }
    }
}