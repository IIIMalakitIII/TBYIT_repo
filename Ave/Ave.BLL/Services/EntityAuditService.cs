using System;
using Ave.DAL.Entities;
using Ave.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using IEntityAuditService = Ave.BLL.Interface.IEntityAuditService;

namespace Ave.DAL.Services
{
    public class EntityAuditService : IEntityAuditService
    {
 
        public EntityAuditService()
        {
        }

        public void ApplyAuditRules(DbContext context)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                entry.Entity.CreatedAt = now;
            }
        }
    }
}
