using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Models
{
    public class Department
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]

        public bool IsActive { get; set; } = true;
        public string Name { get; set; }

        // Navigation Property: One department has many employees
        public ICollection<Employee> Employees { get; set; }
    }

}
