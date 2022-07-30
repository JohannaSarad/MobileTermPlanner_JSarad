using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileTermPlanner_JSarad.Services;
using MobileTermPlanner_JSarad.Models;
using MobileTermPlanner_JSarad.Views;
using Plugin.LocalNotifications;
using Xamarin.Forms;

namespace MobileTermPlanner_JSarad.ViewModels
{
    public class HomeViewModel
    {
        
        public ICommand NavToTermsCommand { get; set; }

        public HomeViewModel()
        {
            NavToTermsCommand = new Command(async () => await NavToTerms());
        }

        private async Task NavToTerms()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new TermViewPage());
        }
    }
}
