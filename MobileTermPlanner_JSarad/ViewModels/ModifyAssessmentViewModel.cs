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
        //properties
        public string AddEdit { get; set; }
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
                ValidString(Name, "assessment name");
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
                ValidSelection(Type, "assessment type");
                SelectionErrorMessage = ValidationMessage;
            }
        }

        public bool Notify
        {
            get
            {
                return _assessment.Notify;
            }
            set
            {
                _assessment.Notify = value;
                OnPropertyChanged();
                UpdateNotifyLabel(Notify, "Assessment");
                NotifyLabel = ValidationMessage;

            }
        }
        
        public bool IsValidInput;

        //commands
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        
        //constructor
        public ModifyAssessmentViewModel()
        {
            Task.Run(async () => { await LoadAssessmentList(); });
            if (DatabaseService.IsAdd)
            {
                AddEdit = "Add Assessment";
                Assessment = new Assessment();
                StartDate = DatabaseService.CurrentCourse.StartDate;
                EndDate = StartDate.AddDays(14);
                Notify = false;
            }
            else
            {
                AddEdit = "Edit Assessment";
                Assessment = DatabaseService.CurrentAssessment;
            }

            SaveCommand = new Command(async () => await SaveAssessment());
            CancelCommand = new Command(async () => await CancelAssessment());
        }

        //methods Save/Cancel
        private async Task SaveAssessment()
        {

            IsValidInput = true;

            ValidString(Name, "assessment name");
            EmptyErrorMessageOne = ValidationMessage;
            ValidSelection(Type, "assessment type");
            SelectionErrorMessage = ValidationMessage;

            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Type) && ValidDates(StartDate, EndDate))
            {
                if (AssessmentList.Count > 0)
                {
                    foreach (Assessment assessment in AssessmentList)
                    {
                        if (Assessment.Id != assessment.Id)
                        {
                            //verifies assessment types are not the same
                            if (assessment.Type == Type)
                            {
                                IsValidInput = false;
                                await Application.Current.MainPage.DisplayAlert($"Duplicate Assessment Type", $"There is already one {Type} Assessment for " +
                                    $"this course \n\n {assessment.Type} Assessment {assessment.Name} from {assessment.StartDate.ToShortDateString()} " +
                                    $"to {assessment.EndDate.ToShortDateString()} ", "Ok");
                                return;
                            }
                            //checks for overlapping assessment dates
                            else if ((Assessment.StartDate <= assessment.StartDate && Assessment.EndDate >= assessment.StartDate) ||
                                (Assessment.StartDate <= assessment.EndDate && Assessment.EndDate >= assessment.EndDate)
                                || (Assessment.StartDate >= assessment.StartDate && Assessment.EndDate <= assessment.EndDate))
                            {
                                IsValidInput = false;
                                await Application.Current.MainPage.DisplayAlert($"Overlapping Assessment", $"There is an overlapping assessment for " +
                                    $"assessment {assessment.Name} from { assessment.StartDate.ToShortDateString()} to {assessment.EndDate.ToShortDateString()}", "Ok");
                                return;
                            }
                        }
                    }
                }
                /*checks assessment dates are within the dates of the respective course (will not affect assessment dates if course dates are 
                 * altered, will require dates of assessment to be changed if an assessment is being edited which has become outside of course date 
                 * ranges after course modification)*/
                if (DatabaseService.CurrentCourse.StartDate > Assessment.StartDate || DatabaseService.CurrentCourse.StartDate > Assessment.EndDate
                    || DatabaseService.CurrentCourse.EndDate < Assessment.StartDate || DatabaseService.CurrentCourse.EndDate < Assessment.EndDate)
                {
                    IsValidInput = false;
                    await Application.Current.MainPage.DisplayAlert("Dates Out Of Range", $"assessment dates must be scheduled during the " +
                        $"course {DatabaseService.CurrentCourse.Name} starting on { DatabaseService.CurrentCourse.StartDate.ToShortDateString()} and ending " +
                        $"on {DatabaseService.CurrentCourse.EndDate.ToShortDateString()}", "Ok");
                    return;
                }

                if (IsValidInput)
                {
                    if (DatabaseService.IsAdd) 
                    { 
                    MessagingCenter.Send(this, "AddAssessment", Assessment);
                    await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        MessagingCenter.Send(this, "UpdateAssessment", Assessment);
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
               
            }
        }

        private async Task CancelAssessment()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task LoadAssessmentList()
        {
            MessagingCenter.Send(this, "Cancel");
            AssessmentList = await DatabaseService.GetAssessmentsByCourse(DatabaseService.CurrentCourse.Id);
        }
    }
}
