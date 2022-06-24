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
    public partial class CourseViewPage : ContentPage
    {
        private CourseViewModel _viewModel;
        public CourseViewPage()
        {
            InitializeComponent();
            _viewModel = new CourseViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Refresh();
        }
    }
}