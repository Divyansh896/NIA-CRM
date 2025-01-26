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

        [Display(Name = "Join Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? JoinDate { get; set; }

        [Display(Name = "Standing Status")]
        [Required(ErrorMessage = "You must select the standing status.")]
        public StandingStatus StandingStatus { get; set; }

        public MemberLogo? MemberLogo { get; set; }
        public MemberThumbnail? MemberThumbnail { get; set; }

        public ICollection<MemberIndustry> MemberIndustries { get; set; } = new List<MemberIndustry>();
        public ICollection<MemberMembershipType> MemberMembershipTypes { get; set; } = new List<MemberMembershipType>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Cancellation> Cancellations { get; set; } = new List<Cancellation>();
        public ICollection<MemberNote> MemberNotes { get; set; } = new List<MemberNote>();
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
        public ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();

    }
}