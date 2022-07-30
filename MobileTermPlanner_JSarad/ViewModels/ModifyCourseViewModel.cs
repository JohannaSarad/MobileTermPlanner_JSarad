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
    //FIX ME!!! Notifications Label starts as empty in edit course but not in add course something missing in the constructor when editing
    public class ModifyCourseViewModel : BaseViewModel
    {
        //properties

        //sets Title to "Add Course" or "Edit Course"
        public string AddEdit { get; set; }
        public bool IsValidInput;
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
                ValidString(CourseName, "course name");
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
                ValidSelection(Status, "course status");
                SelectionErrorMessage = ValidationMessage;
            }
        }

        public string Notes
        {
            get
            {
                return _course.Notes;
            }
            set
            {
                _course.Notes = value;
                OnPropertyChanged();
                ValidCharacters(Notes);
                CharacterErrorMessage = ValidationMessage;
            }
        }

        public bool Notify
        {
            get
            {
                return _course.Notify;
            }
            set
            {
                _course.Notify = value;
                OnPropertyChanged();
                UpdateNotifyLabel(Notify, "Course");
                NotifyLabel = ValidationMessage;

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
                ValidString(InstructorName, "instructor name");
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
        
        //commands
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        //constructor
        public ModifyCourseViewModel()
        {
            Task.Run(async () => { await LoadCourseList(); });
            if (DatabaseService.IsAdd)
            {
                AddEdit = "Add Course";
                Course = new Course();
                CourseStartDate = DatabaseService.CurrentTerm.StartDate;
                CourseEndDate = CourseStartDate.AddDays(30);
                Notify = false;  
                Instructor = new Instructor();
            }
            else
            {
                AddEdit = "Edit Course";
                Course = DatabaseService.CurrentCourse;
                Instructor = DatabaseService.CurrentInstructor;
            }

            SaveCommand = new Command(async () => await SaveCourse());
            CancelCommand = new Command(async () => await CancelCourse());
        }

        //methods Save/Cancel
        private async Task SaveCourse()
        {
            
            IsValidInput = true;

            //validates unchanged properties and displays any errors to user on save attempt
            ValidString(CourseName, "course name");
            EmptyErrorMessageOne = ValidationMessage;
            ValidString(InstructorName, "instructor name");
            EmptyErrorMessageTwo = ValidationMessage;
            ValidEmail(Email);
            EmailErrorMessage = ValidationMessage;
            ValidPhone(Phone);
            PhoneErrorMessage = ValidationMessage;
            ValidSelection(Status, "course status");
            SelectionErrorMessage = ValidationMessage;

            //saves course if all validations return true
            //if (IsValidInput && ValidString(CourseName, "course name") && ValidDates(CourseStartDate, CourseEndDate) && ValidString(InstructorName, "instructor name")
            //    && ValidEmail(Email) && ValidPhone(Phone) && ValidSelection(Status, "course status") 
            //    && (Notes == null || (Notes != null && ValidCharacters(Notes))))

            if (!string.IsNullOrEmpty(CourseName) && !string.IsNullOrEmpty(InstructorName) && !string.IsNullOrEmpty(Status)
                && ValidEmail(Email) && ValidPhone(Phone) && ValidDates(CourseStartDate, CourseEndDate)
                && (Notes == null || Notes != null && ValidCharacters(Notes)))
            {
                //checks for overlapping courses 
                if(CourseList.Count > 0)
                {
                    foreach (Course course in CourseList)
                    {
                        if (Course.Id != course.Id)
                        {
                            if ((Course.StartDate <= course.StartDate && Course.EndDate >= course.StartDate)
                            || (Course.StartDate <= course.EndDate && Course.EndDate >= course.EndDate)
                            || (Course.StartDate >= course.StartDate && Course.EndDate <= course.EndDate))
                            {
                                IsValidInput = false;
                                await Application.Current.MainPage.DisplayAlert($"Overlapping Course", $"There is an overlapping course for " +
                                    $"course {course.Name} from {course.StartDate.ToShortDateString()} to {course.EndDate.ToShortDateString()}", "Ok");
                                return;
                            }
                        }
                    }
                }
                /*checks that course dates are within the dates of the respective term (will not affect course dates if term dates are 
                * altered, but will require dates of courses to be changed if an course is being edited which has become outside of term date 
                * ranges after term modification)*/
                if (DatabaseService.CurrentTerm.StartDate > Course.StartDate || DatabaseService.CurrentTerm.StartDate > Course.EndDate
                    || DatabaseService.CurrentTerm.EndDate < Course.StartDate || DatabaseService.CurrentTerm.EndDate < Course.EndDate)
                {
                    IsValidInput = false;
                    await Application.Current.MainPage.DisplayAlert("Dates Out Of Range", $"course dates must be scheduled durring the" +
                        $" term {DatabaseService.CurrentTerm.Name} starting on { DatabaseService.CurrentTerm.StartDate.ToShortDateString()} and ending " +
                        $"on {DatabaseService.CurrentTerm.EndDate.ToShortDateString()}", "Ok");
                    return;
                }
                if(IsValidInput)
                {
                    if (DatabaseService.IsAdd)
                    {
                        await DatabaseService.AddCourse(Course, DatabaseService.CurrentTerm.Id);
                        MessagingCenter.Send(this, "AddInstructor", Instructor);
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        await DatabaseService.UpdateCourse(Course);
                        MessagingCenter.Send(this, "UpdateInstructor", Instructor);
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
            }
        }

        private async Task CancelCourse()
        {
            MessagingCenter.Send(this, "Cancel");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        //load methods
        private async Task LoadCourseList()
        {
            CourseList = await DatabaseService.GetCoursesByTerm(DatabaseService.CurrentTerm.Id);
        }
    }
}



