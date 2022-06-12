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
    public class TermViewModel : INotifyPropertyChanged
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
        public ICommand AddTermCommand { get; set; }
        public ICommand EditTermCommand { get; set; }
        public ICommand ViewTermCommand { get; set; }
        public ICommand DeleteTermCommand { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
       

        public TermViewModel()
        {
            //Terms = new ObservableCollection<Term>();
            //Refresh();
            LoadSampleData();
            AddTermCommand = new Command(async () => await AddTerm());
            EditTermCommand = new Command(async (o) => await EditTerm(o));
            //ViewTermCommand = new Command(async (o) => await ViewTerm(o));
            DeleteTermCommand = new Command(async (o) => await DeleteTerm(o));
        }

        

        //methods
        private async Task AddTerm()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddTermsPage());
            //Refresh();
        }

        private async Task EditTerm(object o)
        {
            SelectedTerm = o as Term;

            await Application.Current.MainPage.Navigation.PushAsync(new EditTermsPage(SelectedTerm));
        }

        //private async Task ViewTerm(object o)
        //{
        //    throw new NotImplementedException();
        //}

        private async Task DeleteTerm(object o)
        {

            SelectedTerm = o as Term;
            //int id = SelectedTerm.Id;
            await DatabaseService.DeleteTerm(SelectedTerm.Id);
            Refresh();
        }

        private async void LoadSampleData()
        {
            IsBusy = true;
            //await Task.Delay(500);
            Terms = new ObservableCollection<Term>(await DatabaseService.GetTerm());
            IsBusy = false;
        }

        public async void Refresh()
        {
            IsBusy = true;
            //await Task.Delay(500);
            Terms.Clear();
            
            List<Term> TermList = await DatabaseService.GetTerm();

            foreach (Term term in TermList)
            {
                Terms.Add(term);
            }
            TermList.Clear();
            IsBusy = false;

            //List<Term> TermList = new List<Term>();

            //Task.Run(async () =>
            //{

            //TermList = await DatabaseService.GetTerm();
            //});
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
        }
    }
}
