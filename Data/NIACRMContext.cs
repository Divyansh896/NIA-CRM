using System.Numerics;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.Models;


namespace NIA_CRM.Data
{
    public class NIACRMContext : DbContext
    {

        public NIACRMContext(DbContextOptions<NIACRMContext> options)
            : base(options)
        {
        }

        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationCode> OrganizationCodes { get; set; }
        public DbSet<MemberMembershipType> MemberMembershipTypes { get; set; }
        public DbSet<ContactOrganization> ContactOrganizations { get; set; }
        public DbSet<ProductionEmail> ProductionEmails { get; set; }
        public DbSet<Notes> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //1:m relationships
            modelBuilder.Entity<Contact>()
                .HasMany<Interaction>(c => c.Interactions)
                .WithOne(c =>c.Contact)
                .HasForeignKey(c => c.ContactID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
               .HasMany<ContactOrganization>(c => c.ContactOrganizations)
               .WithOne(c => c.Contact)
               .HasForeignKey(c => c.ContactID)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Industry>()
                .HasMany<Organization>(c => c.Organizations)
                .WithOne(c => c.Industry)
                .HasForeignKey(c => c.IndustryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Member>()
                .HasMany<Cancellation>(d => d.Cancellations)
                .WithOne(p => p.Member)
                .HasForeignKey(p => p.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Member>()
                .HasOne<Address>(d => d.Address)
                .WithOne(p => p.Member)
                .HasForeignKey<Address>(p => p.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Organization>()
               .HasMany<OrganizationCode>(d => d.OrganizationCodes)
               .WithOne(p => p.Organization)
               .HasForeignKey(p => p.OrganizationID)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Contact>()
        .HasMany(c => c.Notes)
        .WithOne(n => n.Contact)
        .HasForeignKey(n => n.ContactID);
            //m:m
            modelBuilder.Entity<Organization>()
              .HasMany<ContactOrganization>(d => d.ContactOrganizations)
              .WithOne(p => p.Organization)
              .HasForeignKey(p => p.OrganizationID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
              .HasMany<ContactOrganization>(d => d.ContactOrganizations)
              .WithOne(p => p.Contact)
              .HasForeignKey(p => p.ContactID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Member>()
              .HasMany<MemberMembershipType>(d => d.MemberMembershipTypes)
              .WithOne(p => p.Member)
              .HasForeignKey(p => p.MemberID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MembershipType>()
              .HasMany<MemberMembershipType>(d => d.MemberMembershipTypes)
              .WithOne(p => p.MembershipType)
              .HasForeignKey(p => p.MembershipTypeID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ContactOrganization>()
            .HasKey(t => new { t.ContactID, t.OrganizationID });

            modelBuilder.Entity<MemberMembershipType>()
            .HasKey(t => new { t.MemberID, t.MembershipTypeID });

            //1:1
            modelBuilder.Entity<Opportunity>()
              .HasOne<Interaction>(d => d.Interaction)
              .WithOne(p => p.Opportunity)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductionEmail>()
               .HasIndex(e => e.EmailType)  
               .IsUnique();

        }
    }
}
