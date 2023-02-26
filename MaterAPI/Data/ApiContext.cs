using Microsoft.EntityFrameworkCore;
using MaterCodeTest.Models;

namespace MaterCodeTest.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options) {
        }

        public DbSet<PatientAdmition> Admitions { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Bed> Beds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          
            //demo data - based off criteria sheet
            //Patients
            modelBuilder.Entity<Patient>().HasData(
                new Patient
                {
                    PatientURN = "0083524",
                    FirstName = "John",
                    LastName = "Doe",
                    DOB = DateOnly.Parse("1980-01-01"),
                    PresentingIssues = "Nausea, Dizziness"
                },
                new Patient
                {
                    PatientURN = "00000001",
                    FirstName = "Lorna",
                    LastName = "Smith",
                    DOB = DateOnly.Parse("1995-03-15"),
                    PresentingIssues = "Broken Leg"
                },
                new Patient
                {
                    PatientURN = "00000002",
                    FirstName = "Diana",
                    LastName = "May",
                    DOB = DateOnly.Parse("1972-11-23"),
                    PresentingIssues = "High Fever"
                },
                new Patient
                {
                    PatientURN = "00000003",
                    FirstName = "John",
                    LastName = "Smith",
                    DOB = DateOnly.Parse("2001-01-01"),
                    PresentingIssues = "Cough, Head Aches"
                }
            );

            //Comments
            modelBuilder.Entity<Comment>().HasData(
                new  { 
                    CommentId = 1,
                    StaffId = 1,
                    PatientURN= "0083524",
                    LoggedDate = DateTime.Parse("2020-02-02 09:50:00"),
                    LoggedComment = "Admitted"
                },
                new
                {
                    CommentId = 2,
                    StaffId = 2,
                    PatientURN = "0083524",
                    LoggedDate = DateTime.Parse("2020-02-02 09:55:00"),
                    LoggedComment = "Temp Checked"
                },
                new 
                {
                    CommentId = 3,
                    StaffId = 2,
                    PatientURN = "0083524",
                    LoggedDate = DateTime.Parse("2020-02-02 10:25:22"),
                    LoggedComment = "Blood pressure checked"
                },
                new 
                {
                    CommentId = 4,
                    StaffId = 2,
                    PatientURN = "00000001",
                    LoggedDate = DateTime.Parse("2020-02-02 07:30:00"),
                    LoggedComment = "Admitted"
                },
                new 
                {
                    CommentId = 5,
                    StaffId = 2,
                    PatientURN = "00000001",
                    LoggedDate = DateTime.Parse("2020-02-02 07:30:25"),
                    LoggedComment = "Xray Waiting Results"
                },
                new 
                {
                    CommentId = 6,
                    StaffId = 1,
                    PatientURN = "00000002",
                    LoggedDate = DateTime.Parse("2020-02-02 07:27:25"),
                    LoggedComment = "Admitted"
                },
                new 
                {
                    CommentId = 7,
                    StaffId = 1,
                    PatientURN = "00000002",
                    LoggedDate = DateTime.Parse("2020-02-02 09:45:25"),
                    LoggedComment = "Medication Supplied"
                },
                new 
                {
                    CommentId = 8,
                    StaffId = 1,
                    PatientURN = "00000003",
                    LoggedDate = DateTime.Parse("2022-02-22 09:45:25"),
                    LoggedComment = "Admitted"
                }
                );

            //Staff
            modelBuilder.Entity<Staff>().HasData(
                new Staff {
                    StaffId = 1,
                    Name = "Kelly A.",
                    Role = Roles.Nurse,
                    DateOfHire = DateTime.Parse("2002-02-02")
                },
                new Staff {
                    StaffId = 2,
                    Name = "Mary P.", 
                    Role = Roles.Nurse, 
                    DateOfHire = DateTime.Parse("2003-03-03") 
                }
            );

            //Beds
            modelBuilder.Entity<Bed>().HasData(
                new Bed
                {
                    BedId = 1,
                    PatientURN= "0083524",
                    Vaccancy = Vaccancy.InUse,
                    AdmitionId = 1
                },
                new Bed { BedId = 2},
                new Bed { BedId = 3},
                new Bed { BedId = 4 },
                new Bed 
                {
                    BedId = 5,
                    PatientURN = "00000001",
                    Vaccancy = Vaccancy.InUse,
                    AdmitionId = 2
                },
                new Bed
                {
                    BedId = 6,
                    PatientURN = "00000002",
                    Vaccancy = Vaccancy.InUse,
                    AdmitionId = 3
                },
                new Bed { BedId = 7 },
                new Bed { BedId = 8 }
                );

            //Admitions
            modelBuilder.Entity<PatientAdmition>().HasData(
                new PatientAdmition
                {
                    AdmitionId = 1,
                    BedId = 1,
                    PatientURN = "0083524",
                    PatientName = "John Doe",
                    Date = DateTime.Parse("2020-02-02 09:50:00"),
                    Status = Status.Admitted

                },
                new PatientAdmition
                {
                    AdmitionId = 2,
                    BedId = 5,
                    PatientURN = "00000001",
                    PatientName = "Lorna Smith",
                    Date = DateTime.Parse("2020-02-02 07:30:00"),
                    Status = Status.Admitted

                },
                new PatientAdmition
                {
                    AdmitionId = 3,
                    BedId = 6,
                    PatientURN = "00000003",
                    PatientName = "Diana May",
                    Date = DateTime.Parse("2020-02-02 07:30:00"),
                    Status = Status.Admitted

                }
            );

            base.OnModelCreating( modelBuilder );
        }
    }
}
