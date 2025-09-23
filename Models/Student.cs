using System.ComponentModel.DataAnnotations;

namespace GradeALMS.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Student Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        // Foreign key for ApplicationUser
        public string? ApplicationUserId { get; set; }

        // Navigation properties
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
