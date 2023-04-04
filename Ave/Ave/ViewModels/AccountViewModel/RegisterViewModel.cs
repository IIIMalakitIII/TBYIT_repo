using System.ComponentModel.DataAnnotations;

namespace Ave.API.ViewModels.AccountViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Password { get; set; }

        public DoctorViewModel Doctor { get; set; }

        public PatientViewModel Patient { get; set; }
    }
}
