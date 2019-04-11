using CSC.Models;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    public class HomeController : Controller
    {
        public readonly UserServices _UserServices;
        const string SessionUserID = "_UserID";


        public HomeController(UserServices UserServices)
        {
            _UserServices = UserServices;
        }


        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
               ViewBag.user = await _UserServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
            }
            return View();
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Logon(string nomeLogon, string senha)
        {
            User user = _UserServices.ValidUser(nomeLogon, senha);
            if(user != null)
            {
                HttpContext.Session.SetInt32(SessionUserID, user.Id);

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Logout(int id)
        {
            HttpContext.Session.Remove(SessionUserID);
            return RedirectToAction("Index");
        }
    }
}