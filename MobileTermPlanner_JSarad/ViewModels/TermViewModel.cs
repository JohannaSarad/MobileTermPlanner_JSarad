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

        //commands
        public ICommand NavToAddCommand { get; set; }
        public ICommand NavToEditCommand { get; set; }
        public ICommand ViewTermCommand { get; set; }
        public ICommand DeleteTermCommand { get; set; }

        //constructor
        public TermViewModel()
        {
            LoadTerms();
            
            NavToAddCommand = new Command(async () => await NavToAddTerm());
            NavToEditCommand = new Command(async (o) => await NavToEditTerm(o));
            ViewTermCommand = new Command(async (o) => await ViewTerm(o));
            DeleteTermCommand = new Command(async (o) => await DeleteTerm(o));

            MessagingCenter.Subscribe<ModifyTermViewModel, Term>(this, "AddTerm", (sender, term) =>
            {
                AddTerm(term);
            });

            MessagingCenter.Subscribe<ModifyTermViewModel, Term>(this, "UpdateTerm", (sender, term) =>
            {
                UpdateTerm(term);
            });
        }
            

        // navigation methods
        private async Task NavToAddTerm()
        {
            DatabaseService.IsAdd = true;
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyTermsPage());
        }

        private async Task NavToEditTerm(object o)
        {
            DatabaseService.IsAdd = false;
            DatabaseService.CurrentTerm = o as Term;
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyTermsPage());
        }

        //modify and view methods
        private async Task ViewTerm(object o)
        {
            DatabaseService.CurrentTerm = o as Term;
            await Application.Current.MainPage.Navigation.PushAsync(new CourseViewPage());
        }

        private async void AddTerm(Term term)
        {
            await DatabaseService.AddTerm(term);
            LoadTerms();
        }

        private async void UpdateTerm(Term term)
        {
            await DatabaseService.UpdateTerm(term);
            LoadTerms();
        }

        private async Task DeleteTerm(object o)
        {
            Term term = o as Term;
            await DatabaseService.DeleteTerm(term.Id);
            LoadTerms();
           
        }

        //load methods
        public async void LoadTerms()
        {
            if (Terms != null)
            {
                Terms.Clear();

                List<Term> termList = new List<Term>(await DatabaseService.GetTerms());
                foreach (Term term in termList)
                {
                    Terms.Add(term);
                }
            }
            else
            {
                Terms = new ObservableCollection<Term>(await DatabaseService.GetTerms());
            }
        }
    }
}
