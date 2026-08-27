using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinskiSustav.Models
{
    public class MedicalHistory
    {
        public int Id { get; set; }
        public string Bolest { get; set; } = "";
        public DateOnly DatumPocetka { get; set; }
        public DateOnly? DatumZavrsetka { get; set; }  

        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
}
