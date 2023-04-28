namespace UserManagement.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string Manager { get; set; }       
        //one to many
        public List<User> Users { get; set; }
    }
}
