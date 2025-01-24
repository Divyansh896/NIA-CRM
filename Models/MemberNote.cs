namespace NIA_CRM.Models
{
    public class MemberNote
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedAt { get; set; }

        public Member Member { get; set; }
    }
}
