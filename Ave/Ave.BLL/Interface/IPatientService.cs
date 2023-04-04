using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ave.DAL.Entities;

namespace Ave.BLL.Interface
{
    public interface IPatientService
    {
        Task<List<Patient>> GetAll();

        Task<Patient> GetById(int id);

        Task Update(Patient model);

        Task<List<Patient>> GetPatientByFilter(string filter);

    }
}
