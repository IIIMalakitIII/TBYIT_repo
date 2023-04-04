using System;

namespace Ave.DAL.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime? CreatedAt { get; set; }
    }
}
