using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace MaterCodeTest.Models
{
    [Table("Patients")]
    public class Patient
    {
        [Key]
        //public int Id { get; set; }
        [Required]
        public string PatientURN { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? DOB { get; set; }
        public string PresentingIssues { get; set; }
        public List<Comment>? Comments { get; set; }

    }

}
