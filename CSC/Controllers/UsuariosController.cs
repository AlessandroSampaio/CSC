using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSC.Controllers
{
    public class UsuariosController : Controller
    {
        public readonly UserServices _userServices;
        const string SessionUserID = "_UserID";

        public UsuariosController(UserServices userServices)
        {
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            var listUser = await _userServices.FindAllAsync();
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(listUser);
        }


        public async Task<IActionResult> Listagem(string _name)
        {
            if (_name == null) { _name = ""; }
            var listUsuarios = await _userServices.FindByNameAsync(_name);
            return PartialView("_listUsuarios", listUsuarios);
        }
    }
}