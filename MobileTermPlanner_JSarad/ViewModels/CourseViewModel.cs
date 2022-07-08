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
        }

        //methods
        private async Task NavToAddCourse()
        {
            DatabaseService.IsAdd = true;
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyCoursePage());
        }

        private async Task NavToEditCourse(object o)
        {
            DatabaseService.IsAdd = false;
            DatabaseService.CurrentCourse = o as Course;
            DatabaseService.CurrentInstructor = await DatabaseService.GetInstuctorByCourse(DatabaseService.CurrentCourse.Id);
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyCoursePage());
        }

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
            Course course = o as Course;
            await DatabaseService.DeleteCourse(course.Id);
            LoadCourses();
        }

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
        }
    }
}

