using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
    public class ShiftStatusController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ShiftStatusController(ApplicationDbContext applicationDbContext,
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
        public async Task<IActionResult> GetShiftStatus()
        {
            var logInUser = await _userManager.GetUserAsync(User);
            ViewBag.UserShiftStatus = new List<ShiftStatusViewModel>();
            if (logInUser != null)
            {
                var userDetail = _applicationDbContext.User
                    .Where(x => x.IdentityUserId == logInUser.Id).FirstOrDefault();
                if (userDetail != null)
                {
                    var userShiftId = _applicationDbContext.UserShift
                        .Where(x => x.UserId == userDetail.id).FirstOrDefault();
                    /*.Select(x => x.Id)*/
                    if (userShiftId!=null)
                    {
                        var userShiftLogId = _applicationDbContext.UserShiftLog
                        .Where(x => x.UserId == userDetail.id && x.UserShiftId==userShiftId.Id).FirstOrDefault();
                        ViewBag.UserShiftStatus = _applicationDbContext.UserShiftLog
                            .Where(x => x.UserId == userDetail.id && x.UserShiftId == userShiftId.Id)
                            .Select(x => new ShiftStatusViewModel
                            {
                                UserName = userDetail.name,
                                CheckInTime1 = x.CheckInTime.HasValue ? x.CheckInTime.Value.ToString("hh:mm tt"):null,
                                CheckOutTime1=x.CheckOutTime.HasValue? x.CheckOutTime.Value.ToString("hh:mm tt") :null,
                                AttendanceDate=x.CheckInTime.HasValue? x.CheckInTime.Value.ToString("dd/MM/yyyy"):null,
                                Shift = userShiftId.CheckIn.HasValue && userShiftId.CheckOut.HasValue ?
                                    userShiftId.CheckIn.Value.ToString("hh:mm tt") + "-" + userShiftId.CheckOut.Value.ToString("hh:mm tt") : null,
                                Status=x.CheckInTime.HasValue && x.CheckOutTime.HasValue? (x.CheckInTime.Value>userShiftId.CheckIn.Value.AddMinutes(15) 
                                    && x.CheckOutTime.Value< userShiftId.CheckOut.Value.AddMinutes(-15)?"late In early out"
                                    :x.CheckInTime.Value>userShiftId.CheckIn.Value.AddMinutes(15)?"late In"
                                    :x.CheckOutTime.Value<userShiftId.CheckOut.Value.AddMinutes(-15)?"early out"
                                    :"On Time")
                                    :"No Entry"
                            }) ;
                    }
                }

            }

            return View();
        }
        public async Task<IActionResult> LateInEarlyOut()
        {
            var logInUser = await _userManager.GetUserAsync(User);
            ViewBag.UserShiftStatus = new List<ShiftStatusViewModel>();
            if (logInUser != null)
            {
                var userDetail = _applicationDbContext.User
                    .Where(x => x.IdentityUserId == logInUser.Id).FirstOrDefault();
                if (userDetail != null)
                {
                    var param = new SqlParameter("@UserId", userDetail.id);
                    var result = _applicationDbContext.Sp_LateInEarlyOut_ViMo
                        .FromSqlRaw("[dbo].[Sp_LateInEarlyOut] @UserId", param).ToList();
                    ViewBag.UserShiftStatus=result;
                }

            }
            return View();
        }
    }
}
