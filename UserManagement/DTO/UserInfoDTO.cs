namespace UserManagement.DTO
{
    public class UserInfoDTO
    {
        public UserInfoDTO(string user_Name,  string department_Name, double salary)
        {
            User_Name = user_Name;
            
            Department_Name = department_Name;
            Salary = salary;
        }

        public string User_Name { get; set; }
       
        public string Department_Name { get; set; }
        public double? Salary { get; set; }
    }
}
