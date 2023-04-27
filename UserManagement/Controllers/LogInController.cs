using Microsoft.AspNetCore.Authorization;
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
        //manages the role
        private readonly RoleManager<IdentityRole> _roleManager;
        public LogInController(ILogger<LogInController> logger, ApplicationDbContext appDbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {

            _appDbContext = appDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RegisterUser()
        {
            ViewBag.RoleList = _appDbContext.Roles.ToList();
            return View();
        }
        public async Task<IActionResult> SaveUser(RegisterViewModel registerViewModel)
        {
            var user = new IdentityUser()
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };
            var result1 = await _userManager.CreateAsync(user, registerViewModel.Password);
            //adding roles
            var result = await _userManager.CreateAsync(user, registerViewModel.RoleName);
            if (result.Succeeded && result1.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                // await _userManager.AddToRoleAsync(user, "admin");
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("RegisterUser");
        }
        public IActionResult LogInUser()
        {

            return View();
        }
        public async Task<IActionResult> LogInUsers(LoginViewModel logInViewModel)
        {
           
            var identityResult = await _signInManager.PasswordSignInAsync(logInViewModel.Email, logInViewModel.Password, false, false);
            if (identityResult.Succeeded)
            {
               
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("LogInUser");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("LogInUser");
        }
        // [Authorize(Roles ="admin")]
        public IActionResult AddRole()
        {

            return View();
        }

        public async Task<IActionResult> AddRoleDetail(RoleViewModel roleViewModel)
        {
            var role = new IdentityRole()
            {
                Name = roleViewModel.RoleName,
                
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {               
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("AddRole");
        }

    }
}

// var user = _userManager.Users.ToList();
// calling users
