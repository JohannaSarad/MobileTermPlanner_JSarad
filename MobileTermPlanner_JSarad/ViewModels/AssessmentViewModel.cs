using MobileTermPlanner_JSarad.Models;
using MobileTermPlanner_JSarad.Services;
using MobileTermPlanner_JSarad.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileTermPlanner_JSarad.ViewModels
{

    class AssessmentViewModel : BaseViewModel
    {
        //properties

        private List<Assessment> AssessmentList { get; set; }
        
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

        //adjusts height of ObservableCollection
        private int _rowHeight;
        public int RowHeight
        {
            get
            {
                return _rowHeight;
            }
            set
            {
                _rowHeight = value;
                OnPropertyChanged();
            }
        }

        //commmands
        public ICommand NavToAddCommand { get; set; }
        public ICommand NavToEditCommand { get; set; }
        //public ICommand ViewCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        //class constructor
        public AssessmentViewModel()
        {
            DatabaseService.IsDetailed = false;
            Course = DatabaseService.CurrentCourse;
            LoadAssessments();

            NavToAddCommand = new Command(async () => await NavToAddAssessment());
            NavToEditCommand = new Command(async (o) => await NavToEditAssessment(o));
            //ViewCommand = new Command(async (o) => await ViewCourse(o));
            DeleteCommand = new Command(async (o) => await DeleteAssessment(o));

           MessagingCenter.Unsubscribe<ModifyAssessmentViewModel, Assessment>(this, "AddAssessment");
           MessagingCenter.Subscribe<ModifyAssessmentViewModel, Assessment>(this, "AddAssessment", (sender, assessment) =>
            {
                AddAssessment(assessment);
                
            });

            MessagingCenter.Subscribe<ModifyAssessmentViewModel, Assessment>(this, "UpdateAssessment", (sender, assessment) =>
            {
                UpdateAssessment(assessment);
            });

            MessagingCenter.Subscribe<ModifyAssessmentViewModel>(this, "Cancel", (sender) =>
            {
                LoadAssessments();
            });
        }

        private async Task NavToAddAssessment()
        {
            //verify there are 2 or fewer assessments associated with course
            AssessmentList = await DatabaseService.GetAssessmentsByCourse(DatabaseService.CurrentCourse.Id);
            if (AssessmentList.Count >= 2)
            {
                await Application.Current.MainPage.DisplayAlert("Assessment Limit Reached", "There can be no more than 2 assessments per course", "Ok");
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

        //modify Methods
        private async void AddAssessment(Assessment assessment)
        {
            await DatabaseService.AddAssessment(assessment, DatabaseService.CurrentCourse.Id);
            ///MessagingCenter.Unsubscribe<ModifyAssessmentViewModel, Assessment>(this, "AddAssessment");
            LoadAssessments();
        }

        private async void UpdateAssessment(Assessment assessment)
        {
            await DatabaseService.UpdateAssessment(assessment);
            LoadAssessments();
        }

        private async Task DeleteAssessment(object o)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Confirm Delete", "Are you sure you want to delete this assessment?", "Yes", "No");
            if (answer)
            {
                Assessment assessment = o as Assessment;
                await DatabaseService.DeleteAssessment(assessment.Id);
                LoadAssessments();
            }
            else
            {
                return;
            }
        }


        //load methods
        private async void LoadAssessments()
        {
            if (Assessments != null)
            {
                Assessments.Clear();

                List<Assessment> assessmentList = new List<Assessment>(await DatabaseService.GetAssessmentsByCourse(Course.Id));
                foreach (Assessment assessment in assessmentList)
                {
                    Assessments.Add(assessment);
                }

            }
            else
            {
                Assessments = new ObservableCollection<Assessment>(await DatabaseService.GetAssessmentsByCourse(DatabaseService.CurrentCourse.Id));

            }
            AdjustHeight();
        }

        //Adjusts height of Assessments Observable Collection when modified
        public void AdjustHeight()
        {
            if (Assessments.Count > 0)
            {
                RowHeight = Assessments.Count * 150;
            }
            else
            {
                RowHeight = 100;
            }
        }
    }
}
