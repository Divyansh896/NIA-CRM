namespace NIA_CRM.Models
{
    public class ContactIndustry
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int IndustryId { get; set; }

        public Contact Contact { get; set; }
        public Industry Industry { get; set; }
    }
}
