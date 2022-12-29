using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using MobileTermPlanner_JSarad.Models;
using Xamarin.Forms;
using MobileTermPlanner_JSarad.Services;
using System.Threading.Tasks;
using MobileTermPlanner_JSarad.Views;

namespace MobileTermPlanner_JSarad.ViewModels
{
    public class CourseViewModel : BaseViewModel
    {
        //properties
        
        private ObservableCollection<Course> _courses;
        public ObservableCollection<Course> Courses
        {
            get
            {
                return _courses;
            }
            set
            {
                _courses = value;
                OnPropertyChanged();
            }
        }
        
        private Term _term;
        public Term Term
        {
            get
            {
                return _term;
            }
            set
            {
                _term = value;
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
        public ICommand ViewCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        //class constructor
        public CourseViewModel()
        {
            Term = DatabaseService.CurrentTerm;
            LoadCourses();

            NavToAddCommand = new Command(async () => await NavToAddCourse());
            NavToEditCommand = new Command(async (o) => await NavToEditCourse(o));
            ViewCommand = new Command(async (o) => await ViewCourse(o));
            DeleteCommand = new Command(async (o) => await DeleteCourse(o));

            
            MessagingCenter.Subscribe<ModifyCourseViewModel, Instructor>(this, "AddInstructor", (sender, instructor) =>
            {
                AddInstructor(instructor);
            });

            MessagingCenter.Subscribe<ModifyCourseViewModel, Instructor>(this, "UpdateInstructor", (sender, instructor) =>
            {
                UpdateInstructor(instructor);
            });

            MessagingCenter.Subscribe<ModifyCourseViewModel>(this, "Cancel", (sender) =>
            {
                LoadCourses();
            });
        }

        //navigation methods
        private async Task NavToAddCourse()
        {
            if(Courses.Count >= 6)
            {
                await Application.Current.MainPage.DisplayAlert("Course Limit Reached", "There can be no more than 6 courses per term", "Ok");
            }
            else
            {
                DatabaseService.IsAdd = true;
                await Application.Current.MainPage.Navigation.PushAsync(new ModifyCoursePage());
            }
        }

        private async Task NavToEditCourse(object o)
        {
            DatabaseService.IsAdd = false;
            DatabaseService.CurrentCourse = o as Course;
            DatabaseService.CurrentInstructor = await DatabaseService.GetInstuctorByCourse(DatabaseService.CurrentCourse.Id);
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyCoursePage());
        }

        //modify and view methods
        private async Task ViewCourse(object o)
        {
            DatabaseService.CurrentCourse = o as Course;
            DatabaseService.CurrentInstructor = await DatabaseService.GetInstuctorByCourse(DatabaseService.CurrentCourse.Id);
            await Application.Current.MainPage.Navigation.PushAsync(new DetailedCourseViewPage());
        }

        private async void AddInstructor(Instructor instructor)
        {
            await DatabaseService.AddInstructor(instructor, DatabaseService.LastAddedId);
            LoadCourses();
        }

        private async void UpdateInstructor(Instructor instructor)
        {
            await DatabaseService.UpdateInstructor(instructor);
            LoadCourses();
        }

        private async Task DeleteCourse(object o)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Confirm Delete", "Are you sure you want to delete this course?", "Yes", "No");
            if (answer)
            {
                Course course = o as Course;
                await DatabaseService.DeleteCourse(course.Id);
                LoadCourses();
            }
            else
            {
                return;
            }
        }

        //load methods
        private async void LoadCourses()
        {
            if (Courses != null)
            {
                Courses.Clear();

                List<Course> courseList = new List<Course>(await DatabaseService.GetCoursesByTerm(Term.Id));
                foreach (Course course in courseList)
                {
                    Courses.Add(course);
                }
            }
            else
            {
                Courses = new ObservableCollection<Course>(await DatabaseService.GetCoursesByTerm(Term.Id));
            }
            AdjustHeight();
        }

        //Adjusts height of Assessments Observable Collection when modified
        public void AdjustHeight()
        {
            if (Courses.Count > 0)
            {
                RowHeight = Courses.Count * 170;
            }
            else
            {
                RowHeight = 100;
            }
        }
    }
}

