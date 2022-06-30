using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileTermPlanner_JSarad.Models;
using MobileTermPlanner_JSarad.Services;
using MobileTermPlanner_JSarad.Views;
using Xamarin.Forms;

namespace MobileTermPlanner_JSarad.ViewModels
{
    class DetailedCourseViewModel : BaseViewModel
    {
        private List<Assessment> AssessmentList {get; set;}
        private ObservableCollection<Assessment> _assessments;
        public ObservableCollection<Assessment> Assessments
        {
            get
            {
                return _assessments;
            }
            set
            {
                _assessments = value;
                OnPropertyChanged();
            }
        }

        private Course _course;
        public Course Course
        {
            get
            {
                return _course;
            }
            set
            {
                _course = value;
                OnPropertyChanged();
            }
        }

        public string CourseNotes
        {
            get
            {
                return _course.Notes;
            }
            set
            {
                _course.Notes = value;
                OnPropertyChanged();
            }
        }

        private Instructor _instructor;
        public Instructor Instructor
        {
            get
            {
                return _instructor;
            }
            set
            {
                _instructor = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavToEditCourseCommand { get; set; }
        public ICommand NavToAddAssessmentCommand { get; set; }
        public ICommand NavToEditAssessmentCommand { get; set; }
        public ICommand DeleteAssessmentCommand { get; set; }
        public ICommand NavToNotesCommand { get; set; }

        public DetailedCourseViewModel()
        {
            Course = DatabaseService.CurrentCourse;
            Instructor = DatabaseService.CurrentInstructor;
            LoadAssessments();

            NavToEditCourseCommand = new Command(async () => await NavToEditCourse());
            NavToAddAssessmentCommand = new Command(async () => await NavToAddAssessment());
            NavToEditAssessmentCommand = new Command(async (o) => await NavToEditAssessment(o));
            DeleteAssessmentCommand = new Command(async (o) => await DeleteAssessment(o));
        }

        private async Task NavToEditCourse()
        {
            DatabaseService.IsAdd = false;
            DatabaseService.CurrentCourse = Course;
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyCoursePage());
        }
        private async Task NavToAddAssessment()
        {
            //verify there are 2 or fewer assessments associated with course before allowing the user to add
            AssessmentList = await DatabaseService.GetAssessmentsByCourse(DatabaseService.CurrentCourse.Id);
            if (AssessmentList.Count >= 2)
            {
                await Application.Current.MainPage.DisplayAlert("A course may only have 2 assessments", "There are already 2 assessments assigned" +
                    "to this course. You must edit or delete an existing assessment to update assessment information for this course", "Ok");
            }
            else
            {
                DatabaseService.IsAdd = true;
                await Application.Current.MainPage.Navigation.PushAsync(new ModifyAssessmentsPage());
            }
        }

        private async Task NavToEditAssessment(object o)
        {
            DatabaseService.IsAdd = false;
            DatabaseService.CurrentAssessment = o as Assessment;
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyAssessmentsPage());
        }

        private async Task DeleteAssessment(object o)
        {
            Assessment assessment = o as Assessment;
            await DatabaseService.DeleteAssessment(assessment.Id);
            Refresh();
        }


        private async void LoadAssessments()
        {
            //IsBusy = true;
            Assessments = new ObservableCollection<Assessment>(await DatabaseService.GetAssessmentsByCourse(DatabaseService.CurrentCourse.Id));
            //IsBusy = false;
        }

        public async void Refresh()
        {
            //needs to update course instructor and notes
            IsBusy = true;
            if (Assessments != null)
            {
                Assessments.Clear();
            }
            LoadAssessments();
            Instructor = await DatabaseService.GetInstructor(DatabaseService.CurrentInstructor.Id);
            Course = await DatabaseService.GetCourse(DatabaseService.CurrentCourse.Id);
            IsBusy = false;
        }
    }
}
