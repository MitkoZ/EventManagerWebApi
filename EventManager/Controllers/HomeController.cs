using DataAccess.Models;
using EventManager.Helpers;
using EventManager.ViewModels;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManager.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            UserRepository userRepository = new UserRepository();
            User user = userRepository.GetAll(x => x.Username == registerViewModel.Username).FirstOrDefault();
            if (user != null)
            {
                ModelState.AddModelError("", "A user with this username already exists!");
                return View(registerViewModel);
            }

            User userDb = new User();
            userDb.Username = registerViewModel.Username;
            bool isSaved = userRepository.RegisterUser(userDb, registerViewModel.Password) > 0;
            if (!isSaved)
            {
                ModelState.AddModelError("", "Ooops something went wrong!");
                return View();
            }
            TempData["SuccessMessage"] = "Registered successfully!";
            return View();

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            UserRepository userRepository = new UserRepository();
            User userDb = userRepository.GetUserByNameAndPassword(loginViewModel.Username, loginViewModel.Password);
            if (userDb == null)
            {
                ModelState.AddModelError("", "Invalid username and/or password");
                return View();
            }
            LoginUserSession.Current.SetCurrentUser(userDb.Id, userDb.Username);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            LoginUserSession.Current.Logout();
            return RedirectToAction("Index", "Home");
        }

    }
}