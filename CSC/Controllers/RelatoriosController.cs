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

        //Relatorio de Desempenho por Analista
        public IActionResult DesempenhoAnalista()
        {
            ViewBag.Controller = "Desempenho por Analista";
            var list = _userManager.Users.ToList();
            ViewBag.Funcionarios = list.Select(v => new SelectListItem
            {
                Text = v.Nome,
                Value = v.UserId.ToString()
            }).ToList();

            return View();
        }

        //Relatorio de Desempenho por Analista
        [HttpPost]
        public async Task<IActionResult> DesempenhoAnalistaPDF(int Analista, string dataInicial, string dataFinal, int tipo)
        {
            var desempenho = await _atendimentoServices.GetDesempenhoAnalistas(DateTime.ParseExact(dataInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(dataFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            if (Analista != 0)
            {
                desempenho.Where(a => a.AnalistaId == Analista).ToList();

            }
            if (tipo == 0)
            {
                return new ViewAsPdf("DesempenhoAnalistaPDF", desempenho);
            }
            else
            {
                return View("DesempenhoAnalistaPDF", desempenho);
            }
        }

        public IActionResult AtendimentosCliente()
        {
            ViewBag.Controller = "Desempenho por Analista";
            return View();
        }

        [HttpGet]
        public IActionResult Atendimentos()
        {
            var list = _userManager.Users.ToList();
            ViewBag.Funcionarios = list.Select(v => new SelectListItem
            {
                Text = v.Nome,
                Value = v.UserId.ToString()
            }).ToList();
            ViewBag.Controller = "Listagem de Atendimentos";
            return View();
        }

    }
}