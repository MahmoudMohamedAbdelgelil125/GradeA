using GradeALMS.Data;
using GradeALMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradeALMS.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetStudents()
        {
            var students = await _context.Students
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Email,
                    s.EnrollmentDate,
                    CoursesCount = s.StudentCourses.Count(),
                    AverageGrade = s.Grades.Any() ? s.Grades.Average(g => g.Score) : 0
                })
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .Include(s => s.Grades)
                    .ThenInclude(g => g.Course)
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Email,
                    s.EnrollmentDate,
                    Courses = s.StudentCourses.Select(sc => new
                    {
                        sc.Course.Id,
                        sc.Course.Title,
                        sc.Course.Credits,
                        sc.EnrollmentDate
                    }),
                    Grades = s.Grades.Select(g => new
                    {
                        g.Id,
                        g.Score,
                        g.GradeLetter,
                        g.GradeDate,
                        CourseName = g.Course.Title
                    })
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }
    }
}
