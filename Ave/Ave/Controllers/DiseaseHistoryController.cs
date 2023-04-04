﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Ave.API.Attributes;
using Ave.API.ViewModels;
using Ave.BLL.Interface;
using Ave.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ave.API.Controllers
{
    public class DiseaseHistoryController : BaseApiController
    {
        private readonly IDiseaseHistoryService _diseaseHistoryService;
        private readonly IMapper _mapper;

        public DiseaseHistoryController(IDiseaseHistoryService diseaseHistoryService, IMapper mapper)
        {
            _diseaseHistoryService = diseaseHistoryService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiseaseHistoryViewModel>>> DiseaseHistory()
        {
            var manufacturers = await _diseaseHistoryService.GetAll();
            var manufacturersViewModel = _mapper.Map<IEnumerable<DiseaseHistoryViewModel>>(manufacturers);

            return Ok(manufacturersViewModel);
        }

        [HttpGet("disease-history-by-patient-id/{id}")]
        public async Task<ActionResult<DiseaseHistoryViewModel>> DiseaseHistoryByPatient(int id)
        {
            var manufacturer = await _diseaseHistoryService.GetDiseaseHistoryByPatientId(id);
            var diseaseHistoryViewModel = _mapper.Map<DiseaseHistoryViewModel>(manufacturer);

            return Ok(diseaseHistoryViewModel);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<DiseaseHistoryViewModel>> DiseaseHistory(int id)
        {
            var manufacturer = await _diseaseHistoryService.GetById(id);
            var diseaseHistoryViewModel = _mapper.Map<DiseaseHistoryViewModel>(manufacturer);

            return diseaseHistoryViewModel;
        }


        [HttpPost]
        [Autosave]
        public async Task<IActionResult> DiseaseHistory([FromBody] DiseaseHistoryViewModel model)
        {
            var manufacturer = _mapper.Map<DiseaseHistory>(model);
            var newCreatedId = await _diseaseHistoryService.Create(manufacturer);

            return Ok(new { newCreatedId });
        }

        [HttpPut]
        [Autosave]
        public async Task<IActionResult> UpdateDiseaseHistory([FromBody] DiseaseHistoryViewModel model)
        {
            var manufacturer = _mapper.Map<DiseaseHistory>(model);

            await _diseaseHistoryService.Update(manufacturer);

            return NoContent();
        }
    }
}
