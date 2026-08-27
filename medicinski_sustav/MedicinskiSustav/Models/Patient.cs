using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinskiSustav.Models.MedicinskiSustav.Models;

namespace MedicinskiSustav.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public string Oib { get; set; } = "";
        public DateOnly DatumRodjenja { get; set; }
        public string Spol { get; set; } = "";
        public string AdresaBoravista { get; set; } = "";
        public string AdresaPrebivalista { get; set; } = "";

        public List<MedicalHistory> PovijestBolesti { get; set; } = new();
        public List<Prescription> Lijekovi { get; set; } = new();
        public List<Exam> Pregledi { get; set; } = new();
    }
}
