using System.Linq;
using System.Threading.Tasks;
using Ave.DAL.Entities;
using Ave.DAL.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IEntityAuditService = Ave.BLL.Interface.IEntityAuditService;
using IUnitOfWork = Ave.BLL.Interface.IUnitOfWork;

namespace Ave.BLL.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : IdentityDbContext<User>
    {
        private readonly TContext _context;
        private readonly IEntityAuditService _entityAuditService;

        public UnitOfWork(TContext context, IEntityAuditService entityAuditService)
        {
            _context = context;
            _entityAuditService = entityAuditService;
        }

        public async Task SaveChangesAsync()
        {
            _entityAuditService.ApplyAuditRules(_context);
            await _context.SaveChangesAsync();
            DetachAll();
        }

        public void Rollback()
        {
            DetachAll();
        }

        /// <summary>
        /// Detaches all objects that have been added to the ChangeTracker.
        /// </summary>
        private void DetachAll()
        {
            // don't remove .ToArray()
            var dbEntityEntries = _context.ChangeTracker.Entries().ToArray();

            foreach (var dbEntityEntry in dbEntityEntries)
            {
                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
        }
    }
}
