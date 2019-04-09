using CSC.Models;
using CSC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    public class HomeController : Controller
    {
        public readonly UserServices _UserServices;


        public HomeController(UserServices UserServices)
        {
            _UserServices = UserServices;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Logon(User user)
        {
            await _UserServices.InsertUserAsync(user);
            return RedirectToAction("Index");
        }
    }
}