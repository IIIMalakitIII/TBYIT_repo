using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ave.BLL.Interface;
using Ave.DAL.Context;
using Ave.DAL.Entities;
using Ave.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace Ave.BLL.Services
{
    public class PatientService: IPatientService
    {
        private readonly AppDbContext _dbContext;

        public PatientService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Patient>> GetAll()
        {
            return await _dbContext.Patients.AsNoTracking().ToListAsync();
        }

        public async Task<Patient> GetById(int id)
        {
            return await _dbContext.Patients.AsNoTracking()
                .Include(x => x.Records)
                .Include(x => x.Confidants)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Patient>> GetPatientByFilter(string filter)
        {
            filter = filter.ToLower();
            var keywords = filter.Split(' ');
            var patients = await _dbContext.Patients.AsNoTracking()
                .Include(x => x.User)
                .ToListAsync();

            patients = patients.Where(x =>
                    keywords.Any(y => x.FirstName.ToLower().Contains(y)) ||
                    keywords.Any(y => x.LastName.ToLower().Contains(y)) ||
                    keywords.Any(y => x.Address.ToLower().Contains(y)) ||
                    keywords.Any(y => x.Country.ToLower().Contains(y)) ||
                    keywords.Any(y => x.Passport.ToLower().Contains(y)) ||
                    keywords.Any(y => x.Phone.ToLower().Contains(y)) ||
                    keywords.Any(y => x.User.Email.ToLower().Contains(y)) ||
                    keywords.Any(y => x.User.UserName.ToLower().Contains(y)))
                .ToList();

            return patients;
        }

        public async Task Update(Patient model)
        {
            if (!_dbContext.Patients.Any(x => x.Id == model.Id))
            {
                throw new BusinessLogicException($"Patient with id: {model.Id} doesn't exist");
            }

            _dbContext.Patients.Update(model);

            await _dbContext.SaveChangesAsync();
        }
    }
}
