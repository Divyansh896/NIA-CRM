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

        public ICollection<Organization> Organizations { get; set; } = new HashSet<Organization>();
    }
}