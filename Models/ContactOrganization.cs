namespace NIA_CRM.Models
{
    public class ContactOrganization
    {
        public int ContactID { get; set; }
        public Contact Contact { get; set; }

        public int OrganizationID { get; set; }
        public Organization Organization { get; set; }
    }
}