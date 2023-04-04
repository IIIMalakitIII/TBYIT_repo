﻿using Ave.DAL.Entities;
using Ave.DAL.Сonstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ave.DAL.DataConfiguration
{
    public class RecordConfiguration : IEntityTypeConfiguration<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {
            builder.ToTable(TableName.Records);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RecordStatus)
                .IsRequired();

            builder.Property(x => x.RecordingTime)
                .IsRequired();

            builder.Property(x => x.Description);

            builder.Property(x => x.DoctorId)
                .IsRequired();

            builder.Property(x => x.PatientId)
                .IsRequired();

            builder.HasIndex(x => x.DoctorId);
            builder.HasIndex(x => x.PatientId);

            builder.Property(x => x.CreatedAt);


            builder.HasOne(x => x.Doctor)
                .WithMany(x => x.Records)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Patient)
                .WithMany(x => x.Records)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
