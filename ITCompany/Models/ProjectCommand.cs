namespace ITCompany.Models
{
    public class ProjectCommand
    {
        public int ID { get; set; } 
        public int EmployeeID { get; set; } 
        public User Employee { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public bool isPrime { get; set; }
    }
}
