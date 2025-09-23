using System.ComponentModel.DataAnnotations;

namespace GradeALMS.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        // Navigation properties
        public virtual Student? Student { get; set; }
        public virtual Course? Course { get; set; }
    }
}
