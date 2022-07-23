using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MobileTermPlanner_JSarad.Services;
using MobileTermPlanner_JSarad.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MobileTermPlanner_JSarad.ViewModels
{
    public class ModifyNotesViewModel : BaseViewModel
    {
        private Notes _note;
        public Notes Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }

        public string NoteText
        {
            get
            {
                return _note.Note;
            }
            set
            {
                _note.Note = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ShareCommand { get; set; }

        public ModifyNotesViewModel()
        {
            if (DatabaseService.IsAdd)
            {
                Note = new Notes();
            }
            else
            {
                LoadNote();
            }
            SaveCommand = new Command(async () => await SaveNote());
            CancelCommand = new Command(async () => await CancelNote());
            ShareCommand = new Command(async () => await ShareNote());
        }

        private async Task ShareNote()
        {
            MessagingCenter.Send(this, "UpdateNotes", Note);
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = NoteText,
                Title = $"Notes for {DatabaseService.CurrentCourse.Name}"
            });
        }

        private async void LoadNote()
        {
            Note = await DatabaseService.GetNotesByCourse(DatabaseService.CurrentCourse.Id);
        }

        private async Task SaveNote()
        {
            if (DatabaseService.IsAdd)
            {
                MessagingCenter.Send(this, "AddNotes", Note);
            }
            else
            {
                MessagingCenter.Send(this, "UpdateNotes", Note);
            }
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task CancelNote()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
