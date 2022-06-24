using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MobileTermPlanner_JSarad.Services
{
    public class Validation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
        }

        public string ValidationMessage { get; set; }

        public bool ValidString(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                ValidationMessage = $"* field is required";
                return false;
            }
            else
            {
                ValidationMessage = "";
                return true;
            }
        }

        public bool ValidEmail(string str)
        {
            Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (str == null || !EmailRegex.IsMatch(str))
            {
                ValidationMessage = "* A valid Email is required";
                return false;
            }
            else
            {
                ValidationMessage = "";
                return true;
            }
        }

        public bool ValidPhone(string str)
        {
            string phone = str.Replace("-", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty);
            
            if (str == null)
            {
                ValidationMessage = "* Phone number is required";
                return false;
            }
            else if (phone.Length > 10 || phone.Length < 7 || (!long.TryParse(phone, out long i)))
            {
                ValidationMessage = "* Phone number must be between 7 and 10 digits";
                return false;
            }
            else if (phone.Trim().StartsWith("0"))
            {
                ValidationMessage = "* Phone number cannot start with zero";
                return false;
            }
            else
            {
                ValidationMessage = "";
                return true;
            }
        }

        public bool ValidDates(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date || startDate.Date == endDate.Date)
            {
                
                ValidationMessage = "* Start Date must come before End Date";
                return false;
            }
            else
            {
                ValidationMessage = "";
                return true;
            }
        }

        public bool DatesWithinRange(DateTime start, DateTime end, DateTime compareStart, DateTime compareEnd, string courseName)
        {
            if (start.Date > end.Date)
            {
                ValidationMessage = "Start Date must be before End Date";
                return false;
            }
            else if (start.Date < compareStart.Date || end.Date > compareEnd.Date)
            {
                ValidationMessage = $"The Assessment date must be set durring the Course {courseName} starting on {compareStart} and ending on" +
                    $"{compareEnd} ";
                return false;

            }
            else
            {
                ValidationMessage = "";
                return true;
            }
        }
    }

    
}
