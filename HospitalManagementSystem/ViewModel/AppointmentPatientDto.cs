﻿using System;

namespace HospitalManagementSystem.ViewModel
{
    public class AppointmentPatientDto
    {
        public int Id { get; set; }
        public string Details { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; }
        public string DoctorSpeciality { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; }

    }
}
