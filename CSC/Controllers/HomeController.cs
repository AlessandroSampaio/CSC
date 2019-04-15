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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            user = _UserServices.ValidUser(user);
            HttpContext.Session.SetInt32(SessionUserID, user.Id);
            return  RedirectToAction("Index");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyLogon(string NomeLogon, string Senha)
        {
            User user = new User(NomeLogon, Senha);
            if (_UserServices.ValidUser(user) == null)
            {
                return Json($"O usuario ou a senha estão incorretos!");
            }
            return Json(true);
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.user = await _UserServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
           
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Logout(int id)
        {
            HttpContext.Session.Remove(SessionUserID);
            return RedirectToAction("Login");
        }
    }
}