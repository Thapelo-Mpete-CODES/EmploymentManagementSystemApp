using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }

        public bool IsActive { get; set; }

        // Foreign Key
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        // Navigation Property
        public Department Department { get; set; }
    }
}
