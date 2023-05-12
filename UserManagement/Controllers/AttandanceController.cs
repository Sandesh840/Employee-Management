using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
    public class AttandanceController : Controller
    {
        
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AttandanceController(ApplicationDbContext applicationDbContext, 
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
           
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Attandance()
        {            
            var logedInUser = await _userManager.GetUserAsync(User);
            ViewBag.CheckInCheck = "Check In";
            ViewBag.AttendanceId=null;
            ViewBag.UserShiftLog = new List<UserShiftLogViewModel>();
            //get login user
            
            if(logedInUser != null)
            {
                //get employee from logged inuser id
                var userDetails = _applicationDbContext.User
                    .Where(x => x.IdentityUserId == logedInUser.Id).FirstOrDefault();
                if(userDetails != null)
                {
                    var userShiftId = _applicationDbContext.UserShift
                        .Where(x => x.UserId == userDetails.id)
                        .Select(x => x.Id).FirstOrDefault();
                    if (userShiftId != null)
                    {
                        ViewBag.UserShiftLog=_applicationDbContext.UserShiftLog.
                            Where(x => x.UserId == userDetails.id && x.UserShiftId==userShiftId)
                            .Select(x=> new UserShiftLogViewModel
                            {
                                UserName=userDetails.name,
                                CheckInTime=x.CheckInTime.HasValue ?x.CheckInTime.Value
                                .ToString("hh:mm:tt") : null,
                                CheckOutTime=x.CheckOutTime.HasValue ?x.CheckOutTime.Value
                                .ToString("hh:mm:tt") : null
                            }).ToList();
                    }
                    //take last shift data
                    var userShiftLog = _applicationDbContext.UserShiftLog
                        .Where(es => es.UserId == userDetails.id && es.CheckInTime.Value.Date==DateTime.Today)
                        .OrderByDescending(es => es.Id).FirstOrDefault();
                    if (userShiftLog != null)
                    {
                        if(userShiftLog.CheckInTime !=null && userShiftLog.CheckOutTime == null)
                        {
                            ViewBag.CheckInCheck = "Check Out";
                            ViewBag.AttendanceId = userShiftLog.Id;
                        }                       
                    }
                }
            }
            return View();
        }
        public async Task<IActionResult> CheckInUser(int? attandanceId)
        {
            var logedInUser = await _userManager.GetUserAsync(User);
            if (logedInUser != null)
            {
                //get employee from logged inuser id
                var userDetails = _applicationDbContext.User
                    .Where(x => x.IdentityUserId == logedInUser.Id).FirstOrDefault();
                if (userDetails != null)
                {
                    var userShiftId = _applicationDbContext.UserShift
                        .Where(x => x.UserId == userDetails.id)
                       .Select(x => x.Id).FirstOrDefault();
                    if (attandanceId == null)
                    {
                        UserShiftLog userShiftLog = new UserShiftLog();
                        userShiftLog.CheckInTime=DateTime.Now;
                        userShiftLog.UserId=userDetails.id;
                        userShiftLog.UserShiftId = userShiftId;
                        _applicationDbContext.UserShiftLog.Add(userShiftLog);
                        _applicationDbContext.SaveChanges();
                    }
                    else
                    {
                        UserShiftLog? userShiftLog = _applicationDbContext.UserShiftLog
                            .Where(es => es.Id == attandanceId).FirstOrDefault();
                        if (userShiftLog != null)
                        {
                            userShiftLog.CheckOutTime = DateTime.Now;
                            _applicationDbContext.UserShiftLog.Update(userShiftLog);
                            _applicationDbContext.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("Attandance");
        }
    }
}
