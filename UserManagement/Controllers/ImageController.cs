using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;

        public ImageController(ILogger<ImageController> logger, ApplicationDbContext appDbContext)
        {

            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddImage()
        {
            return View();
        }
        public IActionResult SaveImage(Images image)
        {            
                _appDbContext.Images.Add(image);
                _appDbContext.SaveChanges();
                return Redirect("AddImage");                    
        }
    }
}
