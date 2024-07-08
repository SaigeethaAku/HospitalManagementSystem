using System;

namespace HospitalManagementSystem.ViewModel
{
    public class AppointmentDoctorDto
    {
        public int Id { get; set; }
        public string Details { get; set; }

        public int PatientId { get; set; }   

        public string PatientName { get; set; }
        public DateTime Date { get; set; }
    }
}
