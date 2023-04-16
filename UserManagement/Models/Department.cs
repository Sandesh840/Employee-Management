namespace UserManagement.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int MemberNumber { get; set; }
        public string Manager { get; set; }
        public string Discription { get; set; }
        //one to many
        public List<User> Users { get; set; }
    }
}
