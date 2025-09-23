using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GradeALMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Registration Date")]
        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public override string? PhoneNumber { get; set; }

        // Navigation properties for relationships
        public virtual Student? Student { get; set; }
        public virtual Instructor? Instructor { get; set; }
    }
}
