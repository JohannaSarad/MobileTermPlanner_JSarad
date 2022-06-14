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
        public List<Term> TermList { get; set; }
        
        private Term _modifyTerm;
        public Term ModifyTerm
        {
            get
            {
                return _modifyTerm;
            }
            set
            {
                _modifyTerm = value;
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
        public ICommand SaveTermCommand { get; set; }
        public ICommand CancelTermCommand { get; set; }
        
        public ModifyTermViewModel()
        {
            
            if (DatabaseService.IsAdd) 
            {
                _modifyTerm = new Term();
            }
            else
            {
                _modifyTerm = DatabaseService.SelectedTerm;
            }
            
            SaveTermCommand = new Command(async () => await SaveTerm());
            CancelTermCommand = new Command(async () => await CancelTerm());
        }

        //methods
        private async Task SaveTerm()
        {
            TermList = await DatabaseService.GetTerm();
            IsValidInput = true;
            if (ModifyTerm.Name == null)
            {
                IsValidInput = false;
                InvalidNameMessage = "* Name is Required";
            }
            else
            {
                InvalidNameMessage = "";
            }
            if (ModifyTerm.StartDate > ModifyTerm.EndDate || ModifyTerm.StartDate.Date == ModifyTerm.EndDate.Date)
            {
                IsValidInput = false;
                InvalidDateMessage = "* Term start date must be before term end date";
            }
            else
            {
                InvalidDateMessage = "";
            }

            if (TermList.Count > 0)
            {
                //FIX ME!!! This statement also needs to check if the term being checked is the term currently being worked on. 
                int i;
                for (i = 0; i < TermList.Count; i++)
                {
                    if ((TermList[i].StartDate <= ModifyTerm.EndDate && TermList[i].StartDate >= ModifyTerm.StartDate) ||
                        (TermList[i].EndDate <= ModifyTerm.EndDate && TermList[i].EndDate >= ModifyTerm.StartDate))
                    {
                        IsValidInput = false;
                        OverlapMessage = $"* There is an overlapping term for these dates for Term {TermList[i].Name} from {TermList[i].StartDate.Date} to " +
                            $"{TermList[i].EndDate.Date}";
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
                    MessagingCenter.Send(this, "AddTerm", ModifyTerm);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    MessagingCenter.Send(this, "EditTerm", ModifyTerm);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
        }

        private async Task CancelTerm()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
