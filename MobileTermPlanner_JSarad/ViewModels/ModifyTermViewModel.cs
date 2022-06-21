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
        
        public string Name
        {
            get
            {
                return _modifyTerm.Name;
            }
            set
            {
                _modifyTerm.Name = value;
                OnPropertyChanged();
                ValidateString(Name, "Name");
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _modifyTerm.StartDate;
            }
            set
            {
                _modifyTerm.StartDate = value;
                OnPropertyChanged();
                ValidateDates(_modifyTerm.StartDate, _modifyTerm.EndDate);
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _modifyTerm.EndDate;
            }
            set
            {
                _modifyTerm.EndDate = value;
                OnPropertyChanged();
                ValidateDates(_modifyTerm.StartDate, _modifyTerm.EndDate);
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

        public bool IsValidInput;
        //public bool IsAdd;
        
        //commands
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        
        public ModifyTermViewModel()
        {
            if (DatabaseService.IsAdd) 
            {
                _modifyTerm = new Term();
                StartDate = DateTime.Now;
                EndDate = StartDate.AddDays(180);
            }
            else
            {
                _modifyTerm = DatabaseService.SelectedTerm;
            }
            
            SaveCommand = new Command(async () => await SaveTerm());
            CancelCommand = new Command(async () => await CancelTerm());
        }

        //methods
        private async Task SaveTerm()
        {
            TermList = await DatabaseService.GetTerm();
            IsValidInput = true;

            if (TermList.Count > 0)
            {
                for (int i = 0; i < TermList.Count; i++)
                {
                    if (((TermList[i].StartDate <= ModifyTerm.EndDate && TermList[i].StartDate >= ModifyTerm.StartDate) ||
                        (TermList[i].EndDate <= ModifyTerm.EndDate && TermList[i].EndDate >= ModifyTerm.StartDate)) && (TermList[i].Id != ModifyTerm.Id))
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
            if (IsValidInput && ValidateString(Name, "Name") && ValidateDates(StartDate, EndDate))
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
