namespace NIA_CRM.Models
{
    public class MemberSector
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }

        public int SectorId { get; set; }

        public Sector? Sector { get; set; }
    }
}
