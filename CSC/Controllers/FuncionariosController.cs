﻿using CSC.Models;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    public class FuncionariosController : Controller
    {
        public readonly FuncionarioServices _funcionarioServices;
        public readonly UserServices _userServices;
        const string SessionUserID = "_UserID";


        public FuncionariosController(FuncionarioServices funcionarioServices, UserServices userServices)
        {
            _funcionarioServices = funcionarioServices;
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> Listagem(string _name)
        {
            if (_name == null) { _name = ""; }
            var listFuncionario = await _funcionarioServices.FindByNameAsync(_name);
            return PartialView("_listFuncionarios", listFuncionario);
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
            }
            Funcionario func = await _funcionarioServices.FindByIdAsync(id);
            return View(func);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Funcionario obj)
        {
            await _funcionarioServices.UpdateAsync(obj);
            return View(nameof(Index));

        }

        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Funcionario obj)
        {
            await _funcionarioServices.InsertAsync(obj);
            return View(nameof(Index));
        }
    }
}