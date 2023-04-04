using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ave.BLL.Interface;
using Ave.DAL.Context;
using Ave.DAL.Entities;
using Ave.Common.Authentication;
using Ave.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ave.BLL.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public AccountService(UserManager<User> userManager,
            AppDbContext dbContext,
            IOptions<JwtSettings> jwtOptions,
            IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _mapper = mapper;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<string> SignIn(string email, string password)
        {
            var user = await _userManager
                .FindByEmailAsync(email);

            if (user is null || (!await _userManager.CheckPasswordAsync(user, password)))
            {
                throw new BusinessLogicException("Email or password is incorrect.");
            }

            user.Patient = await _dbContext.Patients.FirstOrDefaultAsync(x => x.UserId == user.Id);
            user.Doctor = await _dbContext.Doctors.FirstOrDefaultAsync(x => x.UserId == user.Id);

            var token = await GenerateToken(user);

            return token;
        }

        public async Task<string> SignUpDoctor(string userName, string email, string password, string role, Doctor model)
        {
            var user = new User
            {
                UserName = userName,
                Email = email,
                Doctor = model
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new BusinessLogicException(string.Join("\n", result.Errors.Select(x => x.Description)));
            }

            await _userManager.AddToRoleAsync(user, role);

            return user.Id;
        }

        public async Task<string> SignUpPatient(string userName, string email, string password, string role, Patient model)
        {
            var user = new User
            {
                UserName = userName,
                Email = email,
                Patient = model
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new BusinessLogicException(string.Join("\n", result.Errors.Select(x => x.Description)));
            }

            await _userManager.AddToRoleAsync(user, role);

            return user.Id;
        }

        public async Task ChangePassword(string id, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                throw new BusinessLogicException(string.Join("\n", result.Errors.Select(x => x.Description)));
            }
        }

        public async Task<User> GetAccountInfo(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new NullReferenceException("User don't exist.");
            }

            var patient = await _dbContext.Patients.AsNoTracking()
                .Include(x => x.DiseaseHistory)
                .Include(x => x.Confidants)
                .Where(x => x.UserId == user.Id)
                .FirstOrDefaultAsync();

            var doctor = await _dbContext.Doctors.AsNoTracking()
                .Include(x => x.MedicalInstitution)
                .Include(x => x.Confidants)
                .Where(x => x.UserId == user.Id)
                .FirstOrDefaultAsync();

            user.Patient = patient;
            user.Doctor = doctor;

            return user;
        }

        public async Task<List<User>> GetAccountInfos()
        {
            var users = await _userManager.Users.ToListAsync();

            if (users == null)
            {
                throw new NullReferenceException("Users don't exist.");
            }

            users.ForEach(x =>
            {
                var patient = _dbContext.Patients.AsNoTracking()
                    .Include(o => o.DiseaseHistory)
                    .Include(o => o.Confidants)
                    .FirstOrDefault(o => o.UserId == x.Id);

                var doctor = _dbContext.Doctors.AsNoTracking()
                    .Include(o => o.MedicalInstitution)
                    .Include(o => o.Confidants)
                    .FirstOrDefault(o => o.UserId == x.Id);

                x.Patient = patient;
                x.Doctor = doctor;
            });



            return users;
        }

        public async Task UpdateAccountInfo(string id, string newUserName, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new NullReferenceException("User don't exist.");
            }

            user.UserName = newUserName;
            user.Email = newEmail;
            await _userManager.UpdateAsync(user);
        }

        private async Task<string> GenerateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = roles
                .Select(x => new Claim(CustomClaimName.Role, x))
                .ToList();

            claims.Add(new Claim(CustomClaimName.UserId, user.Id));
            claims.Add(new Claim(CustomClaimName.Name, user.UserName));

            if (roles.Contains(Role.Patient))
            {
                claims.Add(new Claim(CustomClaimName.Id, user.Patient.Id.ToString()));
            }
            else if (roles.Contains(Role.Doctor))
            {
                claims.Add(new Claim(CustomClaimName.Id, user.Doctor.Id.ToString()));
            }



            var expires = DateTime.Now.AddDays(60);
            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NullReferenceException("User not found");
            }

            var patientInfo = await _dbContext.Patients
                .Include(o => o.DiseaseHistory)
                .Include(o => o.Confidants)
                .Include(o => o.ArtificialOrgans)
                .ThenInclude(x => x.ArtificialOrganKeys)
                .FirstOrDefaultAsync(o => o.UserId == userId);

            var doctorInfo = await _dbContext.Doctors
                .Include(o => o.Confidants)
                .FirstOrDefaultAsync(o => o.UserId == userId);

            if (patientInfo != null)
            {
                if (patientInfo.Confidants != null)
                {
                    _dbContext.RemoveRange(patientInfo.Confidants);
                }
                if (patientInfo.ArtificialOrgans != null)
                {
                    patientInfo.ArtificialOrgans.ForEach(x =>
                    {
                        if (x.ArtificialOrganKeys != null)
                        {
                            _dbContext.RemoveRange(x.ArtificialOrganKeys);
                        }

                        x.OrganDatas = _dbContext.OrganDates.Where(o => o.ArtificialOrganId == x.Id).ToList();
                        if (x.OrganDatas != null)
                        {
                            _dbContext.RemoveRange(x.OrganDatas);
                        }

                    });
                    _dbContext.RemoveRange(patientInfo.ArtificialOrgans);
                }
                if (patientInfo.DiseaseHistory != null)
                {
                    _dbContext.Remove(patientInfo.DiseaseHistory);
                }

                _dbContext.Remove(patientInfo);
            }

            if (doctorInfo != null)
            {
                if (doctorInfo.Confidants != null)
                {
                    _dbContext.RemoveRange(doctorInfo.Confidants);
                }

                _dbContext.Remove(doctorInfo);
            }


            var connectHistories = await _dbContext.ConnectHistories.Where(x => x.UserId == userId).ToListAsync();
            if (connectHistories != null)
            {
                _dbContext.RemoveRange(connectHistories);
            }

            await _userManager.DeleteAsync(user);
        }

    }
}
