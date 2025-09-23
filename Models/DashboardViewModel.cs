namespace GradeALMS.Models
{
    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalCourses { get; set; }
        public int ActiveEnrollments { get; set; }
        public double AverageGrade { get; set; }
        public List<RecentActivity> RecentActivities { get; set; } = new List<RecentActivity>();
    }

    public class RecentActivity
    {
        public DateTime Date { get; set; }
        public string Activity { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string Student { get; set; } = string.Empty;
    }
}
