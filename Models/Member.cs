using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.Models
{
    public class Member
    {
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


        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the member name blank.")]
        [Display(Name = "Member Name")]
        [StringLength(255, ErrorMessage = "Member name cannot be more than 255 characters long.")]
        public string MemberName { get; set; } = "";

        [Required(ErrorMessage = "You cannot leave the size blank.")]
        [Display(Name = "Size")]
        public int MemberSize { get; set; }

        public string? WebsiteUrl { get; set; }

        [Display(Name = "Join Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? JoinDate { get; set; }

        public MemberLogo? MemberLogo { get; set; }
        public MemberThumbnail? MemberThumbnail { get; set; }

        public ICollection<MemberMembershipType> MemberMembershipTypes { get; set; } = new List<MemberMembershipType>();
        public ICollection<IndustryNAICSCode> IndustryNAICSCodes { get; set; } = new List<IndustryNAICSCode>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Cancellation> Cancellations { get; set; } = new List<Cancellation>();
        public ICollection<MemberNote> MemberNotes { get; set; } = new List<MemberNote>();
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
        public ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
        public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();

    }
}