using CSC.Models;
using CSC.Models.Enums;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    public class AtendimentosController : Controller
    {
        public readonly UserServices _userServices;
        public readonly AtendimentoServices _atendimentoServices;
        public readonly ClienteServices _clienteServices;
        public readonly FuncionarioServices _funcionarioServices;
        const string SessionUserID = "_UserID";
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" };

        public AtendimentosController(UserServices userServices, AtendimentoServices atendimentoServices, ClienteServices clienteServices, FuncionarioServices funcionarioServices)
        {
            _userServices = userServices;
            _atendimentoServices = atendimentoServices;
            _clienteServices = clienteServices;
            _funcionarioServices = funcionarioServices;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                User user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                ViewBag.user = user;
                ViewBag.Controller = "Atendimentos";
                var list = _funcionarioServices.FindAll().ToList();
                list.Remove(user.Funcionario);
                ViewBag.Funcionarios = list.Select(v => new SelectListItem
                {
                    Text = v.Nome,
                    Value = v.Id.ToString()
                }).ToList();


                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<IActionResult> Listagem()
        {
            var list = await _atendimentoServices.FindAllAsync();
            return Json(list, SerializerSettings);
        }

        [HttpGet]
        public async Task<IActionResult> Novo(int ClienteId)
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                User user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                ViewBag.Controller = "Atendimentos \\ Novo";
                ViewBag.user = user;
                ViewBag.TipoAtendimento = Enum.GetValues(typeof(TipoAtendimento)).Cast<TipoAtendimento>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();
                Atendimento atendimento = new Atendimento {
                    Funcionario = user.Funcionario,
                    FuncionarioId = user.FuncionarioId,
                    Cliente = await _clienteServices.FindByIdAsync(ClienteId),
                    ClienteId = ClienteId,
                    Abertura = DateTime.Now.Date
                };
                return View(atendimento);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Novo(Atendimento atendimento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View(atendimento);
            }
            await _atendimentoServices.InsertAsync(atendimento);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                Atendimento atendimento = await _atendimentoServices.FindByIDAsync(id);
                if (atendimento.Status != AtendimentoStatus.Aberto)
                {
                    return RedirectToAction("Index");
                }
                User user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                ViewBag.Controller = "Atendimentos \\ Novo";
                ViewBag.user = user;
                ViewBag.TipoAtendimento = Enum.GetValues(typeof(TipoAtendimento)).Cast<TipoAtendimento>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();
                return View(atendimento);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Atendimento obj)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View(obj);
            }
            await _atendimentoServices.UpdateAsync(obj);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<int> Notificacoes(int id)
        {
            var list = await _atendimentoServices.FindByClientAsync(id);
            return list.Where(a => a.Status == AtendimentoStatus.Aberto).Count();
        }

        [HttpPost]
        public async Task<IActionResult> AtendimentoHistorico(int id)
        {
            var list = await _atendimentoServices.FindByClientAsync(id);
            list.OrderBy(a => a.Status);
            return Json(list, SerializerSettings);
        }

        [HttpPost]
        public async Task<JsonResult> TransferirAtendimento(int atdId, int funcionarioDestino)
        {
            Atendimento atdOrig = await _atendimentoServices.FindByIDAsync(atdId);
            Atendimento atdDest = new Atendimento {
                Abertura = atdOrig.Abertura,
                ClienteId = atdOrig.ClienteId,
                AtendimentoTipo = atdOrig.AtendimentoTipo,
                FuncionarioId = funcionarioDestino,
                Detalhes = atdOrig.Detalhes,
                OrigemID = atdOrig.Id
            };
            await _atendimentoServices.InsertAsync(atdDest);
            atdOrig.Status = AtendimentoStatus.Transferido;
            await _atendimentoServices.UpdateAsync(atdOrig);
            return Json(true);
        }

    }
}