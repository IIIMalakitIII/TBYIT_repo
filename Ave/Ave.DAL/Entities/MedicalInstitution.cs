using System.Collections.Generic;

namespace Ave.DAL.Entities
{
    public class MedicalInstitution: AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
    }
}
