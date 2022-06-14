using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileTermPlanner_JSarad.ViewModels;
using MobileTermPlanner_JSarad.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileTermPlanner_JSarad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyTermsPage : ContentPage
    {
        //changed 6/14 untested
        //private ModifyTermViewModel _model = null;
        public ModifyTermsPage()
        {
            InitializeComponent();
        }

        //changed 6/14 untested
        //public ModifyTermsPage(Term selectedTerm)
        //{
        //    InitializeComponent();
        //   _model = new ModifyTermViewModel(selectedTerm);
        //}
    }
}