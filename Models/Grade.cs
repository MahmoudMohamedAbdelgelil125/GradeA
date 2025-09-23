using System.ComponentModel.DataAnnotations;

namespace GradeALMS.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Range(0, 100)]
        [Display(Name = "Score (%)")]
        public double Score { get; set; }

        [StringLength(2)]
        [Display(Name = "Grade Letter")]
        public string GradeLetter { get; set; } = string.Empty;

        [Display(Name = "Grade Date")]
        [DataType(DataType.Date)]
        public DateTime GradeDate { get; set; }

        // Navigation properties
        public virtual Student? Student { get; set; }
        public virtual Course? Course { get; set; }

        // Method to calculate grade letter based on score
        public void CalculateGradeLetter()
        {
            if (Score >= 97) GradeLetter = "A+";
            else if (Score >= 93) GradeLetter = "A";
            else if (Score >= 90) GradeLetter = "A-";
            else if (Score >= 87) GradeLetter = "B+";
            else if (Score >= 83) GradeLetter = "B";
            else if (Score >= 80) GradeLetter = "B-";
            else if (Score >= 77) GradeLetter = "C+";
            else if (Score >= 73) GradeLetter = "C";
            else if (Score >= 70) GradeLetter = "C-";
            else if (Score >= 67) GradeLetter = "D+";
            else if (Score >= 65) GradeLetter = "D";
            else GradeLetter = "F";
        }
    }
}
