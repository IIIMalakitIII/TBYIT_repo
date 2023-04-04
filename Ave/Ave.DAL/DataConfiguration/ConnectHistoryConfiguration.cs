using Ave.DAL.Entities;
using Ave.DAL.Сonstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ave.DAL.DataConfiguration
{
    public class ConnectHistoryConfiguration : IEntityTypeConfiguration<ConnectHistory>
    {
        public void Configure(EntityTypeBuilder<ConnectHistory> builder)
        {
            builder.ToTable(TableName.ConnectHistories);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description)
                .IsRequired();

            builder.Property(x => x.OrganId)
                .IsRequired();

            builder.Property(x => x.CreatedAt);

            builder.Property(x => x.UserId);

            builder.HasIndex(x => x.OrganId);

            builder.HasOne(x => x.Organ)
                .WithMany(x => x.ConnectHistories)
                .HasForeignKey(x => x.OrganId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany(x => x.ConnectHistories)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
