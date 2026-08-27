using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinskiSustav.Models.MedicinskiSustav.Models;

namespace MedicinskiSustav.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public string Specijalizacija { get; set; } = "";

        public List<Exam> Pregledi { get; set; } = new();
    }
}
