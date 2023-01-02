using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MobileTermPlanner_JSarad.Models;
using SQLite;
using Xamarin.Essentials;

namespace MobileTermPlanner_JSarad.Services
{
    public static class DatabaseService
    {
        public static SQLiteAsyncConnection db;

        //static properties
        public static Term CurrentTerm { get; set; }
        public static Course CurrentCourse { get; set; }
        public static Instructor CurrentInstructor { get; set; }
        public static Assessment CurrentAssessment { get; set; }
        public static int LastAddedId { get; set; }
        public static bool IsBusy { get; set; }
        public static bool IsAdd { get; set; }
        
        public static async Task Init()
        {
            if (db != null)
            {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MobileTermPlanner.db");
            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTablesAsync<Term, Course, Instructor, Assessment>();
           
            await FillSampleData();
        }

        //term modifing methods
        public static async Task AddTerm(Term term)
        {
            Term termToAdd = new Term
            {
                Name = term.Name,
                StartDate = term.StartDate,
                EndDate = term.EndDate,
            };
            var id = await db.InsertAsync(termToAdd);
        }

        public static async Task UpdateTerm(Term term)
        {
            var termQuery = await db.Table<Term>()
                .Where(i => i.Id == term.Id)
                .FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.Name = term.Name;
                termQuery.StartDate = term.StartDate;
                termQuery.EndDate = term.EndDate;
                await db.UpdateAsync(termQuery);
            }
        }

        public static async Task DeleteTerm(int id)
        {
            await db.DeleteAsync<Term>(id);

            List<Course> associatedCourses = await GetCoursesByTerm(id);

            if (associatedCourses.Count > 0)
            {
                foreach (Course course in associatedCourses)
                {
                    await DeleteCourse(course.Id);
                }
            }
        }

        //term get methods
        public static async Task<Term> GetTerm(int id)
        {
            Term term = await db.Table<Term>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            return term;
        }
        
        public static async Task<List<Term>> GetTerms()
        {
            await Init();
            List<Term> terms = await db.Table<Term>().ToListAsync();
            return terms;
        }

        //course modifying methods
        public static async Task AddCourse(Course course, int id)
        {
            Course courseToAdd = new Course
            {
                Name = course.Name,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                Status = course.Status,
                Notify = course.Notify,
                Notes = course.Notes,
                TermId = id
            };
            await db.InsertAsync(courseToAdd);
            LastAddedId = courseToAdd.Id;
        }

        public static async Task UpdateCourse(Course course)
        {
            var courseQuery = await db.Table<Course>()
               .Where(i => i.Id == course.Id)
               .FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.Name = course.Name;
                courseQuery.StartDate = course.StartDate;
                courseQuery.EndDate = course.EndDate;
                courseQuery.Status = course.Status;
                courseQuery.Notify = course.Notify;
                courseQuery.Notes = course.Notes;
                await db.UpdateAsync(courseQuery);
            }
        }

        public static async Task DeleteCourse(int id)
        {
            await db.DeleteAsync<Course>(id);
            Instructor associatedInstructor = await GetInstuctorByCourse(id);
            await db.DeleteAsync<Instructor>(associatedInstructor.Id);
            
            List<Assessment> associatedAssessments = await GetAssessmentsByCourse(id);
            if (associatedAssessments.Count > 0)
            {
                foreach (Assessment assessment in associatedAssessments)
                {
                    await db.DeleteAsync<Assessment>(assessment.Id);
                }
            }
        }

        //course get Methods
        public static async Task<Course> GetCourse(int id)
        {
            Course course = await db.Table<Course>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            return course;
        }

        public static async Task<List<Course>> GetCourses()
        {
            var courses = await db.Table<Course>().ToListAsync();
            return courses;
        }

        public static async Task<List<Course>> GetCoursesByTerm(int id)
        {
            var coursesByTerm = await db.Table<Course>()
                .Where(i => i.TermId == id)
                .ToListAsync();
            return coursesByTerm;
        }

