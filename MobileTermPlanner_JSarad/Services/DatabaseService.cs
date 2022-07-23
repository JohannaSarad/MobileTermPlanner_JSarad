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

        //static Database Helpers
        public static Term CurrentTerm { get; set; }
        public static Course CurrentCourse { get; set; }
        public static Instructor CurrentInstructor { get; set; }
        public static Assessment CurrentAssessment { get; set; }
        public static Notes CurrentNotes { get; set; }
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
            await db.CreateTablesAsync<Term, Course, Instructor, Assessment, Notes>();
            //await db.CreateTableAsync<Notifications>();

            await FillSampleData();
        }

        //Term modifing methods
        public static async Task AddTerm(Term term)
        {
            //await Init();
            Term termToAdd = new Term
            {
                Name = term.Name,
                StartDate = term.StartDate,
                EndDate = term.EndDate,
                //NotifyStartDate = term.NotifyStartDate,
                //NotifyEndDate = term.NotifyEndDate
            };
            var id = await db.InsertAsync(termToAdd);
        }

        public static async Task UpdateTerm(Term term)
        {
            //await Init();

            var termQuery = await db.Table<Term>()
                .Where(i => i.Id == term.Id)
                .FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.Name = term.Name;
                termQuery.StartDate = term.StartDate;
                termQuery.EndDate = term.EndDate;
                //termQuery.NotifyStartDate = term.NotifyStartDate; ;
                //termQuery.NotifyEndDate = term.NotifyEndDate;

               await db.UpdateAsync(termQuery);
            }
        }

        public static async Task DeleteTerm(int id)
        {
            //await Init();
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

        //Term get methods
        public static async Task<Term> GetTerm(int id)
        {
            //await Init();
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

        //Course modifying methods
        public static async Task AddCourse(Course course, int id)
        {
            
            //await Init();

            Course courseToAdd = new Course
            {
                Name = course.Name,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                Status = course.Status,
                Notify = course.Notify,
                //NotifyStartDate = course.NotifyStartDate,
                //NotifyEndDate = course.NotifyEndDate,
                TermId = id
            };

            await db.InsertAsync(courseToAdd);
            LastAddedId = courseToAdd.Id;
        }

        public static async Task UpdateCourse(Course course)
        {
            //await Init();

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
                //courseQuery.NotifyStartDate = course.NotifyStartDate;
                //courseQuery.NotifyEndDate = course.NotifyEndDate;

                await db.UpdateAsync(courseQuery);
            }
        }

        public static async Task DeleteCourse(int id)
        {
            //await Init();
            await db.DeleteAsync<Course>(id);
            Instructor associatedInstructor = await GetInstuctorByCourse(id);
            await db.DeleteAsync<Instructor>(associatedInstructor.Id);
            Notes associatedNotes = await GetNotesByCourse(id);
            await db.DeleteAsync<Notes>(associatedNotes.Id);

            List<Assessment> associatedAssessments = await GetAssessmentsByCourse(id);
            if (associatedAssessments.Count > 0)
            {
                foreach (Assessment assessment in associatedAssessments)
                {
                    await db.DeleteAsync<Assessment>(assessment.Id);
                }
            }
        }

        //Course get Methods
        public static async Task<Course> GetCourse(int id)
        {
            //await Init();
            Course course = await db.Table<Course>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            return course;
        }

        public static async Task<List<Course>> GetCourses()
        {
            //await Init();
            var courses = await db.Table<Course>().ToListAsync();
            return courses;
        }

        public static async Task<List<Course>> GetCoursesByTerm(int id)
        {
            //await Init();
            var coursesByTerm = await db.Table<Course>()
                .Where(i => i.TermId == id)
                .ToListAsync();
            return coursesByTerm;
        }

        //Assessment modifing methods
        public static async Task AddAssessment(Assessment assessment, int courseId)
        {
            //await Init();

            Assessment assessmentToAdd = new Assessment
            {
                Type = assessment.Type,
                Name = assessment.Name,
                StartDate = assessment.StartDate,
                EndDate = assessment.EndDate,
                Notify = assessment.Notify,
                //NotifyStartDate = assessment.NotifyStartDate,
                //NotifyEndDate = assessment.NotifyEndDate,
                CourseId = courseId
            };
            await db.InsertAsync(assessmentToAdd);
        }

        public static async Task UpdateAssessment(Assessment assessment)
        {
            //await Init();

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
                //assessmentQuery.NotifyStartDate = assessment.NotifyStartDate;
                //assessmentQuery.NotifyEndDate = assessment.NotifyEndDate;
                await db.UpdateAsync(assessmentQuery);
            }
        }
        public static async Task DeleteAssessment(int id)
        {
            //await Init();
            await db.DeleteAsync<Assessment>(id);
        }

        //Assessment get methods
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
            //await Init();
            var assessmentsByCourse = await db.Table<Assessment>()
                .Where(i => i.CourseId == id)
                .ToListAsync();
            return assessmentsByCourse;
        }

        public static async Task<List<Assessment>> GetAssessments()
        {
            //await Init();
            var assessments = await db.Table<Assessment>().ToListAsync();
            return assessments;
        }

        //Instructor modifying methods
        public static async Task AddInstructor(Instructor instructor, int courseId)
        {
            //await Init();
            
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
            //await Init();

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
            //await Init();
            await db.DeleteAsync<Instructor>(id);
        }

        //Instructor get methods

        public static async Task<Instructor> GetInstructor(int id)
        {
            //await Init();
            Instructor instructor = await db.Table<Instructor>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            return instructor;
        }

        public static async Task<IEnumerable<Instructor>> GetInstructors()
        {
            //await Init();
            var instructors = await db.Table<Instructor>().ToListAsync();
            return instructors;
        }

        public static async Task<Instructor> GetInstuctorByCourse(int id)
        {
            //await Init();
            Instructor instructor = await db.Table<Instructor>()
                .Where(i => i.CourseId == id)
                .FirstOrDefaultAsync();
            return instructor;
        }

        //Notes modifying methods
        public static async Task AddNotes(Notes note, int courseId)
        {
            //await Init();

            Notes noteToAdd = new Notes
            {
                Note = note.Note,
                CourseId = courseId
            };

            await db.InsertAsync(noteToAdd);
        }

        public static async Task UpdateNotes(Notes note)
        {
            //await Init();

            var notesQuery = await db.Table<Notes>()
                .Where(i => i.Id == note.Id)
                .FirstOrDefaultAsync();

            if (notesQuery != null)
            {
                notesQuery.Note = note.Note;
                await db.UpdateAsync(notesQuery);
            }
        }

        public static async Task DeleteNotes(int id)
        {
            //await Init();
            await db.DeleteAsync<Notes>(id);
        }

        //Instructor get methods

        public static async Task<Notes> GetNote(int id)
        {
            //await Init();
            Notes note = await db.Table<Notes>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            return note;
        }

        public static async Task<IEnumerable<Notes>> GetNotes()
        {
            //await Init();
            var notes = await db.Table<Notes>().ToListAsync();
            return notes;
        }

        public static async Task<Notes> GetNotesByCourse(int id)
        {
            //await Init();
            //Notes placeholder;
            Notes note = await db.Table<Notes>()
                .Where(i => i.CourseId == id)
                .FirstOrDefaultAsync();
            //if (note != null)
            //{
            return note;
            //}
            //else
            //{
            //    placeholder = new Notes
            //    {
            //        Note = "There are no notes to display for this course",
            //        CourseId = id
            //    };
            //return placeholder;
            //}
        }

        //public static async Task AddNotification(Notifications notification)
        //{
        //    Notifications notificationToAdd = new Notifications
        //    {
        //        Type = notification.Type,
        //        TypeName = notification.TypeName,
        //        Occurrence = notification.Occurrence,
        //        NotifyDate = notification.NotifyDate
        //    };
        //    await db.InsertAsync(notificationToAdd);
        //}

        //public static async Task DeleteNotification(Notifications notification)
        //{
        //    var notificationQuery = await db.Table<Notifications>()
        //       .Where(i => i.Id == notification.Id)
        //       .FirstOrDefaultAsync();

        //    if (notificationQuery != null)
        //    {
        //        await db.DeleteAsync<Notifications>(notification.Id);
        //    }
        //}

        //public static async Task<List<Notifications>> GetNotifications()
        //{
        //    var notificationList = await db.Table<Notifications>().ToListAsync();
        //    return notificationList;
        //}
        
        //sample data
        public static async Task FillSampleData()
        {
            //await Task.Delay(1000);
            List<Term> terms = await db.Table<Term>().ToListAsync();
            //int lastId = 0;
            if (terms.Count == 0)
            {
                Term sampleTerm = new Term
                {
                    Name = "Term 1",
                    StartDate = new DateTime(2022, 01, 01),
                    EndDate = new DateTime(2022, 05, 31),
                    //NotifyStartDate = true,
                    //NotifyEndDate = false
                };
                var term = await db.InsertAsync(sampleTerm);
                int termId = sampleTerm.Id;

                Course sampleCourse = new Course
                {
                    Status = "Plan to Take",
                    Name = "Course 1",
                    StartDate = new DateTime(2022, 01, 01),
                    EndDate = new DateTime(2022, 01, 31),
                    Notify = true,
                    //NotifyStartDate = true,
                    //NotifyEndDate = false,
                    TermId = termId
                };
                var course = await db.InsertAsync(sampleCourse);
                int courseId = sampleCourse.Id;

                Instructor sampleInstructor = new Instructor
                {
                    Name = "Johanna Sarad",
                    Phone = "6614444763",
                    Email = "jsarad2@wgu.edu",
                    CourseId = courseId
                };
                var instructor = await db.InsertAsync(sampleInstructor);

                Assessment sampleAssessment1 = new Assessment
                {
                    Type = "Objective",
                    Name = "Assessment 1",
                    StartDate = new DateTime(2022, 01, 25),
                    EndDate = new DateTime(2022, 01, 31),
                    Notify = false,
                    //NotifyStartDate = false,
                    //NotifyEndDate = false,
                    CourseId = courseId
                };
                var assessment1 = await db.InsertAsync(sampleAssessment1);

                Assessment sampleAssessment2 = new Assessment
                {
                    Type = "Performance",
                    Name = "Assessment 2",
                    StartDate = new DateTime(2022, 01, 25),
                    EndDate = new DateTime(2022, 01, 31),
                    Notify = false,
                    //NotifyStartDate = false,
                    //NotifyEndDate = false,
                    CourseId = courseId
                };
                var assessment2 = await db.InsertAsync(sampleAssessment2);

                Notes sampleNotes = new Notes
                {
                    Note = null,
                    CourseId = courseId
                };

                var note = await db.InsertAsync(sampleNotes);
                Console.WriteLine("Sample Data filled filled");

            }
            else
            {
                Console.WriteLine("Sample Data filled");
                return;
            }
        }
    }
}
