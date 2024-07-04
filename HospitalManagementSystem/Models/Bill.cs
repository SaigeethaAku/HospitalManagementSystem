using System;

namespace HospitalManagementSystem.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillDate { get; set; }
        public string Status { get; set; } // Paid, Unpaid
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}