        //assessment modifing methods
        public static async Task AddAssessment(Assessment assessment, int courseId)
        {
            Assessment assessmentToAdd = new Assessment
            {
                Type = assessment.Type,
                Name = assessment.Name,
                StartDate = assessment.StartDate,
                EndDate = assessment.EndDate,
                Notify = assessment.Notify,
                CourseId = courseId
            };
            await db.InsertAsync(assessmentToAdd);
        }

        public static async Task UpdateAssessment(Assessment assessment)
        {
            var assessmentQuery = await db.Table<Assessment>()
                .Where(i => i.Id == assessment.Id)
                .FirstOrDefaultAsync();

            if (assessmentQuery != null)
            {
                assessmentQuery.Type = assessment.Type;
                assessmentQuery.Name = assessment.Name;
                assessmentQuery.StartDate = assessment.StartDate;
                assessmentQuery.EndDate = assessment.EndDate;
                assessmentQuery.Notify = assessment.Notify;
                await db.UpdateAsync(assessmentQuery);
            }
        }
        public static async Task DeleteAssessment(int id)
        {
            await db.DeleteAsync<Assessment>(id);
        }

        //assessment get methods...
        public static async Task<Assessment> GetAssessment(int id)
        {
            //await Init();
            Assessment assessment = await db.Table<Assessment>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            return assessment;
        }
        
        public static async Task<List<Assessment>> GetAssessmentsByCourse(int id)
        {
            var assessmentsByCourse = await db.Table<Assessment>()
                .Where(i => i.CourseId == id)
                .ToListAsync();
            return assessmentsByCourse;
        }

        public static async Task<List<Assessment>> GetAssessments()
        {
            var assessments = await db.Table<Assessment>().ToListAsync();
            return assessments;
        }

        //instructor modifying methods
        public static async Task AddInstructor(Instructor instructor, int courseId)
        {
            Instructor instructorToAdd = new Instructor
            {
                Name = instructor.Name,
                Email = instructor.Email,
                Phone = instructor.Phone,
                CourseId = courseId
            };
            await db.InsertAsync(instructorToAdd);
        }

        public static async Task UpdateInstructor(Instructor instructor)
        {
            var instructorQuery = await db.Table<Instructor>()
                .Where(i => i.Id == instructor.Id)
                .FirstOrDefaultAsync();

            if (instructorQuery != null)
            {
                instructorQuery.Name = instructor.Name;
                instructorQuery.Phone = instructor.Phone;
                instructorQuery.Email = instructor.Email;
                await db.UpdateAsync(instructorQuery);
            }
        }

        public static async Task DeleteInstructor(int id)
        {
            await db.DeleteAsync<Instructor>(id);
        }

        //instructor get methods
        public static async Task<Instructor> GetInstructor(int id)
        {
            Instructor instructor = await db.Table<Instructor>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            return instructor;
        }

        public static async Task<IEnumerable<Instructor>> GetInstructors()
        {
            var instructors = await db.Table<Instructor>().ToListAsync();
            return instructors;
        }

        public static async Task<Instructor> GetInstuctorByCourse(int id)
        {
            Instructor instructor = await db.Table<Instructor>()
                .Where(i => i.CourseId == id)
                .FirstOrDefaultAsync();
            return instructor;
        }
        
