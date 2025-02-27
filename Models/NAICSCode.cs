namespace NIA_CRM.Models
{
    public class NAICSCode
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }

        public ICollection<IndustryNAICSCode> IndustryNAICSCodes { get; set; } = new List<IndustryNAICSCode>();
    }
}
