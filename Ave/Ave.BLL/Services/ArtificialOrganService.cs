using Ave.BLL.Interface;
using Ave.Common.Authentication;
using Ave.Common.Exceptions;
using Ave.DAL.Context;
using Ave.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Ave.BLL.Services
{
    public class ArtificialOrganService : IArtificialOrganService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private const int MOCMIN = 4;
        private const int MOCMAX = 5;
        private const int SYSTOLEMAX = 140;
        private const int DISTOLEMAX = 90;

        public ArtificialOrganService(AppDbContext dbContext, UserManager<User> userManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }


        public async Task<string> DiagnosticArtificialOrgan(int id, DateTime from, DateTime to)
        {
            var artificial = await _dbContext.ArtificialOrgans.FirstOrDefaultAsync(x => x.Id == id);

            if (artificial == null)
            {
                throw new NullReferenceException($"Artificial not found with id {id}");
            }

            var artificialDates = await _dbContext.OrganDates
                .Where(x => x.ArtificialOrganId == id && x.CreatedAt >= from && x.CreatedAt <= to)
                .ToListAsync();


            var resultText = new StringBuilder();

            if (artificialDates.Any())
            {
                CalculateMinuteBloodVolume(artificialDates, resultText);
                CheckingForTachycardia(artificialDates, resultText);
            }

            return resultText.ToString();
        }

        private void CalculateMinuteBloodVolume(List<OrganData> artificialDates, StringBuilder resultText)
        {
            var calculation = new Dictionary<DateTime, int>();

            artificialDates.ForEach(x =>
            {
                if (x.ExtendedData != null)
                {
                    var sv = x.ExtendedData.GetType().GetProperty("SV")?.GetValue(x.ExtendedData, null);
                    var hr = x.ExtendedData.GetType().GetProperty("hr")?.GetValue(x.ExtendedData, null);

                    if (sv != null && hr != null)
                    {
                        calculation.Add((DateTime)x.CreatedAt, (int) hr * (int) sv);
                    }
                }
            });

            var result = new List<DateTime>();

            calculation.ForEach(x =>
            {
                if (!result.Any(o => o.Day == x.Key.Day && o.Month == x.Key.Month))
                {
                    var mocOnDay = calculation.Where(o => o.Key.Day == x.Key.Day && o.Key.Month == x.Key.Month)
                        .Select(o => o.Value)
                        .ToList();

                    if (mocOnDay.Any(o => o < MOCMIN))
                    {
                        var moc = mocOnDay.FirstOrDefault(o => o < MOCMIN);
                        resultText.Append($"<br>At {x.Key.Date} The minute volume of blood circulation is lowered. {moc}");
                    } 
                    else if (mocOnDay.Any(o => o > MOCMAX))
                    {
                        resultText.Append($"<br>At {x.Key.Date} The minute volume of blood circulation is increased. {mocOnDay.FirstOrDefault(o => o > MOCMAX)}");
                    }
                    else
                    {
                        resultText.Append($"<br>At {x.Key.Date} The minute volume of blood circulation is normal. {mocOnDay.Average()}");
                    }

                    result.Add(x.Key);
                }
            });
        }

        private void CheckingForTachycardia(List<OrganData> artificialDates, StringBuilder resultText)
        {
            var calculation = new Dictionary<DateTime, Dictionary<int, int>>();

            artificialDates.ForEach(x =>
            {
                if (x.ExtendedData != null)
                {
                    var systole = x.ExtendedData.GetType().GetProperty("systole")?.GetValue(x.ExtendedData, null);
                    var diastole = x.ExtendedData.GetType().GetProperty("diastole")?.GetValue(x.ExtendedData, null);

                    if (systole != null && diastole != null)
                    {
                        var res = new Dictionary<int, int>();
                        res.Add((int) systole, (int) diastole);
                        calculation.Add((DateTime)x.CreatedAt, res);
                    }
                }
            });

            var result = new List<DateTime>();

            calculation.ForEach(x =>
            {
                if (!result.Any(o => o.Day == x.Key.Day && o.Month == x.Key.Month))
                {
                    var mocOnDay = calculation.Where(o => o.Key.Day == x.Key.Day && o.Key.Month == x.Key.Month)
                        .Select(x => x.Value)
                        .ToList();

                    if (mocOnDay.Any(o => o.Keys.Average() >= SYSTOLEMAX  && o.Values.Average() >= DISTOLEMAX))
                    {
                        var systole = mocOnDay.Average(d => d.Keys.Average());
                        var diastole = mocOnDay.Average(d => d.Values.Average());
                        resultText.Append($"<br>At {x.Key.Date} There are signs of tachycardia. Systole - {systole}, Diastole - {diastole}.");
                    }
                    else
                    {
                        resultText.Append($"<br>At {x.Key.Date} No signs of tachycardia were foundю");
                    }

                    result.Add(x.Key);
                }
            });
        }

        public async Task<ArtificialOrgan> CreateArtificialOrgan(ArtificialOrgan model)
        {
            await _dbContext.ArtificialOrgans.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task AddOrganData(OrganData model, Guid key)
        {
            var organKey = await _dbContext.ArtificialOrganKeys.FirstOrDefaultAsync(x => x.AccessKey == key);

            if (organKey == null)
            {
                throw new NullReferenceException("Key not found");
            }

            var organ = await _dbContext.ArtificialOrgans.FirstOrDefaultAsync(x => x.Id == organKey.ArtificialOrganId);

            if (organ == null)
            {
                throw new NullReferenceException("Organ not found");
            }

            model.CreatedAt = DateTime.UtcNow;

            await _dbContext.OrganDates.AddAsync(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ArtificialOrgan> GetOrganById(int id, string userId, string role)
        {
            var organ = await _dbContext.ArtificialOrgans
                .Include(x => x.OrganDatas)
                .FirstOrDefaultAsync(x => x.Id == id);

            await _dbContext.ConnectHistories.AddAsync(new ConnectHistory()
            {
                CreatedAt = DateTime.UtcNow,
                Description = await GetUserInfoById(userId, role),
                Id = 0,
                OrganId = id,
                UserId = userId
            });

            if (organ == null)
            {
                throw new NullReferenceException("Organ not found");
            }

            return organ;
        }

        public async Task<List<ArtificialOrgan>> GetPatientOrgansById(int patientId, string id, string role, string userId)
        {
            if (role == Role.Doctor)
            {
                if (!await _dbContext.Confidants.AnyAsync(
                    x => x.DoctorId == int.Parse(id) && x.PatientId == patientId))
                {
                    throw new BusinessLogicException("You have no access to this info");
                }
            }

            var organ = await _dbContext.ArtificialOrgans
                .Include(x => x.OrganDatas)
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

            if (organ == null)
            {
                throw new NullReferenceException("Organ not found");
            }

            organ.ForEach(x =>
            {
                _dbContext.ConnectHistories.Add(new ConnectHistory()
                {
                    CreatedAt = DateTime.UtcNow,
                    Description = GetUserInfoById(userId, role).Result,
                    Id = 0,
                    OrganId = x.Id,
                    UserId = userId
                });
            });

            return organ;
        }

        public async Task<List<OrganData>> GetOrganData(int id, string userId, string role)
        {
            var organ = await _dbContext.ArtificialOrgans
                .Include(x => x.OrganDatas)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (organ == null)
            {
                throw new NullReferenceException("Organ not found");
            }

            if (!await _dbContext.Confidants.AnyAsync(x => x.PatientId == organ.PatientId && x.Doctor.UserId == userId))
            {
                throw new BusinessLogicException("You have no access");
            }

            await _dbContext.ConnectHistories.AddAsync(new ConnectHistory()
            {
                CreatedAt = DateTime.UtcNow,
                Description = await GetUserInfoById(userId, role),
                Id = 0,
                OrganId = id,
                UserId = userId
            });

            var organData = await _dbContext.OrganDates
                .Where(x => x.ArtificialOrganId == id)
                .ToListAsync();


            return organData;
        }

        public async Task GenerateOrganKey(int id, string userId, string role)
        {
            await GetOrganById(id, userId, role);

            for (int x = 0; x < 4; x++)
            {
                await _dbContext.ArtificialOrganKeys.AddAsync(new ArtificialOrganKey()
                {
                    Id = 0,
                    AccessKey = Guid.NewGuid(),
                    ArtificialOrganId = id,
                });
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ArtificialOrganKey>> GetOrganKeys(int id)
        {
            return await _dbContext.ArtificialOrganKeys.Where(x => x.ArtificialOrganId == id).ToListAsync();
        }

        public async Task UpdateOrgan(ArtificialOrgan model)
        {
            _dbContext.ArtificialOrgans.Update(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateConfidant(string userId, int doctorId)
        {
            var patient = await _dbContext.Patients.FirstOrDefaultAsync(x => x.UserId == userId);
            if (patient != null)
            {
                var existConfidants = await _dbContext.Confidants
                    .AsNoTracking()
                    .AnyAsync(x => x.PatientId == patient.Id && x.DoctorId == doctorId);

                if (existConfidants)
                {
                    return;
                }

                await _dbContext.Confidants.AddAsync(new Confidant()
                {
                    DoctorId = doctorId,
                    PatientId = patient.Id,
                    Id = 0
                });
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Confidant>> GetConfidants(string userId)
        {
            var patient = await _dbContext.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (patient != null)
            {
                return await _dbContext.Confidants
                    .AsNoTracking()
                    .Include(x => x.Patient)
                    .Include(x => x.Doctor)
                    .Where(x => x.PatientId == patient.Id)
                    .ToListAsync();
            }

            var doctor = await _dbContext.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (doctor != null)
            {
                return await _dbContext.Confidants
                    .AsNoTracking()
                    .Include(x => x.Patient)
                    .Include(x => x.Doctor)
                    .Where(x => x.DoctorId == doctor.Id)
                    .ToListAsync();
            }

            return new List<Confidant>();
        }

        public async Task DeleteConfidant(string userId, int confidantId)
        {
            var patient = await _dbContext.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (patient != null)
            {
                var confidant =
                    await _dbContext.Confidants.FirstOrDefaultAsync(x =>
                        x.PatientId == patient.Id && x.Id == confidantId);
                if (confidant == null)
                {
                    throw new NullReferenceException("Patient have not current confidant");
                }

                _dbContext.Confidants.Remove(confidant);
                await _dbContext.SaveChangesAsync();
                return;
            }

            var doctor = await _dbContext.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (doctor != null)
            {
                var confidant =
                    await _dbContext.Confidants.FirstOrDefaultAsync(x =>
                        x.DoctorId == doctor.Id && x.Id == confidantId);
                if (confidant == null)
                {
                    throw new NullReferenceException("Doctor have not current confidant");
                }

                _dbContext.Confidants.Remove(confidant);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<ConnectHistory>> GetConnectHistory(int organId, string userId)
        {
            var organ = await _dbContext.ArtificialOrgans.FirstOrDefaultAsync(x =>
                x.Id == organId && x.Patient.UserId == userId);

            if (organ == null)
            {
                throw new BusinessLogicException("You have no access");
            }

            return await _dbContext.ConnectHistories.Where(x => x.OrganId == organId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(25)
                .ToListAsync();
        }

        private async Task<string> GetUserInfoById(string userId, string role)
        {
            var user = await _dbContext.Users.AsNoTracking()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return "Not found user info";
            }

            if (role == Role.Doctor)
            {
                return "Try to connect " + user.Doctor.LastName + " " + user.Doctor.FirstName + " " + user.Email;
            }
            else if (role == Role.Patient)
            {
                return "Try to connect " + user.Patient.LastName + " " + user.Patient.FirstName + " " + user.Email;
            }
            else if (role == Role.Admin)
            {
                return "Try to connect Admin";
            }
            else
            {
                throw new BusinessLogicException("Something wrong user role not known");
            }
        }
    }
}
