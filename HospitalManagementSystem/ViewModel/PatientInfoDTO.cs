using HospitalManagementSystem.Models;
using System.Collections.Generic;
using System;

namespace HospitalManagementSystem.ViewModel
{
    public class PatientInfoDTO
    {

        public int Id { get; set; }
        public string FullName { get; set; }
        
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        // Relationships
        public ICollection<AppointmentPatientDto> Appointments { get; set; }
        public ICollection<MedicalRecordDto> MedicalRecords { get; set; }
    }
}
