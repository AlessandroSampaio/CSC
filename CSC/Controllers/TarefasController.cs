using CSC.Models;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    public class TarefasController : Controller
    {
        private readonly AtendimentoServices _atendimentoServices;
        private readonly ClienteServices _clienteServices;
        private readonly TarefaServices _tarefaServices;
        private readonly UserServices _userServices;
        const string SessionUserID = "_UserID";
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" };

        public TarefasController(AtendimentoServices atendimentoServices, ClienteServices clienteServices, UserServices userServices, TarefaServices tarefaServices)
        {
            _atendimentoServices = atendimentoServices;
            _clienteServices = clienteServices;
            _tarefaServices = tarefaServices;
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                User user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                ViewBag.user = user;
                ViewBag.Controller = "Tarefas";

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<JsonResult> Listagem()
        {
            return Json(await _tarefaServices.FindAllAsync(), SerializerSettings);
        }

        [HttpGet]
        public async Task<IActionResult> Novo()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                User user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                ViewBag.user = user;
                ViewBag.Controller = "Tarefas \\ Novo";
                Tarefa tarefa = new Tarefa();
                return View(tarefa);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
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
                foreach(var item in tarefa.Atendimentos)
                {
                    var atd = await _atendimentoServices.FindByIDAsync(item.Id);
                    if(atd != null)
                    {
                        list.Add(atd);
                    }
                }
                newTarefa.Atendimentos = list;
                newTarefa.Abertura = DateTime.Now.Date;
                tarefa = null;
                _tarefaServices.Insert(newTarefa);
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View(nameof(Index));
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<JsonResult> LocaLizaAtendimento(int id)
        {
            Atendimento atendimento = await _atendimentoServices.FindByIDAsync(id);
            return Json(atendimento, SerializerSettings);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Tarefa tarefa = await _tarefaServices.FindByIdAsync(id);
            ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
            return View(tarefa);
        }
        
    }
}