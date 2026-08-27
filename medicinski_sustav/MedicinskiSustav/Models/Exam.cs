using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinskiSustav.Models
{
    namespace MedicinskiSustav.Models
    {
        public enum TipPregleda
        {
            CT, MR, ULTRA, EKG, ECHO, OKO, DERM, DENTA, MAMMO, EEG
        }

        public class Exam
        {
            public int Id { get; set; }
            public TipPregleda Tip { get; set; }
            public DateTime Termin { get; set; }

            public int PatientId { get; set; }
            public Patient? Patient { get; set; }

            public int DoctorId { get; set; }
            public Doctor? Doctor { get; set; }
        }
    }
}
