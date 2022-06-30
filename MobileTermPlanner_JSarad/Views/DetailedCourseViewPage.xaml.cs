using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileTermPlanner_JSarad.ViewModels;

namespace MobileTermPlanner_JSarad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedCourseViewPage : ContentPage
    {
        private DetailedCourseViewModel _viewModel;
        public DetailedCourseViewPage()
        {
            InitializeComponent();
            _viewModel = new DetailedCourseViewModel();
            BindingContext = _viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Refresh();
        }
    }
}