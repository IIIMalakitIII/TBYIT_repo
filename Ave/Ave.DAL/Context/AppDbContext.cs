using Ave.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ave.DAL.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Confidant> Confidants { get; set; }

        public DbSet<DiseaseHistory> DiseaseHistories { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<ArtificialOrgan> ArtificialOrgans { get; set; }

        public DbSet<ArtificialOrganKey> ArtificialOrganKeys { get; set; }

        public DbSet<MedicalInstitution> MedicalInstitutions { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Record> Records { get; set; }

        public DbSet<OrganData> OrganDates { get; set; }

        public DbSet<ConnectHistory> ConnectHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
