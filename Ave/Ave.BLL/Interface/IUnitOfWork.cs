using System.Threading.Tasks;

namespace Ave.BLL.Interface
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        void Rollback();
    }
}
