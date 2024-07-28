using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    //[Authorize(Roles = "Admin,Doctor,Patient")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly HospitalContext _context;

        public AppointmentsController(HospitalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments.Include(a => a.Patient).Include(a => a.Doctor).ToListAsync();
        }


        [HttpGet("GetAppointmentsByDoctor/{id}")]
    public async Task<ActionResult<IEnumerable<AppointmentDoctorDto>>> GetAppointmentsByDoctor(int id)
    {
            //return await _context.Appointments.Include(a => a.Patient).Include(a => a.Doctor).Where(d => d.DoctorId == id).ToListAsync();


            var appointments = await _context.Appointments
                                         .Where(a => a.DoctorId == id)
                                         .Include(a => a.Patient)
                                         .Select(a => new AppointmentDoctorDto
                                         {
                                             Id = a.Id,
                                             Date = a.Date,
                                             TimeSlot = a.TimeSlot,
                                             PatientId = a.PatientId,
                                             PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                                             Details = a.Reason
                                         })
                                         .ToListAsync();
            return appointments;
        }
        [HttpGet("GetAppointmentsByPatient/{id}")]
        public async Task<ActionResult<IEnumerable<AppointmentPatientDto>>> GetAppointmentsByPatient(int id)
        {
            //return await _context.Appointments.Include(a => a.Patient).Include(a => a.Doctor).Where(d => d.DoctorId == id).ToListAsync();


            var appointments = await _context.Appointments
                                         .Where(a => a.PatientId == id)
                                         .Include(a => a.Doctor)
                                         .Select(a => new AppointmentPatientDto
                                         {
                                             Id = a.Id,
                                             Date = a.Date,
                                             TimeSlot = a.TimeSlot,
                                             DoctorId = a.DoctorId,
                                             DoctorName = a.Doctor.FirstName + " " + a.Doctor.LastName,
                                             Details = a.Reason
                                         })
                                         .ToListAsync();
            return appointments;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments.Include(a => a.Patient).Include(a => a.Doctor).FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }
        [HttpGet("GetAppointmentDetails/{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentDetails(int id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        [HttpPost("BookAppointment")]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        [HttpDelete("DeleteAppointment/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("GetAppointmentsByDoctorAndDate/{id}/{date}")]
        public async Task<ActionResult<IEnumerable<AppointmentDoctorDto>>> GetAppointmentsByDoctorAndDate(int id, DateTime date)
        {
            var appointments = await _context.Appointments
                                             .Where(a => a.DoctorId == id && a.Date.Date == date.Date)
                                             .Include(a => a.Patient)
                                             .Select(a => new AppointmentDoctorDto
                                             {
                                                 Id = a.Id,
                                                 Date = a.Date,
                                                 TimeSlot = a.TimeSlot,
                                                 PatientId = a.PatientId,
                                                 PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                                                 Details = a.Reason
                                             })
                                             .ToListAsync();
            return appointments;
        }
        [HttpGet("GetAvailableTimeSlots")]
        public async Task<ActionResult<List<string>>> GetAvailableTimeSlots(int doctorId, DateTime date)
        {
            // Fetch booked time slots for the given doctor and date
            var bookedSlots = await _context.Appointments
                                            .Where(a => a.DoctorId == doctorId && a.Date.Date == date.Date)
                                            .Select(a => a.TimeSlot)
                                            .ToListAsync();

            // All possible time slots from 10:00 AM to 4:00 PM with 30-minute intervals
            var allSlots = new List<string>
        {
            "10:00 AM", "10:30 AM", "11:00 AM", "11:30 AM",
            "12:00 PM", "12:30 PM", "01:00 PM", "01:30 PM",
            "02:00 PM", "02:30 PM", "03:00 PM", "03:30 PM",
            "04:00 PM"
        };

            // Get available slots by excluding booked slots
            var availableSlots = allSlots.Except(bookedSlots).ToList();

            return Ok(availableSlots);
        }
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}

