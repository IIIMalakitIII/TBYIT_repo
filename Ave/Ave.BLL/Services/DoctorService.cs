using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ave.BLL.Interface;
using Ave.DAL.Context;
using Ave.DAL.Entities;
using Ave.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Ave.BLL.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _dbContext;

        public DoctorService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Doctor>> GetDoctorByInstituationId(int id)
        {
            return await _dbContext.Doctors.AsNoTracking()
                .Where(x => x.MedicalInstitutionId == id)
                .OrderByDescending(x => x.LastName)
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetDoctorsByFilter(string filter)
        {
            filter = filter.ToLower();
            var keywords = filter.Split(' ');
            var doctors = await _dbContext.Doctors.AsNoTracking()
                .Include(x => x.MedicalInstitution)
                .Include(x => x.User)
                .ToListAsync();

            doctors = doctors.Where(x =>
                    keywords.Any(y => x.FirstName.ToLower().Contains(y)) ||
                    keywords.Any(y => x.LastName.ToLower().Contains(y)) ||
                    keywords.Any(y => x.MedicalInstitution.Name.ToLower().Contains(y)) ||
                    keywords.Any(y => x.Phone.ToLower().Contains(y)) ||
                    keywords.Any(y => x.User.Email.ToLower().Contains(y)) ||
                    keywords.Any(y => x.User.UserName.ToLower().Contains(y)))
                .ToList();

            return doctors;
        }

        public async Task<List<Doctor>> GetAll()
        {
            return await _dbContext.Doctors
                .AsNoTracking()
                .OrderByDescending(x => x.LastName)
                .ToListAsync();
        }

        public async Task<Doctor> GetById(int id)
        {
            return await _dbContext.Doctors.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Doctor model)
        {
            if (!_dbContext.Doctors.Any(x => x.Id == model.Id))
            {
                throw new BusinessLogicException($"Doctor with id: {model.Id} doesn't exist");
            }

            _dbContext.Doctors.Update(model);

            await _dbContext.SaveChangesAsync();
        }
    }
}
