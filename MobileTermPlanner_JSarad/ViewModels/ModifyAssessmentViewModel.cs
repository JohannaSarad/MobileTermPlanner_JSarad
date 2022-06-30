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
    public class ModifyAssessmentViewModel : BaseViewModel
    {
        public List<Assessment> AssessmentList { get; set; }
        public List<string> TypeList { get; } = new List<string> { "Objective", "Performance" };
        public string TypeTitle { get; } = "Select Assessment Type";

        private Assessment _assessment;
        public Assessment Assessment
        {
            get
            {
                return _assessment;
            }
            set
            {
                _assessment = value; 
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _assessment.Name;
            }
            set
            {
                _assessment.Name = value;
                OnPropertyChanged();
                ValidString(Name);
                EmptyErrorMessageOne = ValidationMessage;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _assessment.StartDate;
            }
            set
            {
                _assessment.StartDate = value;
                OnPropertyChanged();
                ValidDates(StartDate, EndDate);
                DatesErrorMessageOne = ValidationMessage;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _assessment.EndDate;
            }
            set
            {
                _assessment.EndDate = value;
                OnPropertyChanged();
                ValidDates(StartDate, EndDate);
                DatesErrorMessageOne = ValidationMessage;
            }
        }

        public string Type
        {
            get
            {
                return _assessment.Type;
            }
            set
            {
                _assessment.Type = value;
                OnPropertyChanged();
                ValidSelection(Type, TypeTitle, "Assessment Type");
                SelectionErrorMessage = ValidationMessage;
            }
        }

        public bool IsValidInput;

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ModifyAssessmentViewModel()
        {
            if (DatabaseService.IsAdd)
            {
                Assessment = new Assessment();
                StartDate = DateTime.Now;
                EndDate = StartDate.AddDays(180);
            }
            else
            {
                Assessment = DatabaseService.CurrentAssessment;
            }

            SaveCommand = new Command(async () => await SaveAssessment());
            CancelCommand = new Command(async () => await CancelAssessment());
        }

        private async Task SaveAssessment()
        {
            AssessmentList = await DatabaseService.GetAssessmentsByCourse(DatabaseService.CurrentCourse.Id);
            IsValidInput = true;

            ValidString(Name);
            EmptyErrorMessageOne = ValidationMessage;
            ValidSelection(Type, TypeTitle, "Assessment Type");
            SelectionErrorMessage = ValidationMessage;
            
            if (AssessmentList.Count > 0)
            {
                //verifies assessment types are not the same
                foreach(Assessment assessment in AssessmentList)
                {
                    if ((assessment.Type == Type) && (assessment.Id != DatabaseService.CurrentAssessment.Id))
                    {
                        IsValidInput = false;
                        await Application.Current.MainPage.DisplayAlert($"There may only be 1 {Type} Assessment per Course", $"There is already one {Type} Assessment for this " +
                            $"course for {assessment.Type} Assessment {assessment.Name} from {assessment.StartDate.Date} to {assessment.EndDate.Date} " +
                            $"Please select a different Assessment Type to continue.", "Ok");
                    }
                }
            }
           
            ////checks that new or edited Assessment dates are within the dates of the Course (for future use)
            //if (DatabaseService.CurrentCourse.StartDate > Assessment.StartDate || DatabaseService.CurrentCourse.StartDate > Term.EndDate
            //    || DatabaseService.CurrentCourse.EndDate < Assessment.StartDate || DatabaseService.CurrentCourse.EndDate < Assessment.EndDate)
            //{
            //    IsValidInput = false;
            //    await Application.Current.MainPage.DisplayAlert("Course Dates Outside of Course", $"Course {DatabaseService.CurrentCourse.Name}" +
            //                $" starts on { DatabaseService.CurrentCourse.StartDate.Date} and ends on {DatabaseService.CurrentCourse.EndDate.Date}" +
            //                $" Assessments within this Course must be schedule within these dates", "Ok");
            //}
            
            //saves course if all validations return true
            if (IsValidInput && ValidString(Name) && ValidDates(StartDate, EndDate) && ValidSelection(Type, TypeTitle, "Course Status"))
            {
                if (DatabaseService.IsAdd)
                {
                    await DatabaseService.AddAssessment(Assessment, DatabaseService.CurrentCourse.Id);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await DatabaseService.UpdateAssessment(Assessment);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
        }

        private async Task CancelAssessment()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
