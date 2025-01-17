using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace NIA_CRM.Models
{
    public class Organization
    {
        public int ID { get; set; }

        [Display(Name = "Organization Name")]
        [Required(ErrorMessage = "You cannot leave the organization name blank.")]
        [StringLength(255, ErrorMessage = "Organization name cannot be more than 255 characters long.")]
        public string OrganizationName { get; set; } = "";

        [Display(Name = "Organization Size")]
        public int? OrganizationSize { get; set; }

        [Display(Name = "Organization Website")]
        [StringLength(255, ErrorMessage = "Organization Website cannot be more than 255 characters long.")]
        public string? OrganizationWeb { get; set; }

        public int IndustryID { get; set; }
        public Industry Industry { get; set; }

        public ICollection<ContactOrganization> ContactOrganizations { get; set; } = new HashSet<ContactOrganization>();
        public ICollection<Opportunity> Opportunities { get; set; } = new HashSet<Opportunity>();
        public ICollection<OrganizationCode> OrganizationCodes { get; set; } = new HashSet<OrganizationCode>();
        public ICollection<Member> Members { get; set; } = new HashSet<Member>();
    }
}

