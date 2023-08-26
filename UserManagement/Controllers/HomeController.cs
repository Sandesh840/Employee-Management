using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var userDepartment = _applicationDbContext.UserDepartment.ToList();
            return View();
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            ViewBag.DepList=_applicationDbContext.Department.ToList();
            ViewBag.UserList = _applicationDbContext.Users.ToList();
            return View();
        }
        public async Task <IActionResult> AddUser(User user)
        {
            await _applicationDbContext.User.AddAsync(user);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("AddUser");
        }
        public async Task <IActionResult> GetUser()
        {
            var logedInUser=await _userManager.GetUserAsync(User);
            var userRole = await _userManager.GetRolesAsync(logedInUser);
            List<User> user = new List<User>();
            if(userRole != null)
            {
                if (userRole.FirstOrDefault() == "Staff")
                {
                    user = await _applicationDbContext.User
                        .Where(x => x.IdentityUserId == logedInUser.Id)
                        .Include(x => x.Department).ToListAsync();
                }
                else if(userRole.FirstOrDefault() == "Admin")
                {
                    user = await _applicationDbContext.User
                        .Include(x => x.Department).ToListAsync();
                   
                }
            }
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
                               
                               DepartName=dep.DepartmentName,
                               Salary=use.salary,
                               
                           };
            /////////////////
            //inner join
            var userInfo = from use in _applicationDbContext.User
                           join dep in _applicationDbContext.Department on
                           use.DepartmentID equals dep.Id
                           select new UserInfoDTO(use.name, dep.DepartmentName, use.salary);
            ViewBag.UserInfo = userInfo;

            /////////////////
            //left join
            var userInfoTem = from use in _applicationDbContext.User
                           join dep in _applicationDbContext.Department on
                           use.name equals dep.DepartmentName into useDep
                           from useDet in useDep.DefaultIfEmpty()
                           select new UserInfoDTO(use.name, useDet.DepartmentName, use.salary);
            ViewBag.userInfoTem = userInfoTem;

            /////////////////
            //right join
            var userInfoTems = from dep in _applicationDbContext.Department
                              join use in _applicationDbContext.User on
                              dep.DepartmentName equals use.name into useDep
                              from useDet in useDep.DefaultIfEmpty()
                              select new UserInfoDTO(useDet.name,  dep.DepartmentName, useDet.salary);
            ViewBag.userInfoTems = userInfoTems;
            return View();

        }
    }
}