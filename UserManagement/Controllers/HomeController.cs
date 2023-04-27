using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UserManagement.Data;
using UserManagement.DTO;
using UserManagement.Models;

namespace UserManagement.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            ViewBag.DepList=_applicationDbContext.Department.ToList();
            return View();
        }
        public IActionResult AddUser(User user)
        {
            _applicationDbContext.User.Add(user);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("AddUser");
        }
        public IActionResult GetUser()
        {
            List<User> user = _applicationDbContext.User.Include(x => x.Department).ToList();
            return View(user);
        }  
        public IActionResult GetUserById(int id)
        {
            ViewBag.DepList = _applicationDbContext.Department.ToList();
            //get user by id
            User user = _applicationDbContext.User.Include(x => x.Department   ).FirstOrDefault(use => use.id == id); //first id is refrenced from user calss and second id is refrenced from this method id
            return View(user);
        }

        public IActionResult UpdateUserById(User user)
        {
             //get user by id
           _applicationDbContext.User.Update(user);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("GetUser");
        }
        public IActionResult DeleteUserById(User user)
        {
            //get user by id
            _applicationDbContext.User.Remove(user);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("GetUser");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult GetUserInfo()
        {
            //get user and dep info using innq

            //dynamic obj 
            var userInfoTest = from use in _applicationDbContext.User
                           join dep in _applicationDbContext.Department on
                           use.DepartmentID equals dep.Id
                           select new
                           {
                               UserName = use.name,
                               UserAddress=use.address,
                               DepartName=dep.DepartmentName,
                               Salary=use.salary,
                               
                           };
            /////////////////
            //inner join
            var userInfo = from use in _applicationDbContext.User
                           join dep in _applicationDbContext.Department on
                           use.DepartmentID equals dep.Id
                           select new UserInfoDTO(use.name, use.address, dep.DepartmentName, use.salary);
            ViewBag.UserInfo = userInfo;

            /////////////////
            //left join
            var userInfoTem = from use in _applicationDbContext.User
                           join dep in _applicationDbContext.Department on
                           use.name equals dep.DepartmentName into useDep
                           from useDet in useDep.DefaultIfEmpty()
                           select new UserInfoDTO(use.name, use.address, useDet.DepartmentName, use.salary);
            ViewBag.userInfoTem = userInfoTem;

            /////////////////
            //right join
            var userInfoTems = from dep in _applicationDbContext.Department
                              join use in _applicationDbContext.User on
                              dep.DepartmentName equals use.name into useDep
                              from useDet in useDep.DefaultIfEmpty()
                              select new UserInfoDTO(useDet.name, useDet.address, dep.DepartmentName, useDet.salary);
            ViewBag.userInfoTems = userInfoTems;
            return View();

        }
    }
}