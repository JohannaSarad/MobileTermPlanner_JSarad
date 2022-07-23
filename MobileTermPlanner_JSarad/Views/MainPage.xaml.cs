using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MobileTermPlanner_JSarad.Models;
using MobileTermPlanner_JSarad.Services;
using Plugin.LocalNotifications;

namespace MobileTermPlanner_JSarad
{
    public partial class MainPage : ContentPage
    {
        //private readonly Task _initializeDatabase();
        public MainPage()
        {
            InitializeComponent();
            //Task.Run(async() => await DatabaseService.Init());
           
        }

        //protected override async void OnAppearing()
        //{
            
        //    //await Task.Delay(2000);
        //    var courseList = await DatabaseService.db.Table<Course>().ToListAsync();
        //    var assessmentList = await DatabaseService.db.Table<Assessment>().ToListAsync();

        //    //try
        //    //{
        //        if (courseList.Count > 0)
        //        {

        //            for (int i = 0; i < courseList.Count; i++)
        //            {
        //            try
        //            {

        //                if (courseList[i].Notify)
        //                {
        //                    if (courseList[i].StartDate.Date == DateTime.Today)
        //                    {
        //                        CrossLocalNotifications.Current.Show("Reminder", $"Upcoming Course {courseList[i].Name} starts today " +
        //                            $"on {courseList[i].StartDate} ", i);
        //                    }
        //                    else if (courseList[i].EndDate.Date == DateTime.Today)
        //                    {
        //                        CrossLocalNotifications.Current.Show("Reminder", $"Course {courseList[i].Name} ends today " +
        //                            $"on {courseList[i].EndDate} ", i + 1);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"{ex}");
        //            }
                        
        //            }
        //        }
        //        if (assessmentList.Count > 0)
        //        { 
        //            for (int i = 0; i < assessmentList.Count; i++)
        //            {
        //                if (assessmentList[i].Notify)
        //                {
        //                    if (assessmentList[i].StartDate.Date == DateTime.Today)
        //                    {
        //                        CrossLocalNotifications.Current.Show("Reminder", $"Upcoming Assessment {assessmentList[i].Name} that " +
        //                        $"starts today on {assessmentList[i].StartDate}", i + 1);
        //                    }
        //                    else if (assessmentList[i].EndDate.Date == DateTime.Today)
        //                    {
        //                        CrossLocalNotifications.Current.Show("Reminder", $"Assessment {assessmentList[i].Name} ends today " +
        //                        $"on {assessmentList[i].EndDate} ", i + 1);
        //                    }
        //                }
        //            }
        //        }
        //    base.OnAppearing();
            //}

            //catch (Exception ex)
            //{
            //    Console.WriteLine($"{ex}");
            //}
        //}
        
        //protected override async void OnAppearing()
        //{
        //    var notifications = await DatabaseService.db.Table<Notifications>().ToListAsync();
        //    if (notifications.Count > 0)
        //    {
        //        try
        //        {
        //            foreach (Notifications notification in notifications)
        //            {
        //                CrossLocalNotifications.Current.Show("Reminder", $"{notification.Type}: {notification.TypeName} is {notification.Occurrence} Today" +
        //                    $"on {notification.NotifyDate}");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"{ex}");
        //        }
        //    }
        //    base.OnAppearing();

        //}

        //private async Task CheckNotifications()
        //{
        //    await Task.Delay(2000);
        //    if (DatabaseService.IsBusy)
        //    {
        //        await DatabaseService.Init();
        //        //await DatabaseService.FillSampleData();

        //        //List<Term> termList = await DatabaseService.GetTerms();
        //        //List<Course> courseList = await DatabaseService.GetCourses();
        //        //List<Assessment> assessmentList = await DatabaseService.GetAssessments();
        //        var termList = await DatabaseService.db.Table<Term>().ToListAsync();
        //        var courseList = await DatabaseService.db.Table<Course>().ToListAsync();
        //        var assessmentList = await DatabaseService.db.Table<Assessment>().ToListAsync();


        //        if (termList.Count > 0)
        //        {
        //            foreach (Term term in termList)
        //            {
        //                if (term.NotifyStartDate && (term.StartDate.Date == DateTime.Today))
        //                {
        //                    CrossLocalNotifications.Current.Show("Reminder", $"Upcoming Term {term.Name} starts today on {term.StartDate} ");
        //                }
        //                else if (term.NotifyEndDate && (term.EndDate.Date == DateTime.Today))
        //                {
        //                    CrossLocalNotifications.Current.Show("Reminder", $"Term {term.Name} ends today on {term.EndDate} ");
        //                }
        //            }
        //        }
        //        if (courseList.Count > 0)
        //        {
        //            foreach (Course course in courseList)
        //            {
        //                if (course.NotifyStartDate && (course.StartDate.Date == DateTime.Today))
        //                {
        //                    CrossLocalNotifications.Current.Show("Reminder", $"Upcoming Course {course.Name} starts today on {course.StartDate} ");
        //                }
        //                else if (course.NotifyEndDate && (course.EndDate.Date == DateTime.Today))
        //                {
        //                    CrossLocalNotifications.Current.Show("Reminder", $"Course {course.Name} ends today on {course.EndDate} ");
        //                }
        //            }
        //        }

        //        if (assessmentList.Count > 0)
        //        {
        //            foreach (Assessment assessment in assessmentList)
        //            {
        //                if (assessment.NotifyStartDate && (assessment.StartDate.Date == DateTime.Today))
        //                {
        //                    CrossLocalNotifications.Current.Show("Reminder", $"Upcoming Assessment {assessment.Name} that starts today on {assessment.StartDate} ");
        //                }
        //                else if (assessment.NotifyEndDate && (assessment.EndDate.Date == DateTime.Today))
        //                {
        //                    CrossLocalNotifications.Current.Show("Reminder", $"Assessment {assessment.Name} ends today on {assessment.EndDate} ");
        //                }
        //            }
        //        }
        //        DatabaseService.IsBusy = false;
        //    }
        //}
    }
}
