using System;
using System.Collections.Generic;
using System.Text;
using MobileTermPlanner_JSarad.Models;
namespace MobileTermPlanner_JSarad.ViewModels
{
    public class ModifyAssessmentViewModel : BaseViewModel
    {
        public List<Assessment> Assessments { get; set; }

        private Assessment _assessment;
        public Assessment Assessment
        {
            get
            {
                return _assessment;
            }
            set
            {
                _assessment = value; OnPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _assessment.Name;
            }
            set
            {
                _assessment.Name = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _assessment.StartDate;
            }
            set
            {
                _assessment.StartDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _assessment.EndDate;
            }
            set
            {
                _assessment.EndDate = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get
            {
                return _assessment.Type;
            }
            set
            {
                _assessment.Type = value;
                OnPropertyChanged();
            }
        }
        
        public ModifyAssessmentViewModel()
        {

        }
    }
}
