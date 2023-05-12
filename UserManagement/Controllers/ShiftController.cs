using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult GetShift(UserShift shift)
        {
            List<UserShift> userShifts=_appDbContext.UserShift.ToList();
            ViewBag.NameList = _appDbContext.User.ToList();
            return View(userShifts);
        }
        public IActionResult GetShiftyId(int id)
        {
            ViewBag.UserList = _appDbContext.User.ToList();
            //get user by id
            UserShift userShift = _appDbContext.UserShift.Find(id); 
            return View(userShift);
        }
        public IActionResult UpdateShiftById(UserShift userShift)
        {           
            _appDbContext.UserShift.Update(userShift);
            _appDbContext.SaveChanges();
            return RedirectToAction("GetShift");
        }
        public IActionResult DeleteShiftById(UserShift userShift)
        {
            _appDbContext.UserShift.Remove(userShift);
            _appDbContext.SaveChanges();
            return RedirectToAction("GetShift");
        }

    }
}
