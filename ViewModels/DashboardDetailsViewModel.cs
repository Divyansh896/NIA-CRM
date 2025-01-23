namespace NIA_CRM.ViewModels
{
    public class DashboardDetailsViewModel
    {
        public int ID { get; set; }
        public string MemberFirstName { get; set; } = "";
        public string? MemberMiddleName { get; set; }
        public string MemberLastName { get; set; } = "";
        
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string IndustryName { get; set; }
        public int IndustryID { get; set; }

        public AddressViewModel Address { get; set; }
        public List<ContactViewModel> Contacts { get; set; }
    }
}
