using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Strategy
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Strategy Name")]
        [Required(ErrorMessage = "Strategy name is required.")]
        public string StrategyName { get; set; } = "";

        [Display(Name = "Assignee")]
        public string? StrategyAssignee { get; set; }

        [Display(Name = "Strategy Note")]
        public string? StrategyNote { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now; // Automatically set to current date

        [Required(ErrorMessage = "You must select the opportunity priority.")]
        public StrategyTerm StrategyTerm { get; set; }

        [Required(ErrorMessage = "You must select the opportunity priority.")]
        public StrategyStatus StrategyStatus { get; set; }
    }
}
