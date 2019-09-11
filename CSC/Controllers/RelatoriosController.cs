using CSC.Models;
using CSC.Models.Enums;
using CSC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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

        [HttpPost]
        public async Task<IActionResult> Atendimentos(string CNPJ, int Analista, int Status, int Tipo, DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                
                return new ViewAsPdf("Atendimentos");
            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        #region Helper
        public Func<TSource, bool> DynamicFilter<TSource>(string field, string value)
        {
            var type = typeof(TSource);
            var pe = Expression.Parameter(type, field);
            var dates = type.GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime));
            Expression busca = Expression.Constant(value);
            Expression selectLeft = null;
            Expression selectRight = null;
            Expression filterExpression = null;
            foreach (var date in dates)
            {
                Expression left = Expression.Property(pe, value);
                Expression comparison =
                    Expression.Equal(left, busca);
                if (selectLeft == null)
                {
                    selectLeft = comparison;
                    filterExpression = selectLeft;
                    continue;
                }
                if (selectRight == null)
                {
                    selectRight = comparison;
                    filterExpression =
                        Expression.AndAlso(selectLeft, selectRight);
                    continue;
                }
                filterExpression =
                    Expression.AndAlso(filterExpression, comparison);
            }
            return Expression.Lambda<Func<TSource, bool>>
                (filterExpression, pe).Compile();
        }

        #endregion

    }
}