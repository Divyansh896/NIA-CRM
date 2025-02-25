namespace NIA_CRM.Models
{
    public class MemberTag
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }

        public int MTagID { get; set; }

        public MTag? MTag { get; set; }
    }
}
