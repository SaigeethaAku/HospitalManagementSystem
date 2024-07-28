using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Numerics;

namespace HospitalManagementSystem.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Bill> Bills { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between Patient and Appointment
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId);

            // Configure one-to-many relationship between Doctor and Appointment
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId);

            // Configure one-to-many relationship between patient and Medical Records
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.MedicalRecords)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId);

            // Configure one-to-many relationship between Doctor and Medical Records
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.MedicalRecords)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId);

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");
                // or
                // entity.Property(e => e.Amount).HasPrecision(18, 4);
            });
            modelBuilder.Entity<Doctor>()
            .HasIndex(d => d.Username)
            .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithOne()
                .HasForeignKey<Doctor>(d => d.Username)
                .HasPrincipalKey<User>(u => u.Username);


            modelBuilder.Entity<Doctor>()
            .HasKey(d => d.Id);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId);

            // Configure Patient entity

            modelBuilder.Entity<Patient>()
           .HasIndex(p => p.Username)
           .IsUnique();

            modelBuilder.Entity<Patient>()
              .HasOne(p => p.User)
              .WithOne()
              .HasForeignKey<Patient>(p => p.Username)
              .HasPrincipalKey<User>(u => u.Username);



            modelBuilder.Entity<Patient>()
            .HasKey(p => p.Id);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId);

            // Configure Appointment entity
            modelBuilder.Entity<Appointment>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);

        }
    }
    }
