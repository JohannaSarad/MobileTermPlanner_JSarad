using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileTermPlanner_JSarad.Views;
using MobileTermPlanner_JSarad.Services;
using MobileTermPlanner_JSarad.Models;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific; //added for scroll adjust
using Plugin.LocalNotifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileTermPlanner_JSarad
{
    public partial class App : Xamarin.Forms.Application
    //public partial class App : Application
    //Changed from scroll adjust
    {
        public App()
        {
            Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            InitializeComponent();

            //OnStart();
            //InitializeDatabase();
            InitializeNavigation();
            
            
            //MainPage = new NavigationPage(new TermViewPage());
        }

        private async void InitializeDatabase()
        {
            await DatabaseService.Init();
        }

        private async void InitializeNavigation()
        {
            MainPage = new NavigationPage(new TermViewPage());
        }

        
        

        protected override void OnStart()
        {
        //    List<Term> termList = await DatabaseService.GetTerms();
        //    List<Course> courseList = await DatabaseService.GetCourses();
        //    List<Assessment> assessmentList = await DatabaseService.GetAssessments();

        //    if (termList.Count > 0)
        //    {
        //        foreach (Term term in termList)
        //        {
        //            if (term.NotifyStartDate && (term.StartDate.Date == DateTime.Today))
        //            {
        //                CrossLocalNotifications.Current.Show("Reminder", $"Upcoming Term {term.Name} starts today on {term.StartDate} ");
        //            }
        //            else if (term.NotifyEndDate && (term.EndDate.Date == DateTime.Today))
        //            {
        //                CrossLocalNotifications.Current.Show("Reminder", $"Term {term.Name} ends today on {term.EndDate} ");
        //            }
        //        }
        //    }
        //    if (courseList.Count > 0)
        //    {
        //        foreach (Course course in courseList)
        //        {
        //            if (course.NotifyStartDate && (course.StartDate.Date == DateTime.Today))
        //            {
        //                CrossLocalNotifications.Current.Show("Reminder", $"Upcoming Course {course.Name} starts today on {course.StartDate} ");
        //            }
        //            else if (course.NotifyEndDate && (course.EndDate.Date == DateTime.Today))
        //            {
        //                CrossLocalNotifications.Current.Show("Reminder", $"Course {course.Name} ends today on {course.EndDate} ");
        //            }
        //        }
        //    }

        //    if (assessmentList.Count > 0)
        //    {
        //        foreach (Assessment assessment in assessmentList)
        //        {
        //            if (assessment.NotifyStartDate && (assessment.StartDate.Date == DateTime.Today))
        //            {
        //                CrossLocalNotifications.Current.Show("Reminder", $"Upcoming Assessment {assessment.Name} that starts today on {assessment.StartDate} ");
        //            }
        //            else if (assessment.NotifyEndDate && (assessment.EndDate.Date == DateTime.Today))
        //            {
        //                CrossLocalNotifications.Current.Show("Reminder", $"Assessment {assessment.Name} ends today on {assessment.EndDate} ");
        //            }
        //        }
        //    }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
