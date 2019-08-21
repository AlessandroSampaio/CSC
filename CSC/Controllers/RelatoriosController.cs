using CSC.Models;
using CSC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    [Authorize(Roles = "Admin, Supervisor")]
    public class RelatoriosController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly AtendimentoServices _atendimentoServices;
        private readonly ClienteServices _clienteServices;

        public RelatoriosController(AtendimentoServices atendimentoServices, ClienteServices clienteServices, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<RelatoriosController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _clienteServices = clienteServices;
            _atendimentoServices = atendimentoServices;
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
            var desempenho = await _atendimentoServices.GetDesempenhoAnalistas(DateTime.ParseExact(dataInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(dataFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            if (tipo == 0)
            {
                return new ViewAsPdf("DesempenhoAnalistaPDF", desempenho);
            }
            else
            {
                return View("DesempenhoAnalistaPDF", desempenho);
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