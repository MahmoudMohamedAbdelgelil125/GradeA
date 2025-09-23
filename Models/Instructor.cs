using System.ComponentModel.DataAnnotations;

namespace GradeALMS.Models
{
    public class Instructor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Instructor Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        // Foreign key for ApplicationUser
        public string? ApplicationUserId { get; set; }

        // Navigation properties
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
