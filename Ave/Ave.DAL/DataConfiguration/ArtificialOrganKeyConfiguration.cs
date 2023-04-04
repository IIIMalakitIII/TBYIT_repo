using Ave.DAL.Entities;
using Ave.DAL.Сonstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ave.DAL.DataConfiguration
{
    public class ArtificialOrganKeyConfiguration: IEntityTypeConfiguration<ArtificialOrganKey>
    {
        public void Configure(EntityTypeBuilder<ArtificialOrganKey> builder)
        {
            builder.ToTable(TableName.ArtificialOrganKeys);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AccessKey)
                .IsRequired();

            builder.Property(x => x.CreatedAt);

            builder.HasIndex(x => x.ArtificialOrganId);

            builder.HasOne(x => x.ArtificialOrgan)
                .WithMany(x => x.ArtificialOrganKeys)
                .HasForeignKey(x => x.ArtificialOrganId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
