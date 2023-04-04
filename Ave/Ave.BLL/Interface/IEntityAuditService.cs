using Microsoft.EntityFrameworkCore;

namespace Ave.BLL.Interface
{
    public interface IEntityAuditService
    {
        void ApplyAuditRules(DbContext context);
    }
}
