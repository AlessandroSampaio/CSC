﻿using CSC.Models;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    public class ClientesController : Controller
    {
        public readonly ClienteServices _clienteServices;
        public readonly UserServices _userServices;
        const string SessionUserID = "_UserID";
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" };

        public ClientesController(UserServices userServices, ClienteServices clienteServices)
        {
            _clienteServices = clienteServices;
            _userServices = userServices;
        }


        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Cliente";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                ViewBag.erro = TempData["Error"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<IActionResult> Listagem()
        {
            var list = await _clienteServices.FindAllAsync();
            return Json(list, SerializerSettings);
        }

        [HttpGet]
        public async Task<IActionResult> Novo(Cliente cliente)
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                ViewBag.cnpjWS = TempData["cnpjWS"];
                ViewBag.Controller = "Clientes \\ Novo";
                return View(cliente);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SalvarNovo(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View("Novo", cliente);
            }
            await _clienteServices.InsertAsync(cliente);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ConsultaCNPJ(string _cnpj)
        {
            try
            {
                Cliente cliente = await _clienteServices.ConsultaWS(_cnpj);
                cliente.DataInicio = DateTime.Now.Date;
                TempData["cnpjWS"] = "true";
                return RedirectToAction("Novo", cliente);
            } catch (NotImplementedException e)
            {
                TempData["Error"] = e.Message.Replace('á','a');
                return RedirectToAction("index");
            }

        }
    }
}