using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserManagement.Data;
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
            List<User> user = _applicationDbContext.User.ToList();
            return View(user);
        }
        public IActionResult GetUserById(int id)
        {
            //get user by id
            User user= _applicationDbContext.User.Find(id);
            return View(user);
        }
        [HttpPost]
        public IActionResult UpdateUserById(User user)
        {
            //get user by id
            _applicationDbContext.User.Update(user);
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
    }
}