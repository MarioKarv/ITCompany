namespace ITCompany.Models
{
    public class TaskCommand
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public User Employee { get; set; }
        public int TaskID { get; set; }
        public Task Task{ get; set; }
    }
}
