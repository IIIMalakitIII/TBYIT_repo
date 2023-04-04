using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ave.API.ViewModels
{
    public class DoctorViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public int MedicalInstitutionId { get; set; }

        public bool RecordingAvailable  { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string License { get; set; }

        [Required]
        public string Phone { get; set; }

        public SelectViewModel MedicalInstitution { get; set; }

        public ICollection<ConfidantViewModel> Confidants { get; set; }
    }
}
