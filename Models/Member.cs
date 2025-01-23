using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Member
    {
        [Display(Name = "Time Since Joined")]
        public string? TimeSinceJoined
        {
            get
            {
                if (JoinDate == null) { return null; }
                DateTime today = DateTime.Today;
                int? years = today.Year - JoinDate?.Year
                    - ((today.Month < JoinDate?.Month ||
                        (today.Month == JoinDate?.Month && today.Day < JoinDate?.Day) ? 1 : 0));
                return years?.ToString() + " year(s) ago";
            }
        }
        [Display(Name = "Member Name")]
        public string Summary
        {
            get
            {
                return MemberFirstName
                    + (string.IsNullOrEmpty(MemberMiddleName) ? " " :
                        (" " + (char?)MemberMiddleName[0] + ". ").ToUpper())
                    + MemberLastName;
            }
        }
        public int ID { get; set; }

        [Display(Name = "Member First Name")]
        [Required(ErrorMessage = "You cannot leave the member first name blank.")]
        [StringLength(100, ErrorMessage = "Member first name cannot be more than 255 characters long.")]
        public string MemberFirstName { get; set; } = "";

        [Display(Name = "Member Middle Name")]
        [StringLength(100, ErrorMessage = "Member middle name cannot be more than 255 characters long.")]
        public string? MemberMiddleName { get; set; }

        [Display(Name = "Member Last Name")]
        [Required(ErrorMessage = "You cannot leave the member last name blank.")]
        [StringLength(100, ErrorMessage = "Member last name cannot be more than 255 characters long.")]
        public string MemberLastName { get; set; } = "";

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