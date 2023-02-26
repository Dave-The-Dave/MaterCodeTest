using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MaterCodeTest.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public string PatientURN { get; set; }
        public int StaffId { get; set; }
        public DateTime LoggedDate { get; set; }
        public string LoggedComment { get; set; }
        [ForeignKey("PatientURN")]
        [JsonIgnore]
        public Patient? Patient { get; set; }

        [ForeignKey("StaffId")]
        public Staff? StaffMember { get; set; }

        public Comment()
        {
            LoggedDate= DateTime.Now;
        }


        //public Comment( string patientURN, int staffId, DateTime loggedDate, string loggedComment) : base()
        //{
        //    PatientURN = patientURN;
        //    StaffId = staffId;
        //    LoggedDate = loggedDate;
        //    LoggedComment = loggedComment;
        //}
    }
}
