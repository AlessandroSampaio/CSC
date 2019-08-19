using CSC.Models;
using CSC.Models.Enums;
using CSC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        public readonly ClienteServices _clienteServices;
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" };

        public ClientesController(ClienteServices clienteServices)
        {
            _clienteServices = clienteServices;
        }

        public IActionResult Index()
        {
            ViewBag.Controller = "Cliente";
            return View();
        }

        public async Task<IActionResult> Listagem()
        {
            var list = await _clienteServices.FindAllAsync();
            return Json(list, SerializerSettings);
        }

        [HttpPost]
        public async Task<IActionResult> Novo(string _doc)
        {
            try
            {
                ViewBag.Controller = "Cliente \\ Novo";
                ViewBag.user = new User();
                Cliente cliente;
                _doc = _doc.Replace(".", "").Replace("-", "").Replace("/", "");
                if (_doc.Length < 14)
                {
                    ViewBag.Type = 'f';
                    cliente = new Cliente { CNPJ = _doc };
                    return View(cliente);
                }
                ViewBag.Type = 'j';
                cliente = await _clienteServices.ConsultaWS(_doc);
                return View(cliente);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public IActionResult Salvar(Cliente cliente)
        {
            try
            {
                ModelState.Remove("DataInicio");
                if (!ModelState.IsValid)
                {
                    ViewBag.user = new User();
                    return View(cliente);
                }
                _clienteServices.Insert(cliente);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<JsonResult> ConsultaCliente(string _doc)
        {
            _doc = _doc.Replace(".", "").Replace("/", "").Replace("-", "");
            Cliente cliente = await _clienteServices.FindByDocAsync(_doc);
            return cliente == null ? Json(false) : Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            ViewBag.Controller = "Clientes \\ Editar";
            ViewBag.user = new User();
            Cliente cliente = await _clienteServices.FindByIdAsync(id);
            if (cliente.CNPJ.Length == 11) { ViewBag.Type = 'f'; }
            else { ViewBag.Type = 'j'; }

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Cliente obj)
        {
            ModelState.Remove("DataInicio");
            if (!ModelState.IsValid)
            {
                ViewBag.user = new User();
                return View(obj);
            }
            await _clienteServices.UpdateAsync(obj);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Inventario(int id)
        {
            ViewBag.Controller = "Clientes \\ Inventario";
            ViewBag.SelectListItem = Enum.GetValues(typeof(Software)).Cast<Software>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            Cliente cliente = await _clienteServices.FindByIdAsync(id);
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Inventario(List<Inventario> inventarios)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.user = new User();
                return View(inventarios);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> InventarioListagem(int id)
        {
            var list = await _clienteServices.FindInventariosAsync(id);
            return Json(list, SerializerSettings);
        }

        [HttpPost]
        public async Task<IActionResult> AddInventario(int id, Software software, int quantidade)
        {
            Inventario inv = new Inventario
            {
                ClienteID = id,
                Cliente = await _clienteServices.FindByIdAsync(id),
                Software = software,
                Quantidade = quantidade
            };
            await _clienteServices.SaveInventario(inv);
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveInventario(Inventario inv)
        {
            await _clienteServices.RemoveInventario(inv);
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> Inativar(int id)
        {
            try
            {
                Cliente cliente = await _clienteServices.FindByIdAsync(id);
                cliente.Status = PessoaStatus.Inativo;
                await _clienteServices.UpdateAsync(cliente);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}