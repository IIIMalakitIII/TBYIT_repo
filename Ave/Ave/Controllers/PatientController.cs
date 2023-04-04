using System;
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
    public class PatientController : BaseApiController
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        public PatientController(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientViewModel>>> Patient()
        {
            var patients = await _patientService.GetAll();
            var patientsViewModel = _mapper.Map<IEnumerable<PatientViewModel>>(patients);

            return Ok(patientsViewModel);
        }

        [HttpGet("patinet-by-filter/{filter}")]
        public async Task<ActionResult<IEnumerable<PatientViewModel>>> PatientByFilter(string filter)
        {
            var result = await this._patientService.GetPatientByFilter(filter);

            return Ok(_mapper.Map<List<PatientViewModel>>(result));
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<PatientViewModel>> Patient(int id)
        {
            var patient = await _patientService.GetById(id);
            var patientViewModel = _mapper.Map<PatientViewModel>(patient);
            return patientViewModel;
        }


        [HttpPut("patient-info")]
        public async Task<IActionResult> UpdatePatient(PatientViewModel model)
        {
            var patient = _mapper.Map<Patient>(model);

            await _patientService.Update(patient);

            return NoContent();
        }
    }
}
