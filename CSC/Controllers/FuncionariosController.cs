using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSC.Models;
using CSC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSC.Controllers
{
    public class FuncionariosController : Controller
    {
        public readonly FuncionarioServices _funcionarioServices;


        public FuncionariosController(FuncionarioServices funcionarioServices)
        {
            _funcionarioServices = funcionarioServices;
        }

        public IActionResult Index()
        {            
            return View();
        }

        public async Task<IActionResult> Listagem(string _name)
        {
            if(_name == null) { _name = ""; }
            var listFuncionario = await _funcionarioServices.FindByNameAsync(_name);
            return PartialView("_listFuncionarios", listFuncionario);
        }

        public async Task<IActionResult> Editar(int id)
        {
            Funcionario func = await _funcionarioServices.FindByIdAsync(id);
            return View(func);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Funcionario obj)
        {
            await _funcionarioServices.UpdateAsync(obj);
            return View(nameof(Index));

        }

        public IActionResult Create()
        {
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