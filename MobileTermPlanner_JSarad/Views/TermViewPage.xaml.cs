using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileTermPlanner_JSarad.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileTermPlanner_JSarad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermViewPage : ContentPage
    {
        //private TermViewModel _viewModel;
        public TermViewPage()
        {
            InitializeComponent();
            //TermViewModel _viewModel = new TermViewModel();
            //BindingContext = _viewModel;
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
            
        //    TermViewModel _viewModel = new TermViewModel();
        //    _viewModel.Refresh();
        //}
    }
}