using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public enum OpportunityStatus
    {
        Open,
        [Display(Name = "In Progress")]
        InProgress,
        Closed
    }
}