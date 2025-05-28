using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctor _doctorService;

        public DoctorController(IDoctor doctorService)
        {
            _doctorService = doctorService;
        }

        // POST: api/Doctor
        [HttpPost]
        public async Task<ActionResult<Doctor>> AddDoctor([FromBody] DoctorAddRequestDto doctorDto)
        {
            if (doctorDto == null)
                return BadRequest("Invalid doctor data.");

            try
            {
                var addedDoctor = await _doctorService.AddDoctor(doctorDto);
                return CreatedAtAction(nameof(GetDoctorByName), new { name = addedDoctor.Name }, addedDoctor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Doctor/by-name/{name}
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<Doctor>> GetDoctorByName(string name)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByName(name);
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/Doctor/by-speciality/{speciality}
        [HttpGet("by-speciality/{speciality}")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsBySpeciality(string speciality)
        {
            try
            {
                var doctors = await _doctorService.GetDoctorsBySpeciality(speciality);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
