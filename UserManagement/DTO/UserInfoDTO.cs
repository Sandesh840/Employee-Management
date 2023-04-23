namespace UserManagement.DTO
{
    public class UserInfoDTO
    {
        public UserInfoDTO(string user_Name, string user_Address, string department_Name, double salary)
        {
            User_Name = user_Name;
            User_Address = user_Address;
            Department_Name = department_Name;
            Salary = salary;
        }

        public string User_Name { get; set; }
        public string User_Address { get; set; }
        public string Department_Name { get; set; }
        public double? Salary { get; set; }
    }
}
