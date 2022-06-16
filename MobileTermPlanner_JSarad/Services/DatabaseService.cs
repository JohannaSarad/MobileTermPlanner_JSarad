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
        public static Term SelectedTerm { get; set; }
        public static int TargetTermId { get; set; }
        public static Course SelectedCourse { get; set; }
        public static int TargetCourseId { get; set; }
        public static bool IsAdd { get; set; }


        public static async Task Init()
        {
            if (db != null)
            {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MobileTermPlanner.db");
            db = new SQLiteAsyncConnection(databasePath);
            db.CreateTablesAsync<Term, Course, Instructor>().Wait();
            
            await FillSampleData();
        }
        
        public static async Task AddTerm(Term term)
        {
            await Init();
            Term termToAdd = new Term
            {
                Name = term.Name,
                StartDate = term.StartDate,
                EndDate = term.EndDate,
                //StartDateNotification = startDateNotification,
                //EndDateNotification = endDateNotification
            };
            TargetTermId = await db.InsertAsync(termToAdd);
        }

        public static async Task DeleteTerm(int id)
        {
            await Init();
            await db.DeleteAsync<Term>(id);
        }

        public static async Task<List<Term>> GetTerm()
        {
            await Init();
            List<Term> terms = await db.Table<Term>().ToListAsync();
            return terms;
        }
        
        public static async Task UpdateTerm(Term term)
        {
            await Init();

            var termQuery = await db.Table<Term>()
                .Where(i => i.Id == term.Id)
                .FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.Name = term.Name;
                termQuery.StartDate = term.StartDate;
                termQuery.EndDate = term.EndDate;

                //termQuery.StartDateNotification = startDateNotification;
                //termQuery.EndDateNotification = endDateNotification;

                var id = await db.UpdateAsync(termQuery);
            }
        }

        public static async Task AddCourse(Course course)
        {
            //bool startDateNotification, bool endDateNotification
            await Init();

            Course courseToAdd = new Course
            {
                Name = course.Name,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                //StartDateNotification = startDateNotification,
                //EndDateNotification = endDateNotification,
                TermId = SelectedTerm.Id
            };

            TargetCourseId = await db.InsertAsync(courseToAdd);
        }

        public static async Task DeleteCourse(int id)
        {
            await Init();
            await db.DeleteAsync(id);
        }

        public static async Task<List<Course>> GetCourseByTerm(int id)
        {
            await Init();
            var coursesByTerm = await db.Table<Course>()
                .Where(i => i.TermId == SelectedTerm.Id)
                .ToListAsync();
            return coursesByTerm;

        }
        public static async Task<List<Course>> GetCourse()
        {
            await Init();
            var courses = await db.Table<Course>().ToListAsync();
            return courses;
        }

        public static async Task UpdateCourse(Course course)
        {
            await Init();

            var courseQuery = await db.Table<Course>()
               .Where(i => i.Id == course.Id)
               .FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.Name = course.Name;
                courseQuery.StartDate = course.StartDate;
                courseQuery.EndDate = course.EndDate;
                //courseQuery.Status = course.Status;
                //courseQuery.StartDateNotification = startDateNotification;
                //courseQuery.EndDateNotification = endDateNotification;
                
                await db.UpdateAsync(courseQuery);
            }
        }

        public static async Task AddAssessment()
        {
            await Init();
        }

        public static async Task DeleteAssessment(int id)
        {
            await Init();
        }

        public static async Task GetAssessment()
        {
            await Init();
        }

        public static async Task UpdateAssessment(string name, DateTime startDate, DateTime endDate, bool startDateNotification, bool endDateNotification)
        {
            await Init();
        }

        //public static async Task AddInstructor(string name, string phone, string email, int courseId)
        //{
        //    await Init();
        //    Instructor instructor = new Instructor
        //    {
        //        Name = name,
        //        Phone = phone,
        //        Email = email,
        //        CourseId = courseId
        //    };

        //    var id = await db.InsertAsync(instructor);
        //}

        public static async Task DeleteInstructor(int id)
        {
            await Init();
            await db.DeleteAsync(id);
        }

        //public static async Task<IEnumerable<Instructor>> GetInstructor()
        //{
        //    await Init();


        //    var instructors = await db.Table<Instructor>().ToListAsync();
        //    return instructors;

        //}

        //public static async Task UpdateInstructor(int id, string name, string phone, string email)
        //{
        //    await Init();

        //    var instructorQuery = await db.Table<Instructor>()
        //        .Where(i => i.Id == id)
        //        .FirstOrDefaultAsync();

        //    if (instructorQuery != null)
        //    {
        //        instructorQuery.Name = name;
        //        instructorQuery.Phone = phone;
        //        instructorQuery.Email = email;

        //        await db.UpdateAsync(instructorQuery);
        //    }
        //}

        public static async Task FillSampleData()
        {
            List<Term> terms = await db.Table<Term>().ToListAsync();
            //var id = 0;
            if (terms.Count == 0)
            {
                Term sampleTerm = new Term
                {
                    Name = "Term 1",
                    StartDate = new DateTime(2022, 01, 01),
                    EndDate = new DateTime(2022, 05, 31),
                    //StartDateNotification = true,
                    //EndDateNotification = false
                };

                await db.InsertAsync(sampleTerm);
                terms = await db.Table<Term>().ToListAsync();
                
                for (int i = 0; i < 1; i++)
                {
                    //SelectedTerm = terms[i];
                    TargetTermId = terms[i].Id;
                }

                Course sampleCourse = new Course
                {
                    Name = "Course 1",
                    StartDate = new DateTime(2022, 01, 01),
                    EndDate = new DateTime(2022, 01, 31),
                    //StartDateNotification = true,
                    //EndDateNotification = false,
                    TermId = TargetTermId
                };
                //id = 
                await db.InsertAsync(sampleCourse);
                
                List<Course> courses = await db.Table<Course>().ToListAsync();
                for (int i = 0; i < 1; i++)
                {
                    TargetCourseId = courses[i].Id;
                }

                Instructor sampleInstructor = new Instructor
                {
                    Name = "Johanna Sarad",
                    Phone = "6614444763",
                    Email = "jsarad2@wgu.edu",
                    CourseId = TargetCourseId
                };
                await db.InsertAsync(sampleInstructor);
            }
            else
            {
                return;
            }
        }
    }
}
