using System.ComponentModel.DataAnnotations;


namespace NIA_CRM.Models
{
    public class Interaction
    {
        public int ID { get; set; }

        [Display(Name = "Interaction Date")]
        [Required(ErrorMessage = "Interaction date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime InteractionDate { get; set; }

        public string? InteractionNote { get; set; }

        public int ContactID { get; set; }
        public Contact? Contact { get; set; }
        public int MemberID { get; set; }
        public Member? Member { get; set; }
        public int OpportunityID { get; set; }
        public Opportunity? Opportunity { get; set; }
    }
}