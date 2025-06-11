using System.Collections.Generic;

namespace ITCompany.Models
{
    public class ProjectType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        ICollection<Project> Projects { get; set; }
    }
}
