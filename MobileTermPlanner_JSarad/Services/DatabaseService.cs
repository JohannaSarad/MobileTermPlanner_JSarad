using System;
using System.Collections.Generic;
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
        //public static Term CurrentTerm { get; set; }
        //public static Course CurrentCourse { get; set; }
        //public static bool IsAdd { get; set; }


        public static async Task Init()
        {
            if (db != null)
            {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MobileTermPlanner.db");
            db = new SQLiteAsyncConnection(databasePath);
            db.CreateTableAsync<Term>().Wait();
            
            
            await FillSampleTerm();
            //if (await TableExists("Course") == false)
            //{
            //    db.CreateTableAsync<Course>().Wait();
            //    await FillSampleCourse();
            //}
            //if (await TableExists("Instructor") == false)
            //{
            //    db.CreateTableAsync<Instructor>().Wait();
            //    await FillSampleInstructor();
            //}
            //if (await TableExists("Assessment") == false)
            //{
            //    db.CreateTableAsync<Assessment>().Wait();
            //    await FillSampleAssessment();
            //}

        }
        
        public static async Task AddTerm(Term obj)
        {
            await Init();
            Term term = new Term
            {
                Name = obj.Name,
                StartDate = obj.StartDate,
                EndDate = obj.EndDate,
                //StartDateNotification = startDateNotification,
                //EndDateNotification = endDateNotification
            };
            var id = await db.InsertAsync(term);
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
          
            //return await db.Table<Term>().ToListAsync();
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

        //public static async Task AddCourse(string name, DateTime startDate, DateTime endDate,  int termId)
        //{
        //    //bool startDateNotification, bool endDateNotification
        //    await Init();

        //    Course course = new Course
        //    {
        //        Name = name,
        //        StartDate = startDate,
        //        EndDate = endDate,
        //        //StartDateNotification = startDateNotification,
        //        //EndDateNotification = endDateNotification,
        //        TermId = termId
        //    };

        //    var id = await db.InsertAsync(course);
        //}

        public static async Task DeleteCourse(int id)
        {
            await Init();
            await db.DeleteAsync(id);
        }

        public static async Task<IEnumerable<Course>> GetCourse()
        {
            await Init();
            var courses = await db.Table<Course>().ToListAsync();
            return courses;
        }

        //public static async Task UpdateCourse(int id, string name, DateTime startDate, DateTime endDate, bool startDateNotification, bool endDateNotification)
        //{
        //    await Init();

        //    var courseQuery = await db.Table<Course>()
        //       .Where(i => i.Id == id)
        //       .FirstOrDefaultAsync();

        //    if (courseQuery != null)
        //    {
        //        courseQuery.Name = name;
        //        courseQuery.StartDate = startDate;
        //        courseQuery.EndDate = endDate;
        //        courseQuery.StartDateNotification = startDateNotification;
        //        courseQuery.EndDateNotification = endDateNotification;

        //        await db.UpdateAsync(courseQuery);
        //    }
        //}

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

        //public static async Task<bool> TableEmpty(string tableName)
        //{
        //    try
        //    {
        //        var tableInfo = await db.GetTableInfoAsync($"{tableName}");
        //        int count = tableInfo.Count;
        //        if (tableInfo.Count > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

       public static async Task FillSampleTerm()
        {
            List<Term> terms = await db.Table<Term>().ToListAsync();
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
                var id = await db.InsertAsync(sampleTerm);
            }
            else
            {
                return;
            }
        }

        //public static async Task FillSampleCourse()
        //{
        //    Course sampleCourse = new Course
        //    {
        //        Name = "Course 1",
        //        StartDate = new DateTime(2022, 01, 01),
        //        EndDate = new DateTime(2022, 01, 31),
        //        //StartDateNotification = true,
        //        //EndDateNotification = false,
        //        TermId = 1
        //    };

        //    await db.InsertAsync(sampleCourse);
        //}

        //public static async Task FillSampleInstructor()
        //{
        //    Instructor sampleInstructor = new Instructor
        //    {
        //        Name = "Johanna Sarad",
        //        Phone = "6614444763",
        //        Email = "jsarad2@wgu.edu",
        //        //StartDateNotification = true,
        //        //EndDateNotification = false,
        //        CourseId = 1
        //    };

        //    await db.InsertAsync(sampleInstructor);
        //}

        //public static async Task FillSampleAssessment()
        //{
        //    Assessment sampleAssessment = new Assessment();
        //    {

        //    };

        //    await db.InsertAsync(sampleAssessment);
        //}
    }
}
