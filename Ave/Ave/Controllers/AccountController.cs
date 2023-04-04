using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Ave.API.ViewModels;
using Ave.API.ViewModels.AccountViewModel;
using Ave.BLL.Interface;
using Ave.DAL.Entities;
using Ave.Common.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Ave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IExportDatabaseToExcel _exportDatabaseToExcel;

        public AccountController(IAccountService accountService, IExportDatabaseToExcel exportDatabaseToExcel, IMapper mapper)
        {
            _accountService = accountService;
            _exportDatabaseToExcel = exportDatabaseToExcel;
            _mapper = mapper;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            var token = await _accountService.SignIn(model.Email, model.Password);

            return Ok(new { token });
        }

        [HttpGet("export-data")]
        public ActionResult<string> Get()
        {
            return Ok(_exportDatabaseToExcel.ExportDbToExcel());
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            var userId = string.Empty;

            switch (model.Role)
            {
                case Role.Doctor:
                    var doctor = _mapper.Map<Doctor>(model.Doctor);
                    userId = await _accountService.SignUpDoctor(model.UserName, model.Email, model.Password, model.Role, doctor);
                    break;
                case Role.Patient:
                    var patient = _mapper.Map<Patient>(model.Patient);
                    userId = await _accountService.SignUpPatient(model.UserName, model.Email, model.Password, model.Role, patient);
                    break;
            }


            return Ok(new { userId });
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            await _accountService.ChangePassword(CurrentUser.UserId, model.CurrentPassword, model.NewPassword);

            return NoContent();
        }

        [HttpGet("account-info")]
        public async Task<ActionResult<UserViewModel>> AccountInfo()
        {
            var userInfo = await _accountService.GetAccountInfo(CurrentUser.UserId);

            return _mapper.Map<UserViewModel>(userInfo);
        }

        [HttpGet("allUsers")]
        public async Task<ActionResult<List<UserViewModel>>> UserInfos()
        {
            var userInfo = await _accountService.GetAccountInfos();

            return _mapper.Map<List<UserViewModel>>(userInfo);
        }

        [HttpDelete("deleteUser")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            await _accountService.DeleteUser(userId);
            return NoContent();
        }

        [HttpPut("account-info")]
        public async Task<ActionResult<UserViewModel>> AccountInfo([FromBody] UpdateAccountInfo accountInfo)
        {
            await _accountService.UpdateAccountInfo(CurrentUser.UserId, accountInfo.UserName, accountInfo.Email);

            return NoContent();
        }
    }
}
