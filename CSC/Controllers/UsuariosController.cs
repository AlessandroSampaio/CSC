using CSC.Models;
using CSC.Models.Enums;
using CSC.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" };

        public UsuariosController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<UsuariosController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult Index()
        {
            ViewBag.Controller = "Usuários";
            return View();
        }

        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Listagem()
        {
            try
            {
                var listUsers = await _userManager.Users.OrderBy(u => u.UserId).ToListAsync();
                listUsers.RemoveAt(0);
                UserIndexViewModel[] userIndexViewModels = new UserIndexViewModel[listUsers.Count];
                if (listUsers.Count > 0)
                {
                    int i = 0;
                    foreach (User user in listUsers)
                    {
                        userIndexViewModels[i] = new UserIndexViewModel(user);
                        i++;
                    }
                    return Json(userIndexViewModels, SerializerSettings);
                }
                else
                {
                    return Json(userIndexViewModels, SerializerSettings);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult Novo()
        {
            ViewBag.Controller = "Usuarios / Novo";
            ViewBag.Roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Novo(UserViewModel userView)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Admissao = userView.Admissao,
                    Email = userView.Email,
                    Nome = userView.Nome,
                    UserName = userView.UserName
                };
                var newUser = await _userManager.CreateAsync(user, userView.Password);
                if (newUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, userView.Role.ToString());
                }
                else
                {
                    ModelState.AddModelError(string.Empty, newUser.Errors.ToString());
                    return View(userView);
                }
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Não foi possivel criar o usuario");
                return View(userView);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var role = await _userManager.GetRolesAsync(user);
                UserViewModel userView = new UserViewModel(user, (Roles)Enum.Parse(typeof(Roles), role[0]));
                ViewBag.Controller = "Usuarios / Perfil";
                return View(userView);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Perfil(UserViewModel userView)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(userView.Id);
                user.Nome = userView.Nome;
                user.Email = userView.Email;
                user.UserName = userView.UserName;
                var update = await _userManager.UpdateAsync(user);
                if (update.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(update);
                    return View(userView);
                }

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> AlterarUserName(string userName, string newUserName)
        {
            if (newUserName == null)
            {
                throw new ArgumentNullException(nameof(newUserName));
            }

            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                var update = await _userManager.SetUserNameAsync(user, newUserName);
                if (update.Succeeded)
                {
                    return Json(true);
                }
                else
                {
                    return Json(update.ToString());
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AlterarSenha(string userName, string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                var loggedUser = await _userManager.GetUserAsync(User);
                var isAdmin = await _userManager.GetRolesAsync(loggedUser);

                if (user != loggedUser && !isAdmin.Contains("Admin"))
                {
                    return StatusCode(403);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var update = await _userManager.ResetPasswordAsync(user, token, password);

                if (update.Succeeded)
                {
                    return Json(true);
                }
                else
                {
                    return Json(update.ToString());
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}