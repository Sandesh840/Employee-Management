using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
    public class LogInController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        //manages user
        private readonly UserManager<IdentityUser> _userManager;
        //helps in signin process
        private readonly SignInManager<IdentityUser> _signInManager;
        public LogInController(ILogger<LogInController> logger, ApplicationDbContext appDbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {

            _appDbContext = appDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RegisterUser()
        {

            return View();
        }
        public async Task<IActionResult> SaveUser(RegisterViewModel registerViewModel)
        {
            var user = new IdentityUser()
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("RegisterUser");
        }
    }
}
