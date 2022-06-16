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
        public Term SelectedTerm 
        {
            get
            {
               return DatabaseService.SelectedTerm;
            }
        }
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

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get
            {
                return _selectedCourse;
            }
            set
            {
                _selectedCourse = value;
                OnPropertyChanged();
                    
            }
        }

        //commmands

        public ICommand NavToAddCommand { get; set; }
        public ICommand NavToEditCommand { get; set; }
        public ICommand ViewCourseCommand { get; set; }
        public ICommand DeleteCourseCommand { get; set; }

        public CourseViewModel()
        {
            LoadCourses();

            NavToAddCommand = new Command(async () => await NavToAddCourse());
            NavToEditCommand = new Command(async (o) => await NavToEditCourse(o));
            ViewCourseCommand = new Command(async (o) => await ViewCourse(o));
            DeleteCourseCommand = new Command(async (o) => await DeleteCourse(o));

            //added 6/14 untested
            MessagingCenter.Subscribe<ModifyCourseViewModel, Course>(this, "AddCourse", (sender, obj) =>
            {
                AddCourse(obj);
            });

            MessagingCenter.Subscribe<ModifyCourseViewModel, Course>(this, "EditCourse", (sender, obj) =>
            {
                UpdateCourse(obj);
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
            DatabaseService.SelectedCourse = o as Course;
            await Application.Current.MainPage.Navigation.PushAsync(new ModifyCoursePage());
        }

        private async Task ViewCourse(object o)
        {
            DatabaseService.SelectedCourse = o as Course;
            await Application.Current.MainPage.Navigation.PushAsync(new DetailedCourseViewPage());
        }

        private async void AddCourse(Course course)
        {
            await DatabaseService.AddCourse(course);
            Refresh();
        }

        private async void UpdateCourse(Course course)
        {
            //SelectedTerm = term;
            await DatabaseService.UpdateCourse(course);
            Refresh();
        }
        private async Task DeleteCourse(object o)
        {
            SelectedCourse = o as Course;
            await DatabaseService.DeleteCourse(SelectedCourse.Id);
            Refresh();
        }

        private async void LoadCourses()
        {
            //IsBusy = true;
            Courses = new ObservableCollection<Course>(await DatabaseService.GetCourseByTerm(DatabaseService.SelectedTerm.Id));
            //IsBusy = false;
        }

        public void Refresh()
        {
            //IsBusy = true;
            Courses.Clear();
            //Terms = new ObservableCollection<Term>(await DatabaseService.GetTerm());
            LoadCourses();
            //IsBusy = false;
        }
    }
}

