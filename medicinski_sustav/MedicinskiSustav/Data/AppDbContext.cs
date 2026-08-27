using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicinskiSustav.Models;
using MedicinskiSustav.Models.MedicinskiSustav.Models;

namespace MedicinskiSustav.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<MedicalHistory> MedicalHistories => Set<MedicalHistory>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<Exam> Exams => Set<Exam>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=medicinski_sustav;Username=pppk;Password=pppk123"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.Oib)
                .IsUnique();
        }
    }
}