namespace NIA_CRM.ViewModels
{
    public class ContactViewModel
    {
        public int ID { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactMiddleName { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }
        public string LinkedinUrl { get; set; }
        public bool IsVIP { get; set; }
        public string Summary { get; set; }
    }
}
