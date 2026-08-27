using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinskiSustav.Models
{
    namespace MedicinskiSustav.Models
    {
        public class Prescription
        {
            public int Id { get; set; }
            public string NazivLijeka { get; set; } = "";
            public string Doza { get; set; } = "";        
            public string Ucestalost { get; set; } = "";  

            public int PatientId { get; set; }
            public Patient? Patient { get; set; }
        }
    }
}
