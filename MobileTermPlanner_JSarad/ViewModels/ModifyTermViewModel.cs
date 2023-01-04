using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileTermPlanner_JSarad.Models;
using Xamarin.Forms;
using MobileTermPlanner_JSarad.Services;

namespace MobileTermPlanner_JSarad.ViewModels
{
    public class ModifyTermViewModel : BaseViewModel
    {
        //properties
        public bool IsValidInput;
        public string AddEdit { get; set; }
        public List<Term> TermList { get; set; }
        
        private Term _term;
        public Term Term
        {
            get
            {
                return _term;
            }
            set
            {
                _term = value;
                OnPropertyChanged();
            }
        }
        
        public string TermName
        {
            get
            {
                return _term.Name;
            }
            set
            {
                _term.Name = value;
                OnPropertyChanged();
                ValidString(TermName, "term name");
                EmptyErrorMessageOne = ValidationMessage;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _term.StartDate;
            }
            set
            {
                _term.StartDate = value;
                OnPropertyChanged();
                ValidDates(StartDate, EndDate);
                DatesErrorMessageOne = ValidationMessage;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _term.EndDate;
            }
            set
            {
                _term.EndDate = value;
                OnPropertyChanged();
                ValidDates(StartDate, EndDate);
                DatesErrorMessageOne = ValidationMessage;
            }
        }
        
        //commands
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        
        //constructor
        public ModifyTermViewModel()
        {
            
            if (DatabaseService.IsAdd)
            {
                AddEdit = "Add Term";
                Term = new Term();
                StartDate = DateTime.Now;
                EndDate = StartDate.AddDays(180);
            }
            else
            {
                AddEdit = "Edit Term";
                Term = DatabaseService.CurrentTerm;
            }
          
            SaveCommand = new Command(async () => await SaveTerm());
            CancelCommand = new Command(async () => await CancelTerm());
            Task.Run(async () => { await LoadTermList(); });

        }

        //methods Save/Cancel
        private async Task SaveTerm()
        {
            IsValidInput = true;

            //validates unchanged properties and displays any errors to user on save attempt
            ValidString(TermName, "term name");
            EmptyErrorMessageOne = ValidationMessage;

            if (!String.IsNullOrEmpty(TermName) && ValidDates(StartDate, EndDate))
            {
                if (TermList.Count > 0)
                {
                    //checks for overlapping Terms
                    foreach (Term term in TermList)
                    {
                        if (Term.Id != term.Id)
                        {
                            if ((Term.StartDate <= term.StartDate && Term.EndDate >= term.StartDate) ||
                                   (Term.StartDate <= term.EndDate && Term.EndDate >= term.EndDate)
                                   || (Term.StartDate >= term.StartDate && Term.EndDate <= term.EndDate))
                            {
                                IsValidInput = false;
                                await Application.Current.MainPage.DisplayAlert("Overlapping Course", $"There is an overlapping term for " +
                                    $"term { term.Name} from { term.StartDate.ToShortDateString()} to {term.EndDate.ToShortDateString()}", "Ok");
                                return;
                            }
                        }
                    }
                }
                //saves term if all validations pass
                if(IsValidInput)
                {
                    if (DatabaseService.IsAdd)
                    {
                        MessagingCenter.Send(this, "AddTerm", Term);
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        MessagingCenter.Send(this, "UpdateTerm", Term);
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
            }
            return;
        }

        private async Task CancelTerm()
        {
            MessagingCenter.Send(this, "Cancel");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        //load methods
        private async Task LoadTermList()
        {
            TermList = await DatabaseService.GetTerms();
        }
    }
}
