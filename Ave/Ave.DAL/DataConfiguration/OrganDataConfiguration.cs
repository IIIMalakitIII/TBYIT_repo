using System.Collections.Generic;
using Ave.DAL.Entities;
using Ave.DAL.Сonstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ave.DAL.DataConfiguration
{
    public class OrganDataConfiguration: IEntityTypeConfiguration<OrganData>
    {
        public void Configure(EntityTypeBuilder<OrganData> builder)
        {

            builder.ToTable(TableName.OrganDates);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt);

            builder.Property(e => e.ExtendedData).HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<JObject>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            builder.HasIndex(x => x.ArtificialOrganId);

            builder.HasOne(x => x.ArtificialOrgan)
                .WithMany(x => x.OrganDatas)
                .HasForeignKey(x => x.ArtificialOrganId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        
    }
}