        //sample data
        public static async Task FillSampleData()
        {
            //await Task.Delay(1000);
            List<Term> terms = await db.Table<Term>().ToListAsync();
            //int lastId = 0;
            if (terms.Count == 0)
            {
                Term sampleTerm1 = new Term
                {
                    Name = "Term 1 (Spring)",
                    StartDate = new DateTime(2023, 01, 18),
                    EndDate = new DateTime(2023, 05, 25),
                };
                var term1 = await db.InsertAsync(sampleTerm1);

                Term sampleTerm2 = new Term
                {
                    Name = "Term 2 (Summer)",
                    StartDate = new DateTime(2023, 05, 28),
                    EndDate = new DateTime(2023, 08, 14),
                };
                var term2 = await db.InsertAsync(sampleTerm2);

                Term sampleTerm3 = new Term
                {
                    Name = "Term 1 (Fall)",
                    StartDate = new DateTime(2023, 08, 17),
                    EndDate = new DateTime(2023, 12, 22),
                };
                var term3 = await db.InsertAsync(sampleTerm3);
                int termId = sampleTerm1.Id;

                Course sampleCourse1 = new Course
                {
                    Status = "Plan to Take",
                    Name = "Mobile Application Developement Using C#",
                    StartDate = new DateTime(2023, 01, 18),
                    EndDate = new DateTime(2023, 05, 25),
                    Notify = true,
                    TermId = termId
                };
                var course1 = await db.InsertAsync(sampleCourse1);
                int courseId = sampleCourse1.Id;

                Course sampleCourse2 = new Course
                {
                    Status = "Plan to Take",
                    Name = "Scripting and Programming Applications",
                    StartDate = new DateTime(2023, 01, 18),
                    EndDate = new DateTime(2023, 05, 25),
                    Notify = true,
                    TermId = termId
                };
                var course2 = await db.InsertAsync(sampleCourse2);
                int courseId2 = sampleCourse2.Id;

                Course sampleCourse3 = new Course
                {
                    Status = "Plan to Take",
                    Name = "Calculus II",
                    StartDate = new DateTime(2023, 01, 18),
                    EndDate = new DateTime(2023, 05, 25),
                    Notify = true,
                    TermId = termId
                };
                var course3 = await db.InsertAsync(sampleCourse3);
                int courseId3 = sampleCourse3.Id;

                Course sampleCourse4 = new Course
                {
                    Status = "Plan to Take",
                    Name = "User Interface Design",
                    StartDate = new DateTime(2023, 01, 18),
                    EndDate = new DateTime(2023, 05, 25),
                    Notify = true,
                    TermId = termId
                };
                var course4 = await db.InsertAsync(sampleCourse4);
                int courseId4 = sampleCourse4.Id;

                Instructor sampleInstructor1 = new Instructor
                {
                    Name = "Edwin Miller",
                    Phone = "5555555555",
                    Email = "e@professor.edu",
                    CourseId = courseId
                };

                var instructor = await db.InsertAsync(sampleInstructor1);

                Instructor sampleInstructor2 = new Instructor
                {
                    Name = "Rebecca Crocker",
                    Phone = "4444444444",
                    Email = "r@professor.edu",
                    CourseId = courseId2
                };
                var instructor2 = await db.InsertAsync(sampleInstructor2);

                Instructor sampleInstructor3 = new Instructor
                {
                    Name = "Abigail Ledford",
                    Phone = "3333333333",
                    Email = "a@professor.edu",
                    CourseId = courseId3
                };
                var instructor3 = await db.InsertAsync(sampleInstructor3);

                Instructor sampleInstructor4 = new Instructor
                {
                    Name = "John Doe",
                    Phone = "2222222222",
                    Email = "j@professor.edu",
                    CourseId = courseId4
                };
                var instructor4 = await db.InsertAsync(sampleInstructor4);

                Assessment sampleAssessment1 = new Assessment
                {
                    Type = "Objective",
                    Name = "Assessment 1",
                    StartDate = new DateTime(2023, 05, 23),
                    EndDate = new DateTime(2023, 05, 24),
                    Notify = false,
                    CourseId = courseId
                };
                var assessment1 = await db.InsertAsync(sampleAssessment1);

                Assessment sampleAssessment2 = new Assessment
                {
                    Type = "Performance",
                    Name = "Assessment 2",
                    StartDate = new DateTime(2023, 05, 21),
                    EndDate = new DateTime(2023, 05, 22),
                    Notify = false,
                    CourseId = courseId
                };
                var assessment2 = await db.InsertAsync(sampleAssessment2);
            }
            else
            {
                return;
            }
        }
    }
}
