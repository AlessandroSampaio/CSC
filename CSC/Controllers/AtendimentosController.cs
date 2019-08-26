using CSC.Models;
using CSC.Models.Enums;
using CSC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    [Authorize]
    public class AtendimentosController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        public readonly AtendimentoServices _atendimentoServices;
        public readonly ClienteServices _clienteServices;
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" };

        public AtendimentosController(AtendimentoServices atendimentoServices, ClienteServices clienteServices, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AtendimentosController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _atendimentoServices = atendimentoServices;
            _clienteServices = clienteServices;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Controller = "Atendimentos";
            var users = _userManager.Users;
            ViewBag.Funcionarios = await users.Select(v => new SelectListItem
            {
                Text = v.Nome,
                Value = v.Id.ToString()
            }).ToListAsync();


            return View();
        }

        public async Task<IActionResult> Listagem()
        {
            var list = await _atendimentoServices.FindAllAsync();
            return Json(list, SerializerSettings);
        }

        [HttpGet]
        public async Task<IActionResult> Novo(int ClienteId)
        {
            try
            {
                //Busca usuario logado
                var user = await _userManager.GetUserAsync(User);
                ViewBag.Controller = "Atendimentos \\ Novo";
                Cliente cliente = await _clienteServices.FindByIdAsync(ClienteId);
                Atendimento atendimento = new Atendimento
                {
                    Cliente = cliente,
                    ClienteId = ClienteId,
                    Abertura = DateTime.Now.Date,
                    User = user,
                    UserId = user.Id
                };
                if (!cliente.Mono)
                {
                    ViewBag.TipoAtendimento = Enum.GetValues(typeof(TipoAtendimento)).Cast<TipoAtendimento>().Select(v => new SelectListItem
                    {
                        Text = v.ToString(),
                        Value = ((int)v).ToString()
                    }).ToList();
                }
                else
                {
                    ViewBag.TipoAtendimento = Enum.GetValues(typeof(TipoAtendimento)).Cast<TipoAtendimento>().Where(t => t.Equals(TipoAtendimento.Chave)).Select(v => new SelectListItem
                    {
                        Text = v.ToString(),
                        Value = ((int)v).ToString()
                    }).ToList();
                }
                return View(atendimento);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Novo(Atendimento atendimento)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                atendimento.User = user;
                atendimento.UserId = user.Id;
                await _atendimentoServices.InsertAsync(atendimento);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Atendimento atendimento = await _atendimentoServices.FindByIDAsync(id);
            if (atendimento.Status != AtendimentoStatus.Aberto)
            {
                return RedirectToAction("Index");
            }
            User user = new User();
            ViewBag.Controller = "Atendimentos \\ Novo";
            ViewBag.user = user;
            ViewBag.TipoAtendimento = Enum.GetValues(typeof(TipoAtendimento)).Cast<TipoAtendimento>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return View(atendimento);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Atendimento obj)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.user = new User();
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
        public async Task<JsonResult> TransferirAtendimento(int atdId, string funcionarioDestino)
        {
            try
            {
                var newUser = await _userManager.FindByIdAsync(funcionarioDestino);
                if (newUser != null)
                {
                    Atendimento atdOrig = await _atendimentoServices.FindByIDAsync(atdId);
                    Atendimento atdDest = new Atendimento
                    {
                        Abertura = atdOrig.Abertura,
                        ClienteId = atdOrig.ClienteId,
                        AtendimentoTipo = atdOrig.AtendimentoTipo,
                        UserId = newUser.Id,
                        User = newUser,
                        Detalhes = atdOrig.Detalhes,
                        OrigemID = atdOrig.Id
                    };
                    await _atendimentoServices.InsertAsync(atdDest);
                    atdOrig.Status = AtendimentoStatus.Transferido;
                    await _atendimentoServices.UpdateAsync(atdOrig);
                    return Json(true);
                }
                else
                {
                    return Json("Usuario não encontrado");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> EncerrarAtendimento(int atdId, string detalhes)
        {
            Atendimento atd = await _atendimentoServices.FindByIDAsync(atdId);
            atd.Detalhes += '\n' + detalhes;
            atd.Status = AtendimentoStatus.Fechado;
            await _atendimentoServices.UpdateAsync(atd);
            return Json(true);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<JsonResult> ReabrirAtendimento(int atdId)
        {
            try
            {
                var atendimento = await _atendimentoServices.FindByIDAsync(atdId);
                atendimento.Status = AtendimentoStatus.Aberto;
                await _atendimentoServices.UpdateAsync(atendimento);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SituacaoAtendimentosChart(string dataIni, string dataFim)
        {
            DateTime DataInicio;
            DateTime DataFinal;
            if (dataIni == null || dataFim == null)
            {
                DataInicio = DateTime.Now.Date;
                DataFinal = DateTime.Now.Date;
            }
            else
            {
                DataInicio = DateTime.ParseExact(dataIni, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DataFinal = DateTime.ParseExact(dataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            var lista = await _atendimentoServices.TotalizacaoAtendimentosAsync();
            return Json(lista.Where(a => a.Abertura >= DataInicio && a.Abertura <= DataFinal).GroupBy(s => s.Status).
                Select(s => new { Tipo = s.Key, Contador = s.Count() }), SerializerSettings);
        }

        [HttpPost]
        public async Task<JsonResult> AtendimentosPorFuncionario(string dataIni, string dataFim)
        {
            DateTime DataInicio;
            DateTime DataFinal;
            if (dataIni == null || dataFim == null)
            {
                DataInicio = DateTime.Now.Date;
                DataFinal = DateTime.Now.Date;
            }
            else
            {
                DataInicio = DateTime.ParseExact(dataIni, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DataFinal = DateTime.ParseExact(dataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            var lista = await _atendimentoServices.FindAllAsync();
            return Json(lista.Where(a => a.Abertura >= DataInicio && a.Abertura <= DataFinal).GroupBy(func => func.User.Nome)
                .Select(funcionario => new { Funcionario = funcionario.Key, Atendimentos = funcionario.Count() }));
        }

        [HttpPost]
        public async Task<JsonResult> AtendimentosPorCategoria(string dataIni, string dataFim)
        {
            DateTime DataInicio;
            DateTime DataFinal;
            if (dataIni == null || dataFim == null)
            {
                DataInicio = DateTime.Now;
                DataFinal = DateTime.Now;
            }
            else
            {
                DataInicio = DateTime.ParseExact(dataIni, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DataFinal = DateTime.ParseExact(dataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            var lista = await _atendimentoServices.FindByDateIntervalAsync(DataInicio, DataFinal);
            int[] CategoriasCount = new int[4];
            for (int x=0; x < 4; x++)
            {
                CategoriasCount.SetValue((lista.Where(a => a.AtendimentoTipo == (TipoAtendimento)x).Count()), x);
            }

            return Json(CategoriasCount);
        }
    }
}