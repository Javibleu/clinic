using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Appointment
    {
        public  int Id  { get; set; }
        public int Type { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int ClinicId { get; set; }
        public DateTime AppDate { get; set; }
        public int StartSlot { get; set; }
        public int EndSlot { get; set; }
        public string Status { get; set; }
        public string BillStatus { get; set; }
        public int DestClinicId { get; set; }
        public int DestPharmaId { get; set; }
        public ICollection<Diagnostics> Diagnostics { get; set; }
        public ICollection<Symptom> Symptoms { get; set; }
        public ICollection<Medication> Medicines { get; set; }
        public ICollection<Orthotic> Orthotics { get; set; }

    }
}
