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

        public bool ValidString(string str, string field)
        {
            if (string.IsNullOrEmpty(str))
            {
                ValidationMessage = $"* {field} is required";
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
                ValidationMessage = "* valid email is required";
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

            if (!string.IsNullOrEmpty(str))
            {
                string phone = str.Replace("-", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);
                if (phone.Length > 10 || phone.Length < 7 || (!long.TryParse(phone, out long i)))
                {
                    ValidationMessage = "* phone number must be between 7 and 10 digits";
                    return false;
                }
                else if (phone.Trim().StartsWith("0"))
                {
                    ValidationMessage = "* phone number cannot start with zero";
                    return false;
                }
                else
                {
                    ValidationMessage = "";
                    return true;
                }
            }
            else
            {
                ValidationMessage = "* phone number is required";
                return false;
            }
        }

        public bool ValidDates(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date || startDate.Date == endDate.Date)
            {
                
                ValidationMessage = "* start date must come before end date";
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
                ValidationMessage = "start date must be before end date";
                return false;
            }
            else if (start.Date < compareStart.Date || end.Date > compareEnd.Date)
            {
                ValidationMessage = $"assessment dates must be set durring the Course {courseName} starting on {compareStart} and ending on" +
                    $"{compareEnd} ";
                return false;

            }
            else
            {
                ValidationMessage = "";
                return true;
            }
        }

        public bool ValidSelection(string str1, string type)
        {
            if(string.IsNullOrEmpty(str1))
            {
                ValidationMessage = $"* {type} is requred";
                return false;
            }
            else
            {
                ValidationMessage = "";
                return true;
            }
        }

        public void UpdateNotifyLabel(bool notificationOn, string str)
        {
            ValidationMessage = notificationOn ? $"Turn Off {str} Notifications" : $"Turn On {str} Notifications";
        }

        public bool ValidCharacters(string note)
        {
            if(note.Length >= 255)
            {
                ValidationMessage = "Notes cannot exceed 255 Characters";
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
