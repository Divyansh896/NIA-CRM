using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.Models;

namespace NIA_CRM.Data
{
    public class NIACRMContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string UserName { get; private set; }

        public NIACRMContext(DbContextOptions<NIACRMContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            UserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
        }

        public NIACRMContext(DbContextOptions<NIACRMContext> options)
            : base(options)
        {
            UserName = "Seed Data";
        }

        // DbSets for entities
        public DbSet<Member> Members { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<MemberMembershipType> MemberMembershipTypes { get; set; }
        public DbSet<ContactIndustry> ContactIndustries { get; set; }
        public DbSet<ProductionEmail> ProductionEmails { get; set; }
        public DbSet<MemberIndustry> MemberIndustries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationships and constraints

            // Member -> MembershipType (Many-to-Many)
            modelBuilder.Entity<MemberMembershipType>()
                .HasKey(mmt => new { mmt.MemberId, mmt.MembershipTypeId });

            modelBuilder.Entity<MemberMembershipType>()
                .HasOne(mmt => mmt.Member)
                .WithMany(m => m.MemberMembershipTypes)
                .HasForeignKey(mmt => mmt.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemberMembershipType>()
                .HasOne(mmt => mmt.MembershipType)
                .WithMany(mt => mt.MemberMembershipTypes)
                .HasForeignKey(mmt => mmt.MembershipTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Contact -> Industry (Many-to-Many)
            modelBuilder.Entity<ContactIndustry>()
                .HasKey(ci => new { ci.ContactId, ci.IndustryId });

            modelBuilder.Entity<ContactIndustry>()
                .HasOne(ci => ci.Contact)
                .WithMany(c => c.ContactIndustries)
                .HasForeignKey(ci => ci.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ContactIndustry>()
                .HasOne(ci => ci.Industry)
                .WithMany(i => i.ContactIndustries)
                .HasForeignKey(ci => ci.IndustryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Address -> Member (One-to-Many)
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Member)
                .WithMany(m => m.Addresses)
                .HasForeignKey(a => a.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            // Industry -> Opportunity (One-to-Many)
            modelBuilder.Entity<Opportunity>()
                .HasOne(o => o.Industry)
                .WithMany(i => i.Opportunities)
                .HasForeignKey(o => o.IndustryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Member -> Cancellation (One-to-Many)
            modelBuilder.Entity<Cancellation>()
                .HasOne(c => c.Member)
                .WithMany(m => m.Cancellations)
                .HasForeignKey(c => c.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            // Interaction -> Opportunity (Optional One-to-One)
            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.Opportunity)
                .WithMany(i => i.Interactions)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }
        public DbSet<NIA_CRM.Models.NAICSCode> NAICSCode { get; set; } = default!;
    }
}
