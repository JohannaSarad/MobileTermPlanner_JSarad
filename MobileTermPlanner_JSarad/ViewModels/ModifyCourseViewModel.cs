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
    class ModifyCourseViewModel : BaseViewModel
    {
        public List<Course> CourseList { get; set; }

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

        private string _invalidNameMessage;
        public string InvalidNameMessage
        {
            get
            {
                return _invalidNameMessage;
            }
            set
            {
                _invalidNameMessage = value;
                OnPropertyChanged();
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

        private string _invalidDateMessage;
        public string InvalidDateMessage
        {
            get
            {
                return _invalidDateMessage;
            }
            set
            {
                _invalidDateMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsValidInput;
        //public bool IsAdd;

        //commands
        public ICommand SaveCourseCommand { get; set; }
        public ICommand CancelCourseCommand { get; set; }
        public ICommand NavToModifyAssessmentsCommand { get; set; }
        public ICommand NavToModifyNotesCommand { get; set; }

        public ModifyCourseViewModel()
        {

            if (DatabaseService.IsAdd)
            {
                _modifyCourse = new Course();
                _modifyInstructor = new Instructor();
                
            }
            else
            {
                _modifyCourse = DatabaseService.SelectedCourse;
            }

            SaveCourseCommand = new Command(async () => await SaveCourse());
            CancelCourseCommand = new Command(async () => await CancelCourse());
        }

        //methods
        private async Task SaveCourse()
        {
            CourseList = await DatabaseService.GetCourseByTerm(DatabaseService.SelectedTerm.Id);

            IsValidInput = true;
            if (ModifyCourse.Name == null)
            {
                IsValidInput = false;
                InvalidNameMessage = "* Name is Required";
            }
            else
            {
                InvalidNameMessage = "";
            }
            if (ModifyInstructor.Name == null)
            {
                IsValidInput = false;
                InvalidNameMessage = "* Name is Required";
            }
            else
            {
                InvalidNameMessage = "";
            }
            if (ModifyCourse.StartDate.Date >= ModifyCourse.EndDate.Date)
            {
                IsValidInput = false;
                InvalidDateMessage = "* Course start date must be before term end date";
            }
            else
            {
                InvalidDateMessage = "";
            }

            if (CourseList.Count > 0)
            {
                //FIX ME!!! This statement also needs to check if the term being checked is the term currently being worked on. 
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
            if (IsValidInput == true)
            {
                if (DatabaseService.IsAdd)
                {
                    MessagingCenter.Send(this, "AddCourse", ModifyCourse);
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

    

