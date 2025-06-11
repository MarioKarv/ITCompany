using System;
using System.Collections.Generic;

namespace ITCompany.Models
{
    public class Task
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime FactDate { get; set; }
        public string Description { get; set; }
        public int Coeffecient { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; } 
        public ICollection<TaskCommand> TaskCommands { get; set; }
    }
}
