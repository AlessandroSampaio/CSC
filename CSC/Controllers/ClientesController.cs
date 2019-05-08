using CSC.Models;
using CSC.Models.Enums;
using CSC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        public async Task<IActionResult> Novo(string _doc)
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                try
                {
                    ViewBag.Controller = "Cliente \\ Novo";
                    ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                    Cliente cliente;
                    _doc = _doc.Replace(".", "").Replace("-", "").Replace("/", "");
                    if (_doc.Length < 14)
                    {
                        ViewBag.cnpjWS = "true";
                        ViewBag.Type = 'f';
                        cliente = new Cliente { CNPJ = _doc };
                        return View(cliente);
                    }
                    ViewBag.Type = 'j';
                    cliente = await _clienteServices.ConsultaWS(_doc);
                    ViewBag.cnpjWS = "true";
                    return View(cliente);
                }
                catch (NotImplementedException e)
                {
                    throw e;
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(Cliente cliente)
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
        public async Task<JsonResult> ConsultaCliente(string _doc)
        {
            _doc = _doc.Replace(".", "").Replace("/", "").Replace("-", "");
            Cliente cliente = await _clienteServices.FindByDocAsync(_doc);
            return cliente == null ? Json(false) : Json(true);
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Clientes \\ Editar";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                Cliente cliente = await _clienteServices.FindByIdAsync(id);
                if (cliente.CNPJ.Length == 11) { ViewBag.Type = 'f'; }
                else { ViewBag.Type = 'j'; }

                return View(cliente);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Cliente obj)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                return View(obj);
            }
            await _clienteServices.UpdateAsync(obj);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Inventario(int id)
        {
            if (HttpContext.Session.GetInt32(SessionUserID).HasValue)
            {
                ViewBag.Controller = "Clientes \\ Inventario";
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
                ViewBag.SelectListItem = Enum.GetValues(typeof(Software)).Cast<Software>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

                Cliente cliente = await _clienteServices.FindByIdAsync(id);
                return View(cliente);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Inventario(List<Inventario> inventarios)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.user = await _userServices.FindByIdAsync(HttpContext.Session.GetInt32(SessionUserID).Value);
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
    }
}