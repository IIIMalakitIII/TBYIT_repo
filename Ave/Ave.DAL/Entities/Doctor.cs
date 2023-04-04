using System.Collections.Generic;

namespace Ave.DAL.Entities
{
    public class Doctor
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int MedicalInstitutionId { get; set; }

        public bool RecordingAvailable  { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string License { get; set; }

        public string Phone { get; set; }

        public ICollection<Record> Records { get; set; }

        public User User { get; set; }

        public MedicalInstitution MedicalInstitution { get; set; }

        public List<Confidant> Confidants { get; set; }
    }
}
