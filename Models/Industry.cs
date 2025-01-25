using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Industry
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the industry name blank.")]
        [Display(Name = "Industry Name")]
        [StringLength(255, ErrorMessage = "Industry name cannot be more than 255 characters long.")]
        public string IndustryName { get; set; } = "";
        [Required(ErrorMessage = "You cannot leave the industry size blank.")]
        [Display(Name = "Industry Size")]
        public int IndustrySize { get; set; }

        public string? WebsiteUrl { get; set; }

        public ICollection<MemberIndustry> MemberIndustries { get; set; } = new List<MemberIndustry>();
        public ICollection<IndustryNAICSCode> IndustryNAICSCodes { get; set; } = new List<IndustryNAICSCode>();
        public ICollection<ContactIndustry> ContactIndustries { get; set; } = new List<ContactIndustry>();
        public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
    }
}
