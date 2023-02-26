using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterCodeTest.Models
{
    [Table("MaterStaff")]
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }
        public string Name { get; set; }
        [Required]
        public Roles Role { get; set; }

        public DateTime DateOfHire { get; set; }

        //public string getStaffName()
        //{
        //    return FirstName + " " + LastName;
        //}
    }

    public enum Roles
    {
        Nurse,
        Doctor,
        Admin,
        Other
    }
}
