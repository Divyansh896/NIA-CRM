using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class OrganizationCode
    {
        public int ID { get; set; }


        [Display(Name = "Organization Code")]
        [Required(ErrorMessage = "You cannot leave the organization code blank.")]
        [StringLength(255, ErrorMessage = "Organization code cannot be more than 255 characters long.")]
        public string? CodeOrganization { get; set; }

        public int OrganizationID { get; set; }
        public Organization? Organization { get; set; }
    }
}