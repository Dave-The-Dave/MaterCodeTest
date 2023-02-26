using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MaterCodeTest.Models
{
    [Table("PatientAdmitions")]
    public class PatientAdmition
    {
        [Key]
        public int AdmitionId { get; set; }
        [Required]
        public int BedId { get; set; }
        [Required]
        public string PatientURN { get; set; }
        public string? PatientName { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public PatientAdmition()
        {
            Status = Status.Admitted;
            Date = DateTime.Now;
        }
    }

    public enum Status
    {
        Admitted,
        Discharged
    }

}
