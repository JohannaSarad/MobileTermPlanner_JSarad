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
    public class AddTermViewModel : TermViewModel
    {
        //properties
        private Term _termToAdd;
        public Term TermToAdd
        {
            get
            {
                return _termToAdd;
            }
            set
            {
                _termToAdd = value;
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


        //commands
        public ICommand SaveTermCommand { get; set; }
        public ICommand CancelTermCommand { get; set; }
        
        public AddTermViewModel()
        {
            _termToAdd = new Term();
            SaveTermCommand = new Command(async () => await SaveTerm());
            CancelTermCommand = new Command(async () => await CancelTerm());
        }
        
        //methods
        private async Task SaveTerm()
        {
            //Term term = new Term
            //{
            //    Name = o.Name,
            //    StartDate = o.StartDate,
            //    EndDate = o.EndDate
            //};
            IsValidInput = true;
            if (TermToAdd.Name == null)
            {
                IsValidInput = false;
                InvalidNameMessage = "* Name is Required";
            }
            if (TermToAdd.StartDate > TermToAdd.EndDate || TermToAdd.StartDate.Date == TermToAdd.EndDate.Date)
            {
                IsValidInput = false;
                InvalidDateMessage = "* Term start date must be before term end date";
            }
            if (Terms.Count > 0 )
            {
                int i;
                for (i = 0; i < Terms.Count; i++) {
                    if ((Terms[i].StartDate <= TermToAdd.EndDate && Terms[i].StartDate >= TermToAdd.StartDate) || 
                        (Terms[i].EndDate <= TermToAdd.EndDate && Terms[i].EndDate >= TermToAdd.StartDate))
                    {
                        IsValidInput = false;
                        OverlapMessage = ($"* There is an overlapping term for these dates for Term {Terms[i].Name} from {Terms[i].StartDate.Date} to " +
                            $"{Terms[i].EndDate.Date}");
                    }
                }
            }
            if (IsValidInput == true)
            {
                await DatabaseService.AddTerm(TermToAdd);
                Refresh();
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                Refresh();
            }
         }

        private async Task CancelTerm()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
