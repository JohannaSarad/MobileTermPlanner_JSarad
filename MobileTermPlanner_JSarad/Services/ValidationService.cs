using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MobileTermPlanner_JSarad.Services
{
    public class ValidationService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _stringValidationMessage;
        public string StringValidationMessage
        {
            get
            {
                return _stringValidationMessage;
            }
            set
            {
                _stringValidationMessage = value;
                OnPropertyChanged();
            }
        }

        private string _emailValidationMessage;
        public string EmailValidationMessage
        {
            get
            {
                return _emailValidationMessage;
            }
            set
            {
                _emailValidationMessage = value;
                OnPropertyChanged();
            }
        }

        private string _phoneValidationMessage;
        public string PhoneValidationMessage
        {
            get
            {
                return _phoneValidationMessage;
            }
            set
            {
                _phoneValidationMessage = value;
                OnPropertyChanged();
            }
        }

        private string _dateValidationMessage;
        public string DateValidationMessage
        {
            get
            {
                return _dateValidationMessage;
            }
            set
            {
                _dateValidationMessage = value;
                OnPropertyChanged();
            }
        }

        private string _dateOutOfRangeMessage;
        public string DateOutOfRangeMessage
        {
            get
            {
                return _dateOutOfRangeMessage;
            }
            set
            {
                _dateOutOfRangeMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsValid;

        public bool ValidateString(string str, string name)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                IsValid = false;
                StringValidationMessage = $"* {name} field is required";
                return false;
            }
            else
            {
                IsValid = true;
                StringValidationMessage = "";
                return true;
            }
        }

        public bool ValidateEmail(string str)
        {
            Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (str == null || !EmailRegex.IsMatch(str))
            {
                IsValid = false;
                EmailValidationMessage = "* A valid Email is required";
                return false;
            }
            else
            {
                IsValid = true;
                EmailValidationMessage = "";
                return true;
            }
        }

        public bool ValidatePhone(string str)
        {
            //int phone = Convert.ToInt32(str);
            if (str == null)
            {
                IsValid = false;
                PhoneValidationMessage = "* Phone number is required";
                return false;
            }
            else if (str.Length > 10 || str.Length < 7)
            {
                PhoneValidationMessage = "* Phone number must be between 7 and 10 digits";
                IsValid = false;
                return false;
            }
            else
            {
                PhoneValidationMessage = "";
                IsValid = true;
                return true;
            }
        }

        public bool ValidateDates (DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date || startDate.Date == endDate.Date)
            {
                IsValid = false;
                DateValidationMessage = "* Term start date must be before term end date";
                return false;
            }
            else
            {
                IsValid = true;
                DateValidationMessage = "";
                return true;
            }
        }
        
        public bool ValidateDatesWithinRange(DateTime start, DateTime end, DateTime compareStart, DateTime compareEnd, string courseName)
        {
            if (start.Date > end.Date)
            {
                IsValid = false;
                DateOutOfRangeMessage = "Start Date must be before End Date";
                return false;
            }
            else if (start.Date < compareStart.Date || end.Date > compareEnd.Date)
            {
                IsValid = false;
                DateOutOfRangeMessage = $"The Assessment date must be set durring the Course {courseName} starting on {compareStart} and ending on" +
                    $"{compareEnd} ";
                return false;

            }
            else
            {
                IsValid = true;
                DateOutOfRangeMessage = "";
                return true;
            }
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
        }
    }
}
