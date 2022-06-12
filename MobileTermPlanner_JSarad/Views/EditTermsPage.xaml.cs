using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileTermPlanner_JSarad.Models;
using MobileTermPlanner_JSarad.ViewModels;


namespace MobileTermPlanner_JSarad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTermsPage : ContentPage
    {
        private EditTermViewModel _model = null;
        public EditTermsPage(Term term)
        {
            InitializeComponent();
            _model = new EditTermViewModel(term);
            this.BindingContext = _model;
        }
    }
}