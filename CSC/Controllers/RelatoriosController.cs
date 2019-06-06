using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSC.Models;
using CSC.Models.ViewModel;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

namespace CSC.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly AtendimentoServices _atendimentoServices;
        private readonly FuncionarioServices _funcionarioServices;
        private readonly UserServices _userServices;
        const string SessionUserID = "_UserID";

        public RelatoriosController(AtendimentoServices atendimentoServices, FuncionarioServices funcionarioServices, UserServices userServices)
        {
            _atendimentoServices = atendimentoServices;
            _funcionarioServices = funcionarioServices;
            _userServices = userServices;
        }


        public async Task<IActionResult> AtendimentosCliente()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Relatorio de Atendimentos por Cliente";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<IActionResult> DadosCliente()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Clientes";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<IActionResult> DesempenhoAnalista()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Atendimentos por Analistas";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                var list = _funcionarioServices.FindAll().ToList();
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

        //Relatorio de Desempenho por Analista
        [HttpPost]
        public async Task<IActionResult> DesempenhoAnalistaPDF(int Analista, DateTime DataInicial, DateTime DataFinal, int tipo)
        {
            List<DesempenhoAnalista> desempenhoAnalistas = new List<DesempenhoAnalista>();
            if (Analista != 0)
            {
                var list = await _atendimentoServices.FindByFuncinoarioAsync(Analista);
                if (list.Where(a => a.Abertura > DataInicial && a.Abertura < DataFinal).Count() > 0)
                {
                    DesempenhoAnalista desempenhoAnalista = new DesempenhoAnalista(list.Where(a => a.Abertura > DataInicial && a.Abertura < DataFinal).ToList(), (DataFinal - DataInicial).Days);
                    desempenhoAnalistas.Add(desempenhoAnalista);
                }
            }
            else
            {
                var func = _funcionarioServices.FindAll();
                foreach(Funcionario f in func)
                {
                    var list = await _atendimentoServices.FindByFuncinoarioAsync(f.Id);
                    if (list.Where(a => a.Abertura > DataInicial && a.Abertura < DataFinal).Count() > 0)
                    {
                        DesempenhoAnalista desempenhoAnalista = new DesempenhoAnalista(list.Where(a => a.Abertura > DataInicial && a.Abertura < DataFinal).ToList(), (DataFinal - DataInicial).Days);
                        desempenhoAnalistas.Add(desempenhoAnalista);
                    }
                }
            }
            if (tipo == 0)
            {
                return new ViewAsPdf("DesempenhoAnalistaPDF", desempenhoAnalistas);
            }
            else
            {
                return View("DesempenhoAnalistaPDF", desempenhoAnalistas);
            }
        }

        public async Task<IActionResult> DetalhesTarefa()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Tarefas";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}