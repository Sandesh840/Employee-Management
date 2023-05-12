using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.ViewModels;

namespace UserManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<UserShift> UserShift { get; set; }
        public DbSet<UserDepartment> UserDepartment { get; set; }
        public DbSet<UserShiftLog> UserShiftLog { get; set; }
        //OnModelCreating gets call paralley when DbContext get initialized

        //fake entity to call strod Procedure
        public DbSet<ShiftStatusViewModel> Sp_LateInEarlyOut_ViMo { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {   
            base.OnModelCreating(builder);
            builder.Entity<UserDepartment>().HasNoKey();
            builder.Entity<ShiftStatusViewModel>().HasNoKey();
        }
    }
}
