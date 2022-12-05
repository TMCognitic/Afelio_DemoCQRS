using Afelio_DemoCQRS.Dal.Commands;
using Afelio_DemoCQRS.Dal.Entities;
using Afelio_DemoCQRS.Dal.Queries;
using Afelio_DemoCQRS.Models.Forms;

using Microsoft.AspNetCore.Mvc;
using Tools.CQRS;
using Tools.CQRS.Commands;
using Tools.CQRS.Queries;

namespace Afelio_DemoCQRS.Controllers
{
    public class AuthController : Controller
    {
        private readonly Dispatcher _dispatcher;

        public AuthController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginForm form)
        {
            if(!ModelState.IsValid)
            {
                return View(form);
            }

            Utilisateur? utilisateur = _dispatcher.Dispatch(new LoginQuery(form.Email, form.Passwd));

            if(utilisateur is null) 
            {
                ModelState.AddModelError("", "Erreur Email/Mot de passe...");
                return View(form);
            }

            HttpContext.Session.SetString("FullName", $"{utilisateur.Nom} {utilisateur.Prenom}");            

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            Result<int> result = _dispatcher.Dispatch(new RegisterCommand(form.Nom, form.Prenom, form.Email, DateOnly.FromDateTime(form.Anniversaire), form.Passwd));

            if(result.IsFailure)
            {
                ModelState.AddModelError("", result.Message!);
            }

            TempData["Id"] = result.Value;
            return RedirectToAction(nameof(Login));
        }
    }
}
