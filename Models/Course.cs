using System.ComponentModel.DataAnnotations;

namespace GradeALMS.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(1, 6)]
        public int Credits { get; set; }

        [Display(Name = "Instructor")]
        public int InstructorId { get; set; }

        // Navigation properties
        public virtual Instructor? Instructor { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
