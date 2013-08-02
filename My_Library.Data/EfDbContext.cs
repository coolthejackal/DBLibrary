using System.Data.Entity;
using My_Library.Core.Domain;

namespace My_Library.Data
{
    public class EfDbContext
        : DbContext
    {
        public EfDbContext()
            : base("DefaultConneciton")
        {
            // Do NOT enable proxied entities, else serialization fails
            Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            Configuration.LazyLoadingEnabled = false;

            // Because Web API will perform validation, we don't need/want EF to do so
            Configuration.ValidateOnSaveEnabled = false;

            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            // We won't use this performance tweak because we don't need 
            // the extra performance and, when autodetect is false,
            // we'd have to be careful. We're not being that careful.
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Entity<FirmDescription>()
            //  .HasMany<UserProfile>(r => r.Users)
            //  .WithMany(u => u.FirmDescriptions)
            //  .Map(m =>
            //  {
            //      m.ToTable("ISG_UserProfileFirmDescription");
            //      m.MapLeftKey("FirmId");
            //      m.MapRightKey("UserId");
            //  });

            //modelBuilder.Entity<FirmActivityRecord>()
            //            .HasMany(f => f.Details)
            //            .WithRequired(r => r.FirmActivityRecord)
            //            .HasForeignKey(f => f.FirmActivityRecordId);

        }
    }
}
