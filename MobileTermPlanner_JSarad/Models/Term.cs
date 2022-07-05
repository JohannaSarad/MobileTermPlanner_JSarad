using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileTermPlanner_JSarad.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool NotifyStartDate { get; set; }
        public bool NotifyEndDate { get; set; }
    }
}
