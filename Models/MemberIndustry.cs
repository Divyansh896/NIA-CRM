namespace NIA_CRM.Models
{
    public class MemberIndustry
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int IndustryId { get; set; }

        public Member Member { get; set; }
        public Industry Industry { get; set; }
    }
}
