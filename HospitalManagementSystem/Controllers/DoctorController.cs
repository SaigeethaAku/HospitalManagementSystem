using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{

    //[Authorize(Roles = "Doctor")]
    //[Route("api/[controller]")]
    //[ApiController]

   // [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly HospitalContext _context;

        public DoctorsController(HospitalContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllDoctors")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
        {
            //return await _context.Doctors.Include(d => d.Appointments)
            //                          .Where(d => d.Id == id)
            //                          .ToListAsync();
            var doctors = await _context.Doctors
                                   .Include(d => d.Appointments)
                                   .Select(d => new DoctorDto
                                   {
                                       Id = d.Id,
                                       FirstName = d.FirstName,
                                       LastName = d.LastName,
                                       Specialty = d.Specialty,
                                       Username = d.Username,
                                       Appointments = d.Appointments
                                                       .Select(a => new AppointmentDoctorDto
                                                       {
                                                           Id = a.Id,
                                                           Date = a.Date,
                                                           Details = a.Reason,
                                                           PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                                                       })
                                                       .ToList()
                                   })
                                   .ToListAsync();

            return Ok(doctors);
        }

        [HttpGet("GetDoctor/{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
        {
           // var doctor = await _context.Doctors.FindAsync(id);


            var doctor = await _context.Doctors
                                      .Include(d => d.Appointments)
                                      .Where(d => d.Id == id)
                                      .FirstOrDefaultAsync();

            if (doctor == null)
            {
                return NotFound();
            }
            var doctorDto = new DoctorDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialty = doctor.Specialty,
                Username = doctor.Username,
                Appointments = doctor.Appointments.Select(a => new AppointmentDoctorDto
                {
                    Id = a.Id,
                    Date = a.Date,
                    TimeSlot = a.TimeSlot,
                    Details = a.Reason,
                   // PatientId = a.PatientId,
                    PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                }).ToList()
            };

            return Ok(doctorDto);

           
        }

        [HttpGet("GetDoctorsBySpecialty")]
        public async Task<ActionResult<Doctor>> GetDoctorsBySpecialty(string specialty)
        {
            //if (string.IsNullOrEmpty(specialty))
            //{
            //    return BadRequest("Specialty is required");
            //}

            var doctors = await _context.Doctors
                .Where(d => d.Specialty == specialty)
                .Select(d => new Doctor
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialty = d.Specialty,
                    Username = d.Username,
                })
                .ToListAsync();

            //if (!doctors.Any())
            //{
            //    return NotFound("No doctors found for the given specialty");
            //}

            return Ok(doctors);
        }
        [HttpGet("GetDoctorByUsername")]
        public async Task<ActionResult<DoctorDto>> GetDoctorByUsername(string username)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Username == username);
            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(new DoctorDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Username = doctor.Username,
                Specialty = doctor.Specialty
            });
        }

        [HttpPost("RegisterDoctor")]
       
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDoctor", new { id = doctor.Id }, doctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return BadRequest();
            }

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("DeleteDoctor/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }

}
