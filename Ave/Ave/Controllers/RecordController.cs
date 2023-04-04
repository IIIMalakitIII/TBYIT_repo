using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Ave.API.Attributes;
using Ave.API.ViewModels;
using Ave.BLL.Interface;
using Ave.DAL.Entities;
using Ave.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Ave.API.Controllers
{
    public class RecordController : BaseApiController
    {
        private readonly IRecordService _recordService;
        private readonly IMapper _mapper;

        public RecordController(IRecordService recordService, IMapper mapper)
        {
            _recordService = recordService;
            _mapper = mapper;
        }

        [HttpGet("patient-records")]
        public async Task<ActionResult<IEnumerable<RecordViewModel>>> PatientRecords()
        {
            var records = await _recordService.GetPatientRecordsAsync(CurrentUser.UserId);
            var recordsViewModel = _mapper.Map<IEnumerable<RecordViewModel>>(records);
            return Ok(recordsViewModel);
        }

        [HttpGet("get-record-by-id/{id}")]
        public async Task<ActionResult<RecordViewModel>> Record(int id)
        {
            var record = await _recordService.GetRecordByIdAsync(id);
            var recordViewModel = _mapper.Map<RecordViewModel>(record);
            return Ok(recordViewModel);
        }

        [HttpGet("patient-records-by-id/{id}")]
        public async Task<ActionResult<IEnumerable<RecordViewModel>>> PatientRecords(int id)
        {
            var records = await _recordService.GetPatientRecordsAsync(id);
            var recordsViewModel = _mapper.Map<IEnumerable<RecordViewModel>>(records);
            return Ok(recordsViewModel);
        }

        [HttpGet("doctor-records")]
        public async Task<ActionResult<IEnumerable<RecordViewModel>>> DoctorRecords()
        {
            var records = await _recordService.GetDoctorRecordsAsync(CurrentUser.UserId);
            var recordsViewModel = _mapper.Map<IEnumerable<RecordViewModel>>(records);
            return Ok(recordsViewModel);
        }

        [HttpPost]
        [Autosave]
        public async Task<IActionResult> Record([FromBody] RecordViewModel model)
        {
            var record = _mapper.Map<Record>(model);
            var newCreatedRecord = await _recordService.CreateRecordAsync(record, CurrentUser.UserId);

            return Ok(new { newCreatedRecord.Id });
        }

        [HttpPut("update-status-like-doctor")]
        [Autosave]
        public async Task<IActionResult> RecordStatusLikeDoctor(RecordStatus status, int recordId)
        {
            await _recordService.UpdateRecordStatusLikeDoctor(status, recordId, CurrentUser.UserId);
            return NoContent();
        }

        [HttpPut("update-status-like-patient")]
        [Autosave]
        public async Task<IActionResult> RecordStatusLikePatient(RecordStatus status, int recordId)
        {
            await _recordService.UpdateRecordStatusLikePatient(status, recordId, CurrentUser.UserId);
            return NoContent();
        }

        [HttpPut("update-record")]
        [Autosave]
        public async Task<IActionResult> Update([FromBody] RecordViewModel model)
        {
            var record = _mapper.Map<Record>(model);
            var newRecord = await _recordService.UpdateRecord(record, CurrentUser.UserId);

            return Ok(new { newRecord.Id });
        }
    }
}
