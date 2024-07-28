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
   // [Authorize(Roles = "Admin,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly HospitalContext _context;

        public MedicalRecordsController(HospitalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetMedicalRecords()
        {
            return await _context.MedicalRecords.Include(m => m.Patient).Include(m => m.Doctor).ToListAsync();
        }


        [HttpGet("GetMedicalRecordsByPatient/{id}")]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetMedicalRecordsByPatient(int id)
        {
            // return await _context.MedicalRecords.Include(m => m.Patient).Include(m => m.Doctor).ToListAsync();
            var medicalRecords = await _context.MedicalRecords
                                         .Where(a => a.PatientId == id)
                                         .Include(a => a.Doctor)
                                         .Select(a => new MedicalRecordDto
                                         {
                                             Id = a.Id,
                                             RecordDate = a.RecordDate,
                                             DoctorId = a.DoctorId,
                                             DoctorName = a.Doctor.FirstName + " " + a.Doctor.LastName,
                                             Description = a.Description
                                         })
                                         .ToListAsync();
            return medicalRecords;
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecord(int id)
        {
            var medicalRecord = await _context.MedicalRecords.Include(m => m.Patient).Include(m => m.Doctor).FirstOrDefaultAsync(m => m.Id == id);

            if (medicalRecord == null)
            {
                return NotFound();
            }

            return medicalRecord;
        }

        [HttpPost]
        public async Task<ActionResult<MedicalRecord>> PostMedicalRecord(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicalRecord", new { id = medicalRecord.Id }, medicalRecord);
        }

        [HttpPost("AddMedicalRecord/{id}")]
        public async Task<IActionResult> PostMedicalRecord(int id, MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicalRecord", new { id = medicalRecord.Id }, medicalRecord);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicalRecordExists(int id)
        {
            return _context.MedicalRecords.Any(e => e.Id == id);
        }
    }
}
