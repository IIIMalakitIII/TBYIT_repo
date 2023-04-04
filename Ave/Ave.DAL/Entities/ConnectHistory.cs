namespace Ave.DAL.Entities
{
    public class ConnectHistory: AuditableEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Description { get; set; }

        public int OrganId { get; set; }

        public ArtificialOrgan Organ { get; set; }

        public User User { get; set; }
    }
}
