using NIA_CRM.Models;
using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.ViewModels
{
    public class MemberCreateViewModel
    {
        // Member-related properties
        public Member? Member { get; set; }
        public Address? Address { get; set; }

        // List of Contacts (since a Contact can now have multiple Members)
        public List<Contact>? Contacts { get; set; } = new List<Contact>();

        // Industry NAICS Code for the member
        public List<IndustryNAICSCode>? IndustryNAICSCodes { get; set; }  // Renamed to plural for clarity
        // List of available NAICS codes to be shown in the drop-down
        public List<NAICSCode>? NAICSCodes { get; set; }

        // MembershipType related properties
        public List<MemberMembershipType>? MemberMembershipTypes { get; set; }
        // List of available Membership Types to populate the drop-down
        public List<MembershipType>? MembershipTypes { get; set; }

        // List of selected membership types (for multiple memberships)
        public List<int> SelectedMembershipTypes { get; set; } = new List<int>();

        public List<int> SelectedNAICSCodeIds { get; set; } = new List<int>();
        public List<NAICSCode>? AvailableNAICSCodes { get; set; }
        public int SelectedNAICSCodeId { get; set; }

        public List<MemberContact>? MemberContacts { get; set; } = new List<MemberContact>();

    }
}
