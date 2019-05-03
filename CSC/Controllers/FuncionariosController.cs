using CSC.Models;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    public class FuncionariosController : Controller
    {
        public readonly FuncionarioServices _funcionarioServices;
        public readonly UserServices _userServices;
        const string SessionUserID = "_UserID";
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" };


        public FuncionariosController(FuncionarioServices funcionarioServices, UserServices userServices)
        {
            _funcionarioServices = funcionarioServices;
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Funcionarios";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public IActionResult Listagem()
        {
            var listFuncionario = _funcionarioServices.FindAll();
            return Json(listFuncionario, SerializerSettings);
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                Funcionario func = await _funcionarioServices.FindByIdAsync(id);
                return View(func);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Funcionario obj)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View(obj);
            }
            await _funcionarioServices.UpdateAsync(obj);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Novo()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Funcionarios \\ Novo";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Novo(Funcionario obj)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View(); ;
            }
            await _funcionarioServices.InsertAsync(obj);
            return RedirectToAction("Index");
        }
    }
}