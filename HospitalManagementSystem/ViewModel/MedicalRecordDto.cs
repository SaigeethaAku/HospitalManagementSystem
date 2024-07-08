using HospitalManagementSystem.Models;
using System;

namespace HospitalManagementSystem.ViewModel
{
    public class MedicalRecordDto
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public string Description { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
    }
}

