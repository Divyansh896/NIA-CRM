using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Member
    {
        public int ID { get; set; }

        [Display(Name = "Member Name")]
        [Required(ErrorMessage = "You cannot leave the member name blank.")]
        [StringLength(255, ErrorMessage = "Member name cannot be more than 255 characters long.")]
        public string MemberName { get; set; } = "";

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? JoinDate { get; set; }

        [Required(ErrorMessage = "You must select the standing status.")]
        public StandingStatus StandingStatus { get; set; }

        public int OrganizationID { get; set; }
        public Organization? Organization { get; set; }

        public ICollection<MemberMembershipType> MemberMembershipTypes { get; set; } = new HashSet<MemberMembershipType>();
        public ICollection<Cancellation> Cancellations { get; set; } = new HashSet<Cancellation>();
        //public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();

        public Address? Address { get; set; }  // Instead of ICollection<Address> Addresses

    }
}