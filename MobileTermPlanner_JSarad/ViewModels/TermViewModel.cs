using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using MobileTermPlanner_JSarad.Models;
using SQLite;
using MobileTermPlanner_JSarad.Services;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using MobileTermPlanner_JSarad.Views;

namespace MobileTermPlanner_JSarad.ViewModels
{
    public class TermViewModel : BaseViewModel
    {
        //properties
        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Term> _terms;
        public ObservableCollection<Term> Terms
        {
            get
            {
                return _terms;
            }
            set
            {
                _terms = value;
                OnPropertyChanged();
            }
        }

        private Term _selectedTerm;
        public Term SelectedTerm
        {
            get
            {
                return _selectedTerm;
            }
            set
            {
                _selectedTerm = value;
                OnPropertyChanged();
            }
        }

        //commands
        public ICommand NavToAddCommand { get; set; }
        public ICommand NavToEditCommand { get; set; }
        public ICommand ViewTermCommand { get; set; }
        public ICommand DeleteTermCommand { get; set; }

        public TermViewModel()
        {
            LoadTerms();
            
            NavToAddCommand = new Command(async () => await NavToAddTerm());
            NavToEditCommand = new Command(async (o) => await NavToEditTerm(o));
            ViewTermCommand = new Command(async (o) => await ViewTerm(o));
            DeleteTermCommand = new Command(async (o) => await DeleteTerm(o));

            //added 6/14 untested
            MessagingCenter.Subscribe<ModifyTermViewModel, Term>(this, "AddTerm", (sender, obj) =>
            {
                AddTerm(obj);
            });

            MessagingCenter.Subscribe<ModifyTermViewModel, Term>(this, "EditTerm", (sender, obj) =>
            {
                UpdateTerm(obj);
            });
        }

        //methods
        private async Task NavToAddTerm()
        {
            DatabaseService.IsAdd = true;
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyTermsPage());
        }

        private async Task NavToEditTerm(object o)
        {
            DatabaseService.IsAdd = false;
            DatabaseService.SelectedTerm = o as Term;
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyTermsPage());
        }

        private async Task ViewTerm(object o)
        {
            DatabaseService.SelectedTerm = o as Term;
            await Application.Current.MainPage.Navigation.PushAsync(new CourseViewPage());
        }

        private async void AddTerm(Term term)
        {
            //SelectedTerm = term;
            await DatabaseService.AddTerm(term);
            Refresh();
        }

        private async void UpdateTerm(Term term)
        {
            //SelectedTerm = term;
            await DatabaseService.UpdateTerm(term);
            Refresh();
        }
        private async Task DeleteTerm(object o)
        {
            SelectedTerm = o as Term;
            await DatabaseService.DeleteTerm(SelectedTerm.Id);
            Refresh();
        }

        private async void LoadTerms()
        {
            IsBusy = true;
            Terms = new ObservableCollection<Term>(await DatabaseService.GetTerm());
            IsBusy = false;
        }

        public void Refresh()
        {
            IsBusy = true;
            Terms.Clear();
            //Terms = new ObservableCollection<Term>(await DatabaseService.GetTerm());
            LoadTerms();
            IsBusy = false;
        }
    }
}
