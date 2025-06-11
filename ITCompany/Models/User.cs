using System.Collections.Generic;

namespace ITCompany.Models
{
    public class User
    {
        public int ID { get; set; } 
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public string FullName { get => FirstName + " " + LastName; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<ProjectCommand> ProjectCommands { get; set; }
        public ICollection<TaskCommand> TaskCommands { get; set; }

    }
}
