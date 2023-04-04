using AutoMapper;
using Ave.API.ViewModels;
using Ave.API.ViewModels.AccountViewModel;
using Ave.DAL.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ave.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapViewModels();
        }

        private void MapViewModels()
        {
            CreateMap<DoctorViewModel, Doctor>().ReverseMap();

            CreateMap<ConfidantViewModel, Confidant>().ReverseMap();

            CreateMap<DiseaseHistoryViewModel, DiseaseHistory>().ReverseMap();

            CreateMap<MedicalInstitutionViewModel, MedicalInstitution>().ReverseMap();

            CreateMap<PatientViewModel, Patient>().ReverseMap();

            CreateMap<ArtificialOrgan, ArtificialOrganViewModel>().ReverseMap();

            CreateMap<ArtificialOrganKey, ArtificialOrganKeyViewModel>().ReverseMap();

            CreateMap<OrganDataViewModel, OrganData>()
                .ForMember(d => d.ExtendedData,
                    m => m.MapFrom(s => JObject.Parse(s.ExtendedData)));

            CreateMap<OrganData, OrganDataViewModel>();

            CreateMap<UserViewModel, User>().ReverseMap();

            CreateMap<RecordViewModel, Record>();

            CreateMap<Record, RecordViewModel>()
                .ForMember(d => d.MedicalInstitution,
                    m => m.MapFrom(s =>
                        $"{s.Doctor.MedicalInstitution.Name}, {s.Doctor.MedicalInstitution.Country}, {s.Doctor.MedicalInstitution.City}, {s.Doctor.MedicalInstitution.Address}"));

            CreateMap<MedicalInstitution, SelectViewModel>()
                .ForMember(d => d.Name,
                    m => m.MapFrom(s => $"{s.Name}, {s.Country}, {s.City}, {s.Address}"));

            CreateMap<DiseaseHistory, SelectViewModel>()
                .ForMember(d => d.Name,
                    m => m.MapFrom(s => s.Description));

            CreateMap<Doctor, SelectViewModel>()
                .ForMember(d => d.Name,
                    m => m.MapFrom(s => $"{s.FirstName} {s.LastName}"));

            CreateMap<Patient, SelectViewModel>()
                .ForMember(d => d.Name,
                    m => m.MapFrom(s => $"{s.FirstName} {s.LastName}"));

            CreateMap<ConnectHistory, ConnectHistoryViewModel>()
                .ReverseMap();
        }
    }
}
