using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Ave.DAL.Entities
{
    public class User : IdentityUser
    {
        public Patient Patient { get; set; }

        public Doctor Doctor { get; set; }

        public virtual List<ConnectHistory> ConnectHistories { get; set; }
    }
}
