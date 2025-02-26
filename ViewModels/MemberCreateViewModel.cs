using NIA_CRM.Models;
using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.ViewModels
{
    public class MemberCreateViewModel
    {
        // Member related properties
        public Member? Member { get; set; }
        public Address? Address { get; set; }
        public Contact? Contact { get; set; }

        // Industry NAICS Code for the member
        public List<IndustryNAICSCode>? IndustryNAICSCode { get; set; }  // Change to List<IndustryNAICSCode>
        // List of available NAICS codes to be shown in the drop-down
        public List<NAICSCode>? NAICSCodes { get; set; }

        // MembershipType related properties
        public List<MemberMembershipType>? MemberMembershipTypes { get; set; }  // Change to List<MemberMembershipType>
        // List of available Membership Types to populate the drop-down
        public List<MembershipType>? MembershipTypes { get; set; }

        // List of selected membership types (for multiple memberships)
        public List<int> SelectedMembershipTypes { get; set; } = new List<int>();

        public List<int> SelectedNAICSCodeIds { get; set; } = new List<int>();  // Add this property
        public List<NAICSCode>? AvailableNAICSCodes { get; set; }  // List of available NAICS Codes for dropdown
        public int SelectedNAICSCodeId { get; set; }  // Single selected NAICS Code for the form

    }
}
