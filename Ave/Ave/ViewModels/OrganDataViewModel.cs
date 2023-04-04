using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ave.DAL.Entities;

namespace Ave.API.ViewModels
{
    public class OrganDataViewModel
    {
        public int Id { get; set; }

        public int ArtificialOrganId { get; set; }

        public string ExtendedData { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
