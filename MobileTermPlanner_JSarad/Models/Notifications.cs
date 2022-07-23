using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace MobileTermPlanner_JSarad.Models
{
    public class Notifications
    {
       [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string Occurrence { get; set; }
        public DateTime NotifyDate { get; set; }
    }
}
