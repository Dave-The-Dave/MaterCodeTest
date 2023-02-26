using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MaterCodeTest.Models
{
    [Table("Beds")]
    public class Bed
    {
        [Key]
        public int BedId { get; set; } //same as bed number
        public Vaccancy Vaccancy { get; set; }
        public int? AdmitionId { get; set; }
        public string? PatientURN { get; set; }

        [ForeignKey("PatientURN")]
        public Patient? Patient { get; set; }

        [ForeignKey("AdmitionId")]
        public PatientAdmition? Admition { get; set; }
    }

    public enum Vaccancy
    {
        Free,
        InUse
    }
}
