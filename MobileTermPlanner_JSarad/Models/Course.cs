using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileTermPlanner_JSarad.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Notify { get; set; }
        public string Notes { get; set; }

        //public bool NotifyStartDate { get; set; }
        //public bool NotifyEndDate { get; set; }
        [Indexed]
        public int TermId { get; set; }
    }
}
