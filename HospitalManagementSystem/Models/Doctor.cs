using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Specialty { get; set; }
        [Required]
        public string Username { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public User User { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
