using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileTermPlanner_JSarad.Views;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific; //added for scroll adjust

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

            MainPage = new NavigationPage(new TermViewPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
