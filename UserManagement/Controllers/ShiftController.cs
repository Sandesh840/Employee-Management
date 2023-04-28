using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class ShiftController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;

        public ShiftController(ILogger<ShiftController> logger, ApplicationDbContext appDbContext)
        {

            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddShift()
        {
            ViewBag.UserList = _appDbContext.User.ToList();
            return View();
        }
        public async Task<IActionResult> AddUserShift(UserShift user)
        {
            await _appDbContext.UserShift.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("AddShift");
        }
    }
}
