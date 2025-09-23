using GradeALMS.Data;
using GradeALMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GradeALMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalStudents = await _context.Students.CountAsync(),
                TotalInstructors = await _context.Instructors.CountAsync(),
                TotalCourses = await _context.Courses.CountAsync(),
                ActiveEnrollments = await _context.StudentCourses.CountAsync(),
                AverageGrade = await _context.Grades.AverageAsync(g => (double?)g.Score) ?? 0.0
            };

            // Get recent activities
            var recentGrades = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Course)
                .OrderByDescending(g => g.GradeDate)
                .Take(5)
                .Select(g => new RecentActivity
                {
                    Date = g.GradeDate,
                    Activity = "Assignment graded",
                    Course = g.Course!.Title,
                    Student = g.Student!.Name
                })
                .ToListAsync();

            var recentEnrollments = await _context.StudentCourses
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .OrderByDescending(sc => sc.EnrollmentDate)
                .Take(3)
                .Select(sc => new RecentActivity
                {
                    Date = sc.EnrollmentDate,
                    Activity = "New enrollment added",
                    Course = sc.Course!.Title,
                    Student = sc.Student!.Name
                })
                .ToListAsync();

            viewModel.RecentActivities = recentGrades.Concat(recentEnrollments)
                .OrderByDescending(ra => ra.Date)
                .Take(4)
                .ToList();

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
