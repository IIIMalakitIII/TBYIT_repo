using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Ave.API.Attributes;
using Ave.API.ViewModels;
using Ave.BLL.Interface;
using Ave.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalInstituationController : BaseApiController
    {
        private readonly IMedicalInstitutionService _institutionService;
        private readonly IMapper _mapper;

        public MedicalInstituationController(IMedicalInstitutionService institutionService, IMapper mapper)
        {
            _institutionService = institutionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalInstitutionViewModel>>> Institution()
        {
            var institutions = await _institutionService.GetAll();
            var institutionsViewModel = _mapper.Map<IEnumerable<MedicalInstitutionViewModel>>(institutions);

            return Ok(institutionsViewModel);
        }

        [HttpGet("autocomplete")]
        public async Task<ActionResult<IEnumerable<SelectViewModel>>> Autocomplete()
        {
            var institutions = await _institutionService.GetAll();
            var institutionsViewModel = _mapper.Map<IEnumerable<SelectViewModel>>(institutions);

            return Ok(institutionsViewModel);
        }

        [HttpGet("institution-by-doctor-id/{doctorId}")]
        public async Task<ActionResult<MedicalInstitutionViewModel>> InstitutionByDoctor(int doctorId)
        {
            var institution = await _institutionService.GetInstitutionByDoctorId(doctorId);
            var institutionViewModel = _mapper.Map<MedicalInstitutionViewModel>(institution);

            return institutionViewModel;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<MedicalInstitutionViewModel>> Institution(int id)
        {
            var institution = await _institutionService.GetById(id);
            var institutionViewModel = _mapper.Map<MedicalInstitutionViewModel>(institution);

            return institutionViewModel;
        }


        [HttpPost]
        [Autosave]
        public async Task<IActionResult> Institution([FromBody] MedicalInstitutionViewModel model)
        {
            var institution = _mapper.Map<MedicalInstitution>(model);
            var newCreatedId = await _institutionService.Create(institution);

            return Ok(new { newCreatedId });
        }

        [HttpPut]
        [Autosave]
        public async Task<IActionResult> UpdateInstitution(MedicalInstitutionViewModel model)
        {
            var institution = _mapper.Map<MedicalInstitution>(model);

            await _institutionService.Update(institution);

            return NoContent();
        }
    }
}
