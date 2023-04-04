using System.Collections.Generic;
using System.Threading.Tasks;
using Ave.DAL.Entities;

namespace Ave.BLL.Interface
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetDoctorByInstituationId(int id);

        Task<List<Doctor>> GetAll();

        Task<Doctor> GetById(int id);

        Task Update(Doctor model);

        Task<List<Doctor>> GetDoctorsByFilter(string filter);


    }
}
