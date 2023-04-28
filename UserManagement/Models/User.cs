using Microsoft.AspNetCore.Identity;

namespace UserManagement.Models
{
    public class User
    {
        public int id { get; set; } 
        public string name { get; set; }     
        public double salary { get; set; }
        public string email { get; set; }       
        //Many to one
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public string IdentityUserId { get; set; }
    }
}
