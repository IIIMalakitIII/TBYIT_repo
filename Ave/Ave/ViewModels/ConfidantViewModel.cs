namespace Ave.API.ViewModels
{
    public class ConfidantViewModel
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public SelectViewModel Patient { get; set; }

        public SelectViewModel Doctor { get; set; }

    }
}
