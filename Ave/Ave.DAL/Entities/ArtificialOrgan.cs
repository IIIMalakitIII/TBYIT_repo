using System.Collections.Generic;

namespace Ave.DAL.Entities
{
    public class ArtificialOrgan: AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public virtual List<ArtificialOrganKey> ArtificialOrganKeys { get; set; }

        public virtual List<OrganData> OrganDatas { get; set; }

        public virtual List<ConnectHistory> ConnectHistories { get; set; }
    }
}
