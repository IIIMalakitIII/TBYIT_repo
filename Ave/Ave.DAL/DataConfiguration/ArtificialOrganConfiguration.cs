using Ave.DAL.Entities;
using Ave.DAL.Сonstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ave.DAL.DataConfiguration
{
    public class ArtificialOrganConfiguration : IEntityTypeConfiguration<ArtificialOrgan>
    {
        public void Configure(EntityTypeBuilder<ArtificialOrgan> builder)
        {
            builder.ToTable(TableName.ArtificialOrgans);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(StringLengthConstants.SmallLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(StringLengthConstants.MediumLength);

            builder.HasIndex(x => x.PatientId);

            builder.HasOne(x => x.Patient)
                .WithMany(x => x.ArtificialOrgans)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
