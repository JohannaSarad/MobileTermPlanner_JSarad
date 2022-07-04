using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MobileTermPlanner_JSarad.Services;
using MobileTermPlanner_JSarad.Models;
using Xamarin.Forms;
using System.Threading.Tasks;

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

        public ModifyNotesViewModel()
        {
            LoadNote();
            SaveCommand = new Command(async () => await SaveNote());
            CancelCommand = new Command(async () => await CancelNote());
        }

        private async void LoadNote()
        {
            Note = await DatabaseService.GetNotesByCourse(DatabaseService.CurrentCourse.Id);
            if (Note == null || string.IsNullOrEmpty(NoteText))
            {
                NoteText = "There are no notes for this course";
            }
        }

        private async Task SaveNote()
        {
            //this doesn't work right. Either need to only edit or have the option to not have sample notes in DatabaseService AddNotes Method
            await DatabaseService.UpdateNotes(Note);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task CancelNote()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
