using NIA_CRM.Models;
using System.ComponentModel.DataAnnotations;

namespace NIA_CRM.ViewModels
{
    public class MemberCreateViewModel
    {
        [Key]
        public int ID { get; set; }

        public Member Member { get; set; } = new Member();
        public MemberNote MemberNote { get; set; } = new MemberNote();

        public Address Address { get; set; } = new Address();
        public Contact Contact { get; set; } = new Contact();
        public ContactNote ContactNote { get; set; } = new ContactNote();
        public NAICSCode NAICSCode { get; set; } = new NAICSCode();
        public IndustryNAICSCode IndustryNAICSCode { get; set; } = new IndustryNAICSCode();

        //public MembershipType MembershipType { get; set; } = new MembershipType();
        public MemberMembershipType MemberMembershipType { get; set; } = new MemberMembershipType();
    }
}
