using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileTermPlanner_JSarad.Views;
using MobileTermPlanner_JSarad.Models;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific; //added for scroll adjust
using Plugin.LocalNotifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileTermPlanner_JSarad.Services;

namespace MobileTermPlanner_JSarad
{
    public partial class App : Xamarin.Forms.Application
    //public partial class App : Application
    //Changed from scroll adjust
    {
        //private readonly Task _displayNotifications;
        public App()
        {
            Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            
            InitializeComponent();
            //_displayNotifications = DisplayNotifications();
            //Task.Run(async () => { await DisplayNotifications(); });
            MainPage = new NavigationPage(new MainPage());
        }

        //private async Task DisplayNotifications()
        //{
        //    await DatabaseService.Init();

        //    var courseList = await DatabaseService.db.Table<Course>().ToListAsync();
        //    var assessmentList = await DatabaseService.db.Table<Assessment>().ToListAsync();
            
        //    if (courseList.Count > 0)
        //    {
        //        try
        //        {
        //            for (int i = 1; i <= courseList.Count; i++)
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
        //                            $"on {courseList[i].EndDate} ", i);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"{ex}");
        //        }
        //    }

        //    if (assessmentList.Count > 0)
        //    {
        //        try
        //        {
        //            for (int i = 1; i <= assessmentList.Count; i++)
        //            {
        //                if (assessmentList[i].Notify)  
        //                {
        //                    if (assessmentList[i].StartDate.Date == DateTime.Today)
        //                    {
        //                        CrossLocalNotifications.Current.Show("Reminder", $"Upcoming Assessment {assessmentList[i].Name} that " +
        //                            $"starts today on {assessmentList[i].StartDate}", i);
        //                    }
        //                    else if (assessmentList[i].EndDate.Date == DateTime.Today)
        //                    {
        //                        CrossLocalNotifications.Current.Show("Reminder", $"Assessment {assessmentList[i].Name} ends today " +
        //                            $"on {assessmentList[i].EndDate} ");
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"{ex}");
        //        }
        //    }
        //    //Task.Run(async() => await MainPage = new NavigationPage(new MainPage());

        //}
        protected override void OnSleep()
        {
        }

       
    }
}
