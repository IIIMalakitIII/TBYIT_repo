using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ave.DAL.Entities;

namespace Ave.API.ViewModels
{
    public class ArtificialOrganViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PatientId { get; set; }

        public virtual List<ArtificialOrganKey> ArtificialOrganKeys { get; set; }

        public virtual List<OrganDataViewModel> OrganDatas { get; set; }
    }
}
