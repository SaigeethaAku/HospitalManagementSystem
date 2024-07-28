using System.Collections.Generic;

namespace HospitalManagementSystem.ViewModel
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public string Username { get; set; }
        public ICollection<AppointmentDoctorDto> Appointments { get; set; }
    }
}
