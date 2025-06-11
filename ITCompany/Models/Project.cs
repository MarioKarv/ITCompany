using System;
using System.Collections.Generic;

namespace ITCompany.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Price { get; set; }
        public int ProjectTypeID { get; set; }
        public int Percent { get; set; }
        public int ClientID { get; set; }
        public User Client { get; set; }
        public ProjectType ProjectType { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<ProjectCommand> ProjectCommands { get; set; }

    }
}
