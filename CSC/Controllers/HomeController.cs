using CSC.Models;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    public class HomeController : Controller
    {
        public readonly UserServices _UserServices;
        const string SessionUserID = "_UserID";
        const string SessionUserTime = "_LogonTime";


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
            HttpContext.Session.SetString(SessionUserTime, DateTime.Now.ToString());
            return RedirectToAction("Index");
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
                ViewBag.Controller = "Painel de Controle";
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

        [HttpGet]
        public async Task<IActionResult> SessionTime()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.user = await _UserServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                TimeSpan TimeLogon = DateTime.Now - DateTime.Parse(HttpContext.Session.GetString(SessionUserTime).ToString());

                return Json(TimeLogon.Minutes);
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }


    }
}