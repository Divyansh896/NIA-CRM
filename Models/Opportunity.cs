using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Opportunity
    {
        public int ID { get; set; }

        [Display(Name = "Opportunity Name")]
        [Required(ErrorMessage = "You cannot leave the opportunity name blank.")]
        [StringLength(255, ErrorMessage = "Opportunity name cannot be more than 255 characters long.")]
        public string OpportunityName { get; set; } = "";

        [Display(Name = "Opportunity Description")]
        public string? OpportunityDescr { get; set; }

        [Required(ErrorMessage = "You must select the opportunity status.")]
        public OpportunityStatus OpportunityStatus { get; set; }

        public int OrganizationID { get; set; }
        public Organization Organization { get; set; }

        public Interaction Interaction { get; set; }
    }
}