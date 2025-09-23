using GradeALMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GradeALMS.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                // Ensure the database is created
                await context.Database.EnsureCreatedAsync();

                // Create roles if they don't exist
                await CreateRolesAsync(roleManager, logger);

                // Check if database is already seeded
                if (await context.Users.AnyAsync())
                {
                    return; // DB has been seeded
                }

                // Create default admin user
                await CreateAdminUserAsync(userManager, logger);

                // Create sample instructor users and entities
                var instructors = await CreateInstructorUsersAsync(userManager, context, logger);

                // Create sample student users and entities
                var students = await CreateStudentUsersAsync(userManager, context, logger);

                // Create courses
                var courses = await CreateCoursesAsync(context, instructors, logger);

                // Create student enrollments and grades
                await CreateEnrollmentsAndGradesAsync(context, students, courses, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            string[] roles = { UserRoles.Admin, UserRoles.Instructor, UserRoles.Student };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation($"Created role: {role}");
                }
            }
        }

        private static async Task CreateAdminUserAsync(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            var adminEmail = "admin@gradealms.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                    logger.LogInformation($"Created admin user: {adminEmail}");
                }
            }
        }

        private static async Task<List<Instructor>> CreateInstructorUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger logger)
        {
            var instructorData = new[]
            {
                new { Name = "Dr. Sarah Johnson", Email = "sarah.johnson@gradealms.com", HireDate = new DateTime(2018, 8, 15) },
                new { Name = "Prof. Michael Chen", Email = "michael.chen@gradealms.com", HireDate = new DateTime(2020, 1, 10) },
                new { Name = "Dr. Emily Rodriguez", Email = "emily.rodriguez@gradealms.com", HireDate = new DateTime(2019, 9, 1) }
            };

            var instructors = new List<Instructor>();

            foreach (var data in instructorData)
            {
                var user = new ApplicationUser
                {
                    UserName = data.Email,
                    Email = data.Email,
                    FullName = data.Name,
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now
                };

                var result = await userManager.CreateAsync(user, "Instructor123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Instructor);

                    var instructor = new Instructor
                    {
                        Name = data.Name,
                        Email = data.Email,
                        HireDate = data.HireDate,
                        ApplicationUserId = user.Id
                    };

                    context.Instructors.Add(instructor);
                    instructors.Add(instructor);
                    logger.LogInformation($"Created instructor: {data.Name}");
                }
            }

            await context.SaveChangesAsync();
            return instructors;
        }

        private static async Task<List<Student>> CreateStudentUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger logger)
        {
            var studentData = new[]
            {
                new { Name = "John Doe", Email = "john.doe@student.gradealms.com", EnrollmentDate = new DateTime(2023, 8, 20) },
                new { Name = "Jane Smith", Email = "jane.smith@student.gradealms.com", EnrollmentDate = new DateTime(2023, 8, 20) },
                new { Name = "Alice Johnson", Email = "alice.johnson@student.gradealms.com", EnrollmentDate = new DateTime(2023, 1, 15) },
                new { Name = "Robert Brown", Email = "robert.brown@student.gradealms.com", EnrollmentDate = new DateTime(2023, 1, 15) },
                new { Name = "Emily Davis", Email = "emily.davis@student.gradealms.com", EnrollmentDate = new DateTime(2022, 8, 22) }
            };

            var students = new List<Student>();

            foreach (var data in studentData)
            {
                var user = new ApplicationUser
                {
                    UserName = data.Email,
                    Email = data.Email,
                    FullName = data.Name,
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now
                };

                var result = await userManager.CreateAsync(user, "Student123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Student);

                    var student = new Student
                    {
                        Name = data.Name,
                        Email = data.Email,
                        EnrollmentDate = data.EnrollmentDate,
                        ApplicationUserId = user.Id
                    };

                    context.Students.Add(student);
                    students.Add(student);
                    logger.LogInformation($"Created student: {data.Name}");
                }
            }

            await context.SaveChangesAsync();
            return students;
        }

        private static async Task<List<Course>> CreateCoursesAsync(ApplicationDbContext context, List<Instructor> instructors, ILogger logger)
        {
            var courses = new List<Course>
            {
                new Course
                {
                    Title = "csci101",
                    Description = "Fundamentals of computer science including programming, algorithms, and data structures.",
                    Credits = 3,
                    InstructorId = instructors[0].Id
                },
                new Course
                {
                    Title = "Math 203",
                    Description = "Differential calculus with applications to science and engineering.",
                    Credits = 4,
                    InstructorId = instructors[1].Id
                },
                new Course
                {
                    Title = "Engl201",
                    Description = "Academic writing and critical thinking skills development.",
                    Credits = 3,
                    InstructorId = instructors[2].Id
                },
                new Course
                {
                    Title = "Csci207",
                    Description = "Advanced programming concepts including data structures and algorithm design.",
                    Credits = 4,
                    InstructorId = instructors[0].Id
                }
            };

            context.Courses.AddRange(courses);
            await context.SaveChangesAsync();

            foreach (var course in courses)
            {
                logger.LogInformation($"Created course: {course.Title}");
            }

            return courses;
        }

        private static async Task CreateEnrollmentsAndGradesAsync(ApplicationDbContext context, List<Student> students, List<Course> courses, ILogger logger)
        {
            // Create Student-Course enrollments
            var studentCourses = new List<StudentCourse>
            {
                // John Doe enrollments
                new StudentCourse { StudentId = students[0].Id, CourseId = courses[0].Id, EnrollmentDate = new DateTime(2023, 8, 25) },
                new StudentCourse { StudentId = students[0].Id, CourseId = courses[1].Id, EnrollmentDate = new DateTime(2023, 8, 25) },
                
                // Jane Smith enrollments
                new StudentCourse { StudentId = students[1].Id, CourseId = courses[0].Id, EnrollmentDate = new DateTime(2023, 8, 25) },
                new StudentCourse { StudentId = students[1].Id, CourseId = courses[2].Id, EnrollmentDate = new DateTime(2023, 8, 25) },
                
                // Alice Johnson enrollments
                new StudentCourse { StudentId = students[2].Id, CourseId = courses[1].Id, EnrollmentDate = new DateTime(2023, 1, 20) },
                new StudentCourse { StudentId = students[2].Id, CourseId = courses[2].Id, EnrollmentDate = new DateTime(2023, 1, 20) },
                new StudentCourse { StudentId = students[2].Id, CourseId = courses[3].Id, EnrollmentDate = new DateTime(2023, 8, 25) },
                
                // Robert Brown enrollments
                new StudentCourse { StudentId = students[3].Id, CourseId = courses[0].Id, EnrollmentDate = new DateTime(2023, 1, 20) },
                new StudentCourse { StudentId = students[3].Id, CourseId = courses[3].Id, EnrollmentDate = new DateTime(2023, 8, 25) },
                
                // Emily Davis enrollments
                new StudentCourse { StudentId = students[4].Id, CourseId = courses[1].Id, EnrollmentDate = new DateTime(2022, 8, 27) },
                new StudentCourse { StudentId = students[4].Id, CourseId = courses[2].Id, EnrollmentDate = new DateTime(2022, 8, 27) }
            };

            context.StudentCourses.AddRange(studentCourses);
            await context.SaveChangesAsync();

            // Create Grades
            var grades = new List<Grade>
            {
                // John Doe grades
                new Grade
                {
                    StudentId = students[0].Id,
                    CourseId = courses[0].Id,
                    Score = 85.5,
                    GradeDate = new DateTime(2023, 12, 15)
                },
                new Grade
                {
                    StudentId = students[0].Id,
                    CourseId = courses[1].Id,
                    Score = 78.0,
                    GradeDate = new DateTime(2023, 12, 18)
                },
                
                // Jane Smith grades
                new Grade
                {
                    StudentId = students[1].Id,
                    CourseId = courses[0].Id,
                    Score = 92.0,
                    GradeDate = new DateTime(2023, 12, 15)
                },
                new Grade
                {
                    StudentId = students[1].Id,
                    CourseId = courses[2].Id,
                    Score = 88.5,
                    GradeDate = new DateTime(2023, 12, 20)
                },
                
                // Alice Johnson grades
                new Grade
                {
                    StudentId = students[2].Id,
                    CourseId = courses[1].Id,
                    Score = 95.0,
                    GradeDate = new DateTime(2023, 5, 15)
                },
                new Grade
                {
                    StudentId = students[2].Id,
                    CourseId = courses[2].Id,
                    Score = 90.5,
                    GradeDate = new DateTime(2023, 5, 18)
                },
                
                // Robert Brown grades
                new Grade
                {
                    StudentId = students[3].Id,
                    CourseId = courses[0].Id,
                    Score = 82.0,
                    GradeDate = new DateTime(2023, 5, 15)
                },
                
                // Emily Davis grades
                new Grade
                {
                    StudentId = students[4].Id,
                    CourseId = courses[1].Id,
                    Score = 87.5,
                    GradeDate = new DateTime(2022, 12, 15)
                },
                new Grade
                {
                    StudentId = students[4].Id,
                    CourseId = courses[2].Id,
                    Score = 91.0,
                    GradeDate = new DateTime(2022, 12, 18)
                }
            };

            // Calculate grade letters for each grade
            foreach (var grade in grades)
            {
                grade.CalculateGradeLetter();
            }

            context.Grades.AddRange(grades);
            await context.SaveChangesAsync();

            logger.LogInformation($"Created {studentCourses.Count} enrollments and {grades.Count} grades");
        }
    }
}
