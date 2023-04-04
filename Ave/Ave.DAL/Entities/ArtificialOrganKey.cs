using System;

namespace Ave.DAL.Entities
{
    public class ArtificialOrganKey: AuditableEntity
    {
        public int Id { get; set; }

        public int ArtificialOrganId { get; set; }

        public Guid AccessKey { get; set; }

        public ArtificialOrgan  ArtificialOrgan { get; set; }
    }
}
