using System;
using Ave.DAL.Enums;

namespace Ave.DAL.Entities
{
    public class Record: AuditableEntity
    {
        public int Id { get; set; } 

        public DateTime RecordingTime { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public string Description { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public Doctor Doctor { get; set; }

        public Patient Patient { get; set; }
    }
}
