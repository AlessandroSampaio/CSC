using CSC.Models;
using CSC.Models.ViewModel;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly AtendimentoServices _atendimentoServices;
        private readonly ClienteServices _clienteServices;

        public RelatoriosController(AtendimentoServices atendimentoServices, ClienteServices clienteServices)
        {
            _atendimentoServices = atendimentoServices;
            _clienteServices = clienteServices;
        }

        public IActionResult AtendimentosCliente()
        {
            ViewBag.Controller = "Relatorio de Atendimentos por Cliente";
            ViewBag.user = new User();
            return View();
        }

        public IActionResult DadosCliente()
        {
            ViewBag.Controller = "Clientes";
            ViewBag.user = new User();
            return View();
        }

        public async Task<IActionResult> DadosClientePDF(int tipo)
        {
            var _clientList = await _clienteServices.FindAllAsync();
            if (tipo == 0)
            {
                return new ViewAsPdf("DadosClientePDF", _clientList)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    CustomSwitches = "--footer-right \" [page]/[toPage]\"" +
          " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Times New Roman\""
                };
            }
            else
            {
                return View();
            }
        }

        public IActionResult DesempenhoAnalista()
        {
            ViewBag.Controller = "Atendimentos por Analistas";
            ViewBag.user = new User();
            var list = new List<User>();
            ViewBag.Funcionarios = list.Select(v => new SelectListItem
            {
                Text = v.Nome,
                Value = v.Id.ToString()
            }).ToList();

            return View();
        }

        //Relatorio de Desempenho por Analista
        [HttpPost]
        public async Task<IActionResult> DesempenhoAnalistaPDF(int Analista, string dataInicial, string dataFinal, int tipo)
        {
            DateTime DataInicial = DateTime.ParseExact(dataInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime DataFinal = DateTime.ParseExact(dataFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            List<DesempenhoAnalista> desempenhoAnalistas = new List<DesempenhoAnalista>();
            if (Analista != 0)
            {
                var list = await _atendimentoServices.FindByFuncinoarioAsync(Analista);
                if (list.Where(a => a.Abertura >= DataInicial && a.Abertura <= DataFinal).Count() > 0)
                {
                    DesempenhoAnalista desempenhoAnalista = new DesempenhoAnalista(list.Where(a => a.Abertura >= DataInicial && a.Abertura <= DataFinal).ToList(), (DataFinal - DataInicial).Days);
                    desempenhoAnalistas.Add(desempenhoAnalista);
                }
            }
            else
            {
                var listAtendimentos = await _atendimentoServices.FindByDateIntervalAsync(DataInicial, DataFinal);
                var func = listAtendimentos.Select(f => f.User).Distinct();
                foreach (User f in func)
                {
                    var list = listAtendimentos.Where(a => a.User == f).ToList();
                    if (list.Count() > 0)
                    {
                        DesempenhoAnalista desempenhoAnalista = new DesempenhoAnalista(list, (DataFinal - DataInicial).Days);
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

        public IActionResult DetalhesTarefa()
        {
            ViewBag.Controller = "Tarefas";
            ViewBag.user = new User();
            return View();
        }
    }
}