using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileTermPlanner_JSarad.Models
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Indexed]
        public int CourseId { get; set; }
    }
}
