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

        //commmands

        public ICommand NavToAddCommand { get; set; }
        public ICommand NavToEditCommand { get; set; }
        public ICommand ViewCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public CourseViewModel()
        {
            Term = DatabaseService.CurrentTerm;
            LoadCourses();

            NavToAddCommand = new Command(async () => await NavToAddCourse());
            NavToEditCommand = new Command(async (o) => await NavToEditCourse(o));
            ViewCommand = new Command(async (o) => await ViewCourse(o));
            DeleteCommand = new Command(async (o) => await DeleteCourse(o));
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

        //private async void AddCourse(Course course)
        //{
        //    await DatabaseService.AddCourse(course);
        //    Refresh();
        //}

        //private async void UpdateCourse(Course course)
        //{
        //    //SelectedTerm = term;
        //    await DatabaseService.UpdateCourse(course);
        //    Refresh();
        //}
        private async Task DeleteCourse(object o)
        {
            Course course = o as Course;
            await DatabaseService.DeleteCourse(course.Id);
            Refresh();
        }

        private async void LoadCourses()
        {
            //IsBusy = true;
            Courses = new ObservableCollection<Course>(await DatabaseService.GetCourseByTerm(DatabaseService.CurrentTerm.Id));
            //IsBusy = false;
        }

        public void Refresh()
        {
            IsBusy = true;
            Courses.Clear();
            LoadCourses();
            IsBusy = false;
        }
    }
}

