using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileTermPlanner_JSarad.Models;
using Xamarin.Forms;
using MobileTermPlanner_JSarad.Services;

namespace MobileTermPlanner_JSarad.ViewModels
{
    public class EditTermViewModel : TermViewModel
    {
        //properties
        private Term _modifiedTerm;
        public Term ModifiedTerm
        {
            get
            {
                return _modifiedTerm;
            }
            set
            {
                _modifiedTerm = value;
                OnPropertyChanged();
            }
        }

        //commands
        public ICommand SaveTermCommand { get; set; }
        public ICommand CancelTermCommand { get; set; }
        
        public EditTermViewModel(Term term)
        {
            _modifiedTerm = term;
            SaveTermCommand = new Command(async () => await SaveTerm());
            CancelTermCommand = new Command(async () => await CancelTerm());
        }

        private async Task CancelTerm()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task SaveTerm()
        {
            await DatabaseService.UpdateTerm(ModifiedTerm);
            Refresh();
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        
    }
}
