﻿using CSC.Models;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    public class AtendimentosController : Controller
    {
        public readonly UserServices _userServices;
        public readonly AtendimentoServices _atendimentoServices;
        public readonly ClienteServices _clienteServices;
        const string SessionUserID = "_UserID";
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" };

        public AtendimentosController(UserServices userServices, AtendimentoServices atendimentoServices, ClienteServices clienteServices)
        {
            _userServices = userServices;
            _atendimentoServices = atendimentoServices;
            _clienteServices = clienteServices;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Atendimentos";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
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
        public async Task<IActionResult> Novo()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                User user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                ViewBag.Controller = "Atendimentos \\ Novo";
                ViewBag.user = user;
                Atendimento atendimento = new Atendimento {
                    Funcionario = user.Funcionario,
                    Cliente = await _clienteServices.FindByIdAsync(12)
                };
                return View(atendimento);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}