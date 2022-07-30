using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileTermPlanner_JSarad.Models;
using MobileTermPlanner_JSarad.Services;
using MobileTermPlanner_JSarad.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileTermPlanner_JSarad.ViewModels
{
    class DetailedCourseViewModel : BaseViewModel
    {
        //properties
        public string Placeholder = "No Notes to Display";
        
        /*checks Assessments in Database against Observable collection for one of each assessment type and no more than two assessments validation.
        Loads Assessments */
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
                CheckNotes();
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

        //sets text content for notes... if there are no notes text is Placeholder
        private string _filler;
        public string Filler
        {
            get
            {
                return _filler;
            }
            set
            {
                _filler = value;
                OnPropertyChanged();
            }
        }

        //commands
        public ICommand NavToEditCourseCommand { get; set; }
        public ICommand NavToAddAssessmentCommand { get; set; }
        public ICommand NavToEditAssessmentCommand { get; set; }
        public ICommand DeleteAssessmentCommand { get; set; }
        public ICommand ShareCommand { get; set; }

        //constuctor
        public DetailedCourseViewModel()
        {
            Course = DatabaseService.CurrentCourse;
            Instructor = DatabaseService.CurrentInstructor;
            LoadAssessments();
           
            NavToEditCourseCommand = new Command(async () => await NavToEditCourse());
            NavToAddAssessmentCommand = new Command(async () => await NavToAddAssessment());
            NavToEditAssessmentCommand = new Command(async (o) => await NavToEditAssessment(o));
            DeleteAssessmentCommand = new Command(async (o) => await DeleteAssessment(o));
            ShareCommand = new Command(async () => await ShareNote());
            
            MessagingCenter.Subscribe<ModifyAssessmentViewModel, Assessment>(this, "AddAssessment", (sender, assessment) =>
            {
                AddAssessment(assessment);
            });

            MessagingCenter.Subscribe<ModifyAssessmentViewModel, Assessment>(this, "UpdateAssessment", (sender, assessment) =>
            {
                UpdateAssessment(assessment);
            });

            //Used to update entire course including instructor and notes (strange, but it works)
            MessagingCenter.Subscribe<ModifyCourseViewModel, Instructor>(this, "UpdateInstructor", (sender, instructor) =>
            {
                UpdateInstructor(instructor);
            });

            MessagingCenter.Subscribe<ModifyAssessmentViewModel>(this, "Cancel", (sender) =>
            {
                LoadAssessments();
            });
        }

        //navigation methods
        private async Task NavToEditCourse()
        {
            DatabaseService.IsAdd = false;
            DatabaseService.CurrentCourse = Course;
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyCoursePage());
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
            LoadAssessments();
        }

        private async void UpdateAssessment(Assessment assessment)
        {
            await DatabaseService.UpdateAssessment(assessment);
            LoadAssessments();
        }

        private async Task DeleteAssessment(object o)
        {
            Assessment assessment = o as Assessment;
            await DatabaseService.DeleteAssessment(assessment.Id);
            LoadAssessments();
        }

        private async void UpdateInstructor(Instructor instructor)
        {
            //Updates Course which updates Notes on property changed
            Course = await DatabaseService.GetCourse(DatabaseService.CurrentCourse.Id);
            await DatabaseService.UpdateInstructor(instructor);
            Instructor = await DatabaseService.GetInstructor(instructor.Id);
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

        private async Task ShareNote()
        {
            if (string.IsNullOrEmpty(_course.Notes))
            {
                await Application.Current.MainPage.DisplayAlert("Notice", "There are no notes to share for this course", "Ok");
            }
            else
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Subject = $"Notes for {Course.Name}",
                    Text = _course.Notes,
                    Title = $" Share Notes for {Course.Name}"
                }); 
            }
        }
        
        //Adjusts height of Assessments Observable Collection when modified
        public void AdjustHeight()
        {
            if (Assessments.Count > 0)
            {
                RowHeight = Assessments.Count * 180;
            }
            else
            {
                RowHeight = 100;
            }
        }

        //Displays notes or placeholder "No notes to display" When Course is loded/modified
        private void CheckNotes()
        {
            if (string.IsNullOrEmpty(_course.Notes))
            {
                Filler = Placeholder;
            }
            else
            {
                Filler = _course.Notes;
            }
        }
    }
}
