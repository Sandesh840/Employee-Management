using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;
using UserManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace UserManagement.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;

        public DepartmentController(ILogger<DepartmentController> logger, ApplicationDbContext appDbContext)
        {
            
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddDepartment()
        {
            return View();
        }

        public IActionResult SaveDepartment(Department department )
        {
            _appDbContext.Department.Add(department);
            _appDbContext.SaveChanges();
            return Redirect("AddDepartment");
        }
        public IActionResult GetDepartment(Department department)
        {
           List<Department> departmentList = _appDbContext.Department.ToList();
            return View(departmentList);
        }
        public IActionResult GetDepartmentById(int id)
        {
            // ViewBag.UserList = _appDbContext.User.ToList();
            Department department = _appDbContext.Department.Include(x => x.Users).FirstOrDefault(use => use.Id == id);
            return View(department);
        }
        public IActionResult UpdateDepartmentById(Department department)
        {
            _appDbContext.Department.Update(department);
            _appDbContext.SaveChanges();
            return RedirectToAction("GetDepartment");
        }
        public IActionResult DeleteDepartmentById(Department department)
        {
            //get user by id
            _appDbContext.Department.Remove(department);
            _appDbContext.SaveChanges();
            return RedirectToAction("GetDepartment");
        }

    }
}
