using System.Reflection;

namespace NIA_CRM.Models
{
    public class MemberMembershipType
    {
        public int MemberID { get; set; }
        public Member Member { get; set; }
        public int MembershipTypeID { get; set; }
        public MembershipType MembershipType { get; set; }
    }
}