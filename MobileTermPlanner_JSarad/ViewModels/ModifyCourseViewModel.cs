using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileTermPlanner_JSarad.Models;
using MobileTermPlanner_JSarad.Services;
using Xamarin.Forms;

namespace MobileTermPlanner_JSarad.ViewModels
{
    public class ModifyCourseViewModel : BaseViewModel
    {
        public List<Course> CourseList { get; set; }

        public List<string> StatusList { get; } = new List<string>{ "In Progress", "Completed", "Plan to Take", "Dropped" };
        public string StatusTitle { get; } = "Select a Course Status";

        //course properties
        private Course _course;
        public Course Course
        {
            get
            {
                return _course;
            }
            set
            {
                _course = value;
                OnPropertyChanged();
            }
        }

        public string CourseName
        {
            get
            {
                return _course.Name;
            }
            set
            {
                _course.Name = value;
                OnPropertyChanged();
                ValidString(CourseName);
                EmptyErrorMessageOne = ValidationMessage;
            }
        }

        public DateTime CourseStartDate
        {
            get
            {
                return _course.StartDate;
            }
            set
            {
                _course.StartDate = value;
                OnPropertyChanged();
                ValidDates(CourseStartDate, CourseEndDate);
                DatesErrorMessageOne = ValidationMessage;
                //ValidateDates(_modifyCourse.StartDate, _modifyCourse.EndDate);
            }
        }

        public DateTime CourseEndDate
        {
            get
            {
                return _course.EndDate;
            }
            set
            {
                _course.EndDate = value;
                OnPropertyChanged();
                ValidDates(CourseStartDate, CourseEndDate);
                DatesErrorMessageOne = ValidationMessage;
            }
        }

        public string Status
        {
            get
            {
                return _course.Status;
            }
            set
            {
                _course.Status = value;
                OnPropertyChanged();
                ValidSelection(Status, StatusTitle, "Course Status");
                SelectionErrorMessage = ValidationMessage;
            }
        }

        public bool NotifyStart
        {
            get
            {
                return _course.NotifyStartDate;
            }
            set
            {
                _course.NotifyStartDate = value;
                OnPropertyChanged();
                UpdateNotifyLabel(NotifyStart, "Start");
                StartDateLabel = ValidationMessage;

            }
        }

        public bool NotifyEnd
        {
            get
            {
                return _course.NotifyEndDate;
            }
            set
            {
                _course.NotifyEndDate = value;
                OnPropertyChanged();
                UpdateNotifyLabel(NotifyEnd, "End");
                EndDateLabel = ValidationMessage;
            }
        }

        //instructor properties
        private Instructor _instructor;
        public Instructor Instructor
        {
            get
            {
                return _instructor;
            }
            set
            {
                _instructor = value;
                OnPropertyChanged();
            }
        }

        public string InstructorName
        {
            get
            {
                return _instructor.Name;
            }
            set
            {
                _instructor.Name = value;
                OnPropertyChanged();
                ValidString(InstructorName);
                EmptyErrorMessageTwo = ValidationMessage;
            }
        }

        public string Email
        {
            get
            {
                return _instructor.Email;
            }
            set
            {
                _instructor.Email = value;
                OnPropertyChanged();
                ValidEmail(Email);
                EmailErrorMessage = ValidationMessage;
            }
        }

        
        public string Phone
        {
            get
            {
                return _instructor.Phone;
            }
            set
            {
                _instructor.Phone = value;
                OnPropertyChanged();
                ValidPhone(Phone);
                PhoneErrorMessage = ValidationMessage;
            }
        }

       
        //public string AddEdit { get; set; }
        public bool IsValidInput;

        //commands
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ModifyCourseViewModel()
        {
            if (DatabaseService.IsAdd)
            {
                //AddEdit = "Add Course";
                Course = new Course();
                CourseStartDate = DatabaseService.CurrentTerm.StartDate;
                CourseEndDate = CourseStartDate.AddDays(30);
                NotifyStart = false;
                NotifyEnd = false;
                Instructor = new Instructor();
            }
            else
            {
                //AddEdit = "Edit Course";
                Course = DatabaseService.CurrentCourse;
                Instructor = DatabaseService.CurrentInstructor;
            }

            SaveCommand = new Command(async () => await SaveCourse());
            CancelCommand = new Command(async () => await CancelCourse());
        }

        //methods
        
        private async Task SaveCourse()
        {
            CourseList = await DatabaseService.GetCourseByTerm(DatabaseService.CurrentTerm.Id);
            IsValidInput = true;

            //validates unchanged properties and displays any errors to user on save attempt
            ValidString(CourseName);
            EmptyErrorMessageOne = ValidationMessage;
            ValidString(InstructorName);
            EmptyErrorMessageTwo = ValidationMessage;
            ValidEmail(Email);
            EmailErrorMessage = ValidationMessage;
            ValidPhone(Phone);
            PhoneErrorMessage = ValidationMessage;
            ValidSelection(Status, StatusTitle, "Course Status");
            SelectionErrorMessage = ValidationMessage;
            
            //checks for overlapping courses 
            if (CourseList.Count > 0)
            {
                int i;
                for (i = 0; i < CourseList.Count; i++)
                {
                    if (((CourseList[i].StartDate <= Course.EndDate && CourseList[i].StartDate >= Course.StartDate) ||
                        (CourseList[i].EndDate <= Course.EndDate && CourseList[i].EndDate >= Course.StartDate)) && (CourseList[i].Id != Course.Id))
                    {
                        IsValidInput = false;
                        await Application.Current.MainPage.DisplayAlert("Overlapping Course", $" * There is an overlapping term for these dates for " +
                            $"Term { CourseList[i].Name} from { CourseList[i].StartDate.Date} to {CourseList[i].EndDate.Date}", "Ok");
                    }
                }
            }

            ////checks that new or edited Course dates are within the dates of the Term for future use
            //if (DatabaseService.CurrentTerm.StartDate > Course.StartDate || DatabaseService.CurrentTerm.StartDate > Course.EndDate
            //    || DatabaseService.CurrentTerm.EndDate < Course.StartDate || DatabaseService.CurrentTerm.EndDate < Course.EndDate)
            //{
            //    IsValidInput = false;
            //    await Application.Current.MainPage.DisplayAlert("Course Dates Outside of Term", $"Term {DatabaseService.CurrentTerm.Name}" +
            //                $" starts on { DatabaseService.CurrentTerm.StartDate.Date} and ends on {DatabaseService.CurrentTerm.EndDate.Date}" +
            //                $" Courses within this term must be schedule within these dates", "Ok");
            //}

            //saves course if all validations return true
            if (IsValidInput && ValidString(CourseName) && ValidDates(CourseStartDate, CourseEndDate) && ValidString(InstructorName)
                && ValidEmail(Email) && ValidPhone(Phone) && ValidSelection(Status, StatusTitle, "Course Status"))
            {
                if (DatabaseService.IsAdd)
                {
                    await DatabaseService.AddCourse(Course, DatabaseService.CurrentTerm.Id);
                    //note that you changed this before and after breaking. May cause break. 
                    
                    await DatabaseService.AddInstructor(Instructor, DatabaseService.LastAddedId);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await DatabaseService.UpdateCourse(Course);
                    await DatabaseService.UpdateInstructor(Instructor);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
        }

        private async Task CancelCourse()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}



