using CSC.Models;
using CSC.Models.ViewModel;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Usuarios";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                var listUser = await _userServices.FindAllAsync();
                return View(listUser);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        public async Task<IActionResult> Listagem()
        {
            var listUsuarios = await _userServices.FindAllAsync();
            return Json(listUsuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Novo()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Usuarios / Novo";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                var listFunc = await _funcionarioServices.FindFuncionariosWithNoUsers();
                var ViewModel = new UserFormViewModel() { Funcionarios = listFunc };
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Novo(User user)
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                await _userServices.InsertUserAsync(user);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            User user = await _userServices.FindByIdAsync(id);
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View(user);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<IActionResult> AlterarSenha(int Id, string Senha)
        {
            try
            {
                User user = await _userServices.FindByIdAsync(Id);
                user.Senha = _userServices.GetHash(Senha);
                await _userServices.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return View("Error", e.Message);
            }
            return Json(true);
        }

        public async Task<IActionResult> AlterarNomeLogon(int Id, string NomeLogon)
        {
            try
            {
                User user = await _userServices.FindByIdAsync(Id);
                user.NomeLogon = NomeLogon;
                await _userServices.UpdateAsync(user);

            }
            catch (DbUpdateConcurrencyException e)
            {
                return View("Error", e.Message);
            }
            return Json(true);
        }
    }
}