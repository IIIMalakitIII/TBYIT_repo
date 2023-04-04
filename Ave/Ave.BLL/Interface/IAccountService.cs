using System.Collections.Generic;
using System.Threading.Tasks;
using Ave.DAL.Entities;

namespace Ave.BLL.Interface
{
    public interface IAccountService
    {
        Task<string> SignIn(string email, string password);

        Task<string> SignUpPatient(string userName, string email, string password, string role, Patient model);

        Task<string> SignUpDoctor(string userName, string email, string password, string role, Doctor model);

        Task ChangePassword(string id, string currentPassword, string newPassword);

        Task<User> GetAccountInfo(string id);

        Task UpdateAccountInfo(string id, string newUserName, string newEmail);

        Task<List<User>> GetAccountInfos();

        Task DeleteUser(string userId);
    }
}
