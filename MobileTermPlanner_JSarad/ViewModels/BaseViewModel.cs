﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MobileTermPlanner_JSarad.Models;
using MobileTermPlanner_JSarad.Services;

namespace MobileTermPlanner_JSarad.ViewModels
{
    public class BaseViewModel : Validation
    {

        //private ObservableCollection<Term> _terms;
        //public ObservableCollection<Term> Terms
        //{
        //    get
        //    {
        //        return _terms;
        //    }
        //    set
        //    {
        //        _terms = value;
        //        OnPropertyChanged();
        //    }
        //}
        
        //public async void LoadTerms()
        //{
        //    if (Terms != null) {
        //        Terms.Clear();
                
        //        List<Term> termList = new List<Term>(await DatabaseService.GetTerms());
        //        foreach (Term term in termList)
        //        {
        //            Terms.Add(term);
        //        }
        //    }
        //    else
        //    {
        //        Terms = new ObservableCollection<Term>(await DatabaseService.GetTerms());
        //    }
        //}


        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        
        //Validation MessageProperties

        private string _emptyErrorMessageOne;
        public string EmptyErrorMessageOne
        {
            get 
            { 
                return _emptyErrorMessageOne; 
            }
            set
            {
                _emptyErrorMessageOne = value;
                OnPropertyChanged();
            }
        }

        private string _emptyErrorMessageTwo;
        public string EmptyErrorMessageTwo
        {
            get
            {
                return _emptyErrorMessageTwo;
            }
            set
            {
                _emptyErrorMessageTwo = value;
                OnPropertyChanged();

            }
        }

        private string _datesErrorMessageOne;
        public string DatesErrorMessageOne
        {
            get
            {
                return _datesErrorMessageOne;
            }
            set
            {
                _datesErrorMessageOne = value;
                OnPropertyChanged();
            }
        }

        private string _datesErrorMessageTwo;
        public string DatesErrorMessageTwo
        {
            get
            {
                return _datesErrorMessageTwo;
            }
            set
            {
                _datesErrorMessageTwo = value;
                OnPropertyChanged();
            }
        }
        private string _emailErrorMessage;
        public string EmailErrorMessage
        {
            get
            {
                return _emailErrorMessage;
            }
            set
            {
                _emailErrorMessage = value;
                OnPropertyChanged();
            }
        }

        private string _phoneErrorMessage;
        public string PhoneErrorMessage
        {
            get
            {
                return _phoneErrorMessage;
            }
            set
            {
                _phoneErrorMessage = value;
                OnPropertyChanged();
            }
        }

        private string _selectionErrorMessage;
        public string SelectionErrorMessage
        {
            get
            {
                return _selectionErrorMessage;
            }
            set
            {
                _selectionErrorMessage = value;
                OnPropertyChanged();
            }
        }

        private string _startDateLabel;
        public string StartDateLabel
        {
            get
            {
                return _startDateLabel;
            }
            set
            {
                _startDateLabel = value;
                OnPropertyChanged();
            }
        }

        private string _endDateLabel;
        public string EndDateLabel
        {
            get
            {
                return _endDateLabel;
            }
            set
            {
                _endDateLabel = value;
                OnPropertyChanged();
            }
        }
    }
}
