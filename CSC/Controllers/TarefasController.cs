using CSC.Models;
using CSC.Models.Enums;
using CSC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    [Authorize]
    public class TarefasController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly AtendimentoServices _atendimentoServices;
        private readonly ClienteServices _clienteServices;
        private readonly TarefaServices _tarefaServices;
        const string SessionUserID = "_UserID";
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" };

        public TarefasController(AtendimentoServices atendimentoServices, ClienteServices clienteServices, TarefaServices tarefaServices, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<TarefasController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _atendimentoServices = atendimentoServices;
            _clienteServices = clienteServices;
            _tarefaServices = tarefaServices;
        }

        public IActionResult Index()
        {
            ViewBag.Controller = "Tarefa";
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Listagem()
        {
            return Json(await _tarefaServices.FindAllAsync(), SerializerSettings);
        }

        [HttpGet]
        public IActionResult Novo()
        {
            ViewBag.Controller = "Tarefa \\ Novo";
            Tarefa tarefa = new Tarefa();
            return View(tarefa);
        }

        [HttpPost]
        public async Task<IActionResult> Novo(Tarefa tarefa)
        {
            try
            {
                Tarefa newTarefa = new Tarefa();
                newTarefa.TarefaNumero = tarefa.TarefaNumero;
                newTarefa.Descricao = tarefa.Descricao;
                List<Atendimento> list = new List<Atendimento>();
                foreach (var item in tarefa.Atendimentos)
                {
                    var atd = await _atendimentoServices.FindByIDAsync(item.Id);
                    if (atd != null)
                    {
                        list.Add(atd);
                    }
                }
                newTarefa.Atendimentos = list;
                newTarefa.Abertura = DateTime.Now.Date;
                tarefa = null;
                _tarefaServices.Insert(newTarefa);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult VerifyTarefaNumero(string TarefaNumero)
        {
            if (_tarefaServices.FindByTarefa(TarefaNumero))
            {
                return Json($"Já existe uma tarefa com este código!");
            }
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> LocaLizaAtendimento(int id)
        {
            Atendimento atendimento = await _atendimentoServices.FindByIDAsync(id);
            return Json(atendimento, SerializerSettings);
        }

        [HttpPost]
        public async Task<JsonResult> ListaAtendimento()
        {
            var listaAtendimento = await _atendimentoServices.FindAllAsync(AtendimentoStatus.Aberto);
            return Json(listaAtendimento, SerializerSettings);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Tarefa tarefa = await _tarefaServices.FindByIdAsync(id);
            ViewBag.user = new User();
            return View(tarefa);
        }

        [HttpPost]
        public async Task<JsonResult> Concluir(int id)
        {
            try
            {
                Tarefa tarefa = await _tarefaServices.FindByIdAsync(id);
                foreach (Atendimento t in tarefa.Atendimentos)
                {
                    t.Status = AtendimentoStatus.Fechado;
                }
                tarefa.Conclusao = DateTime.Now.Date;
                _tarefaServices.Update(tarefa);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}