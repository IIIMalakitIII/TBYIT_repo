namespace Ave.DAL.Entities
{
    public class DiseaseHistory: AuditableEntity
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string Description { get; set; }

        public Patient Patient { get; set; }
    }
}
