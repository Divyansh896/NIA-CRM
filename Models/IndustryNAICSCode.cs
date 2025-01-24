namespace NIA_CRM.Models
{
    public class IndustryNAICSCode
    {
        public int Id { get; set; }
        public int IndustryId { get; set; }
        public int NAICSCodeId { get; set; }

        public Industry Industry { get; set; }
        public NAICSCode NAICSCode { get; set; }
    }
}
