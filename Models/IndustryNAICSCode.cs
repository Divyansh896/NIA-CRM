namespace NIA_CRM.Models
{
    public class IndustryNAICSCode
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int NAICSCodeId { get; set; }

        public Member Member { get; set; }
        public NAICSCode NAICSCode { get; set; }
    }
}
