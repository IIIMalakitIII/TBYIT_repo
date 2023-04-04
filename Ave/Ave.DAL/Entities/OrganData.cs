using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ave.DAL.Entities
{
    public class OrganData: AuditableEntity
    {
        private string _extendedData;

        public int Id { get; set; }

        public int ArtificialOrganId { get; set; }

        public JObject ExtendedData { get; set; }

        public ArtificialOrgan  ArtificialOrgan { get; set; }
    }
}
