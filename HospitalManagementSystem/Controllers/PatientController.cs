using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    //[Authorize(Roles = "Patient")]
    //[Route("api/[controller]")]
    //[ApiController]
    //[Authorize(Roles = "Admin,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly HospitalContext _context;
        public PatientController(HospitalContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllPatients")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
          //  return await _context.Patients.ToListAsync();



            var patients = await _context.Patients
                                        .Include(p => p.Appointments)
                                         .ThenInclude(a => a.Doctor)
                                        .Include(p => p.MedicalRecords)
                                        .Select(p => new PatientInfoDTO
                                        {
                                            Id = p.Id,
                                            FirstName = p.FirstName ,
                                            LastName =  p.LastName,
                                            DateOfBirth = p.DateOfBirth,
                                            Gender = p.Gender,
                                            Address = p.Address,
                                            PhoneNumber = p.PhoneNumber,
                                            Appointments = p.Appointments.Select(a => new AppointmentPatientDto
                                            {
                                                Id = a.Id,
                                                Date = a.Date,
                                                TimeSlot = a.TimeSlot,
                                                Details = a.Reason,
                                                DoctorId = a.DoctorId,
                                                DoctorName = a.Doctor.FirstName + " " + a.Doctor.LastName,
                                                DoctorSpeciality = a.Doctor.LastName
                                            }).ToList(),
                                            MedicalRecords = p.MedicalRecords.Select(x => new MedicalRecordDto
                                            {
                                                Id = x.Id,
                                                RecordDate = x.RecordDate,
                                                Description = x.Description,
                                                DoctorId = x.DoctorId,
                                                DoctorName = x.Doctor.FirstName + " " + x.Doctor.LastName,
                                                DoctorSpeciality = x.Doctor.Specialty
                                            }).ToList()
                                        })
                                        .ToListAsync();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientInfoDTO>> GetPatient(int id)
        {
            var patient = await _context.Patients
                                       .Where(p => p.Id == id)
                                       .Select(p => new PatientInfoDTO
                                       {
                                           Id = p.Id,
                                           FirstName = p.FirstName,
                                           LastName = p.LastName,
                                           DateOfBirth = p.DateOfBirth,
                                           Gender = p.Gender,
                                           Address = p.Address,
                                           PhoneNumber = p.PhoneNumber,
                                           Appointments = p.Appointments.Select(a => new AppointmentPatientDto
                                           {
                                               Id = a.Id,
                                               Date = a.Date,
                                               TimeSlot = a.TimeSlot,
                                               Details = a.Reason,
                                               DoctorId = a.DoctorId,
                                               DoctorName = a.Doctor.FirstName + " " + a.Doctor.LastName,
                                               DoctorSpeciality = a.Doctor.Specialty
                                           }).ToList(),
                                           MedicalRecords = p.MedicalRecords.Select(x=> new MedicalRecordDto
                                           { 
                                               Id = x.Id,
                                               RecordDate = x.RecordDate,
                                               Description = x.Description,
                                               DoctorId= x.DoctorId,
                                               DoctorName= x.Doctor.FirstName + " " + x.Doctor.LastName,
                                               DoctorSpeciality = x.Doctor.Specialty
                                           }).ToList()
                                       })
                                       .FirstOrDefaultAsync();




            if (patient == null)
            {
                return NotFound();
            }

            //var patientInfo = new PatientInfoDTO
            //{
            //    Id = patient.Id,
            //    FullName = patient.FirstName,
            //    LastName = patient.LastName,
            //    Gender = patient.Gender,
            //    Appointments = patient.Appointments.Select(a => new AppointmentPatientDto
            //    {
            //        Id = a.Id,
            //        Date = a.Date,
            //        Details = a.Reason,
            //        DoctorId = a.DoctorId,
            //        DoctorName = a.Doctor.FirstName + " " + a.Doctor.LastName,
            //    }).ToList(),
            //    MedicalRecords = patient.MedicalRecords.Select(b => new MedicalRecordDto
            //    {
            //        Id = b.Id,
            //        RecordDate = b.RecordDate,
            //        Description = b.Description,
            //        DoctorId = b.DoctorId,
            //        DoctorName = b.Doctor.FirstName+ " "+ b.Doctor.LastName,
            //    }).ToList()
            //};

            return patient;
        }
        [HttpGet("GetPatientByUsername")]
        public async Task<ActionResult<Patient>> GetPatientByUsername(string username)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(d => d.Username == username);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }


        [HttpPost("RegisterPatient")] 
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        [HttpDelete("DeletePatient/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}