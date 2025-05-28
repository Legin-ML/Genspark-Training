using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Contexts
{

    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<DoctorSpeciality> DoctorSpecialities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().HasKey(ap => ap.AppointmentNumber).HasName("PK_Appointments");

            modelBuilder.Entity<Appointment>().HasOne(ap => ap.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(ap => ap.PatientId)
                .HasConstraintName("FK_Appointments_Patient")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>().HasOne(ap => ap.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(app => app.DoctorId)
                .HasConstraintName("FK_Appoinment_Doctor")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DoctorSpeciality>().HasKey(ds => ds.SerialNumber);

            modelBuilder.Entity<DoctorSpeciality>().HasOne(ds => ds.Doctor)
                .WithMany(d => d.DoctorSpecialities)
                .HasForeignKey(ds => ds.DoctorId)
                .HasConstraintName("FK_Speciality_Doctor")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DoctorSpeciality>().HasOne(ds => ds.Speciality)
                .WithMany(s => s.DoctorSpecialities)
                .HasForeignKey(ds => ds.SpecialityId)
                .HasConstraintName("FK_Speciality_Spec")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
