using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSC.Models;
using CSC.Models.ViewModel;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSC.Controllers
{
    public class UsuariosController : Controller
    {
        public readonly FuncionarioServices _funcionarioServices;
        public readonly UserServices _userServices;
        const string SessionUserID = "_UserID";

        public UsuariosController(UserServices userServices, FuncionarioServices funcionarioServices)
        {
            _funcionarioServices = funcionarioServices;
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var listFunc = await _funcionarioServices.FindFuncionariosWithNoUsers();
            var ViewModel = new UserFormViewModel() { Funcionarios = listFunc };
            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
             
            await _userServices.InsertUserAsync(user);
            return RedirectToAction("Index");
        }
    }
}