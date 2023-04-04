namespace Ave.API.ViewModels.AccountViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public PatientViewModel Patient { get; set; }

        public DoctorViewModel Doctor { get; set; }
    }
}
