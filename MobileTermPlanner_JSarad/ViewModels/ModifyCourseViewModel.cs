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

        //course properties
        private Course _modifyCourse;
        public Course ModifyCourse
        {
            get
            {
                return _modifyCourse;
            }
            set
            {
                _modifyCourse = value;
                OnPropertyChanged();
            }
        }

        public string CourseName
        {
            get
            {
                return _modifyCourse.Name;
            }
            set
            {
                _modifyCourse.Name = value;
                OnPropertyChanged();
                ValidString(CourseName);
                EmptyErrorMessageOne = ValidationMessage;
                //ValidateString(CourseName, "Name");

            }
        }

        public DateTime CourseStartDate
        {
            get
            {
                return _modifyCourse.StartDate;
            }
            set
            {
                _modifyCourse.StartDate = value;
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
                return _modifyCourse.EndDate;
            }
            set
            {
                _modifyCourse.EndDate = value;
                OnPropertyChanged();
                ValidDates(CourseStartDate, CourseEndDate);
                DatesErrorMessageOne = ValidationMessage;
                //ValidateDates(_modifyCourse.StartDate, _modifyCourse.EndDate);
            }
        }

        //instructor properties
        private Instructor _modifyInstructor;
        public Instructor ModifyInstructor
        {
            get
            {
                return _modifyInstructor;
            }
            set
            {
                _modifyInstructor = value;
                OnPropertyChanged();
            }
        }

        public string InstructorName
        {
            get
            {
                return _modifyInstructor.Name;
            }
            set
            {
                _modifyInstructor.Name = value;
                OnPropertyChanged();
                ValidString(InstructorName);
                EmptyErrorMessageTwo = ValidationMessage;
                //ValidateString(InstructorName, "Name");
            }
        }

        public string Email
        {
            get
            {
                return _modifyInstructor.Email;
            }
            set
            {
                _modifyInstructor.Email = value;
                OnPropertyChanged();
                ValidEmail(Email);
                EmailErrorMessage = ValidationMessage;
                //ValidateEmail(Email);
            }
        }

        public string Phone
        {
            get
            {
                return _modifyInstructor.Phone;
            }
            set
            {
                _modifyInstructor.Phone = value;
                OnPropertyChanged();
                ValidPhone(Phone);
                PhoneErrorMessage = ValidationMessage;
                //ValidatePhone(Phone);
            }
        }

        
        private string _overlapMesssage;
        public string OverlapMessage
        {
            get
            {
                return _overlapMesssage;
            }
            set
            {
                _overlapMesssage = value;
                OnPropertyChanged();
            }
        }

        //public string AddEdit { get; set; }
        public bool IsValidInput;

        //commands
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        //public ICommand NavToModifyAssessmentsCommand { get; set; }
        //public ICommand NavToModifyNotesCommand { get; set; }

        public ModifyCourseViewModel()
        {
            if (DatabaseService.IsAdd)
            {
                //AddEdit = "Add Course";
                _modifyCourse = new Course();
                CourseStartDate = DatabaseService.SelectedTerm.StartDate;
                CourseEndDate = CourseStartDate.AddDays(30);
                _modifyInstructor = new Instructor();
            }
            else
            {
                //AddEdit = "Edit Course";
                _modifyCourse = DatabaseService.SelectedCourse;
                //GetInstructor(ModifyCourse.Id);
                _modifyInstructor = DatabaseService.SelectedInstructor;
            }

            SaveCommand = new Command(async () => await SaveCourse());
            CancelCommand = new Command(async () => await CancelCourse());
        }

        //methods
        
        //private async void GetInstructor(int id)
        //{
        //    _modifyInstructor = await DatabaseService.GetInstuctorByCourse(id);
        //}
        private async Task SaveCourse()
        {
            CourseList = await DatabaseService.GetCourseByTerm(DatabaseService.SelectedTerm.Id);
            IsValidInput = true;
           
            if (CourseList.Count > 0)
            {
                int i;
                for (i = 0; i < CourseList.Count; i++)
                {
                    if (((CourseList[i].StartDate <= ModifyCourse.EndDate && CourseList[i].StartDate >= ModifyCourse.StartDate) ||
                        (CourseList[i].EndDate <= ModifyCourse.EndDate && CourseList[i].EndDate >= ModifyCourse.StartDate)) && (CourseList[i].Id != ModifyCourse.Id))
                    {
                        IsValidInput = false;
                        OverlapMessage = $"* There is an overlapping term for these dates for Term {CourseList[i].Name} from {CourseList[i].StartDate.Date} to " +
                            $"{CourseList[i].EndDate.Date}";
                    }
                    else
                    {
                        OverlapMessage = "";
                    }
                }
            }
            //if (IsValidInput && ValidateDates(CourseStartDate, CourseEndDate) && ValidateString(CourseName, "Course Name") && 
            //    ValidateString(InstructorName, "Instructor Name") && ValidateEmail(Email) && ValidatePhone(Phone))
            if (IsValidInput && ValidString(CourseName) && ValidDates(CourseStartDate, CourseEndDate) && ValidString(InstructorName)
                && ValidEmail(Email) && ValidPhone(Phone))
            {
                if (DatabaseService.IsAdd)
                {
                    MessagingCenter.Send(this, "AddCourse", ModifyCourse);
                    await DatabaseService.AddInstructor(ModifyInstructor, ModifyCourse.Id);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    MessagingCenter.Send(this, "EditCourse", ModifyCourse);
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

    

