using AutoMapper;
using Ave.API.ViewModels;
using Ave.BLL.Interface;
using Ave.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ave.API.Attributes;

namespace Ave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Autosave]
    public class ArtificialOrganController : BaseApiController
    {
        private readonly IArtificialOrganService _artificialOrganService;
        private readonly IMapper _mapper;

        public ArtificialOrganController(IArtificialOrganService artificialOrganService, IMapper mapper)
        {
            _artificialOrganService = artificialOrganService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ArtificialOrganViewModel>> Organ(int id)
        {
            return Ok(_mapper.Map<ArtificialOrganViewModel>(await _artificialOrganService.GetOrganById(id, CurrentUser.UserId, CurrentUser.Role)));
        }

        [HttpGet("patient-organ/{patientId}")]
        public async Task<ActionResult<List<ArtificialOrganViewModel>>> PatientOrgan(int patientId)
        {
            return Ok(_mapper.Map<List<ArtificialOrganViewModel>>(await _artificialOrganService.GetPatientOrgansById(patientId, CurrentUser.Id, CurrentUser.Role, CurrentUser.UserId)));
        }

        [HttpGet("diagnostic-organ/{id}/{from}/{to}")]
        public async Task<ActionResult<string>> DiagnosticOrgan(int id, DateTime from, DateTime to)
        {
            return Ok(await _artificialOrganService.DiagnosticArtificialOrgan(id, from, to));
        }

        [HttpPost]
        public async Task<ActionResult<ArtificialOrganViewModel>> Organ([FromBody] ArtificialOrganViewModel model)
        {
            var organ = _mapper.Map<ArtificialOrgan>(model);
            return Ok(_mapper.Map<ArtificialOrganViewModel>(await _artificialOrganService.CreateArtificialOrgan(organ)));
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateOrgan([FromBody] ArtificialOrganViewModel model)
        {
            var organ = _mapper.Map<ArtificialOrgan>(model);
            await _artificialOrganService.UpdateOrgan(organ);
            return Ok();
        }

        [HttpPost("organ-data")]
        public async Task<ActionResult> OrganData([FromForm] OrganDataViewModel model, Guid key)
        {
            var organData = _mapper.Map<OrganData>(model);
            await _artificialOrganService.AddOrganData(organData, key);
            return Ok();
        }

        [HttpPost("generate-organ-key/{id}")]
        public async Task<ActionResult> GenerateOrganKey(int id)
        {
            await _artificialOrganService.GenerateOrganKey(id, CurrentUser.UserId, CurrentUser.Role);
            return Ok();
        }

        [HttpGet("organ-key")]
        public async Task<ActionResult<List<ArtificialOrganKeyViewModel>>> GetOrganId(int id)
        {
            return Ok(_mapper.Map<List<ArtificialOrganKeyViewModel>>(await _artificialOrganService.GetOrganKeys(id)));
        }

        [HttpPost("confidant/{doctorId}")]
        public async Task<ActionResult> CreateConfidant(int doctorId)
        {
            await _artificialOrganService.CreateConfidant(CurrentUser.UserId, doctorId);

            return Ok();
        }

        [HttpGet("confidant-info")]
        public async Task<ActionResult<List<ConfidantViewModel>>> GetConfidants()
        {
            var result = await _artificialOrganService.GetConfidants(CurrentUser.UserId);
            var retResult = _mapper.Map<List<ConfidantViewModel>>(result);
            return Ok(retResult);
        }

        [HttpDelete("delete-confidant/{confidantId}")]
        public async Task<ActionResult> GetConfidants(int confidantId)
        {
            await _artificialOrganService.DeleteConfidant(CurrentUser.UserId, confidantId);

            return Ok();
        }

        [HttpGet("organ-data")]
        public async Task<ActionResult<List<OrganDataViewModel>>> GetOrganData(int organId)
        {
            return Ok(_mapper.Map<List<OrganDataViewModel>>(await _artificialOrganService.GetOrganData(organId, CurrentUser.UserId, CurrentUser.Role)));
        }

        [HttpGet("connect-history/{organId}")]
        public async Task<ActionResult<List<ConnectHistoryViewModel>>> GetOrganHistory(int organId)
        {
            return Ok(_mapper.Map<List<ConnectHistoryViewModel>>(await _artificialOrganService.GetConnectHistory(organId, CurrentUser.UserId)));
        }
    }
}
