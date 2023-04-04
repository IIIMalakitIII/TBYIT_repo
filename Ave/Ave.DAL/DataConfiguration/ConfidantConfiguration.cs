using Ave.DAL.Entities;
using Ave.DAL.Сonstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ave.DAL.DataConfiguration
{
    public class ConfidantConfiguration : IEntityTypeConfiguration<Confidant>
    {
        public void Configure(EntityTypeBuilder<Confidant> builder)
        {
            builder.ToTable(TableName.Confidants);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt);

            builder.HasIndex(x => x.PatientId);
            builder.HasIndex(x => x.DoctorId);

            builder.HasOne(x => x.Patient)
                .WithMany(x => x.Confidants)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Doctor)
                .WithMany(x => x.Confidants)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
