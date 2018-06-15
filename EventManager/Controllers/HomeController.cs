using DataAccess.Models;
using EventManager.Helpers;
using EventManager.ViewModels.Home;
using Repositories;
using Services;
using System.Linq;
using System.Web.Mvc;

namespace EventManager.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        #region Constructors and fields
        private UserService userService;

        public HomeController()
        {
            ModelStateWrapper modelStateWrapper = new ModelStateWrapper(ModelState);
            this.userService = new UserService(modelStateWrapper, new UserRepository());
        }
        #endregion

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
            if (!userService.PreValidate())
            {
                return View(registerViewModel);
            }

            User user = userService.GetAll(x => x.Username == registerViewModel.Username).FirstOrDefault();
            if (user != null)
            {
                userService.AddValidationError("", "A user with this username already exists!");
                return View(registerViewModel);
            }

            User userDb = new User
            {
                Username = registerViewModel.Username
            };
            bool isSaved = userService.RegisterUser(userDb, registerViewModel.Password) > 0;
            if (!isSaved)
            {
                userService.AddValidationError("", "Ooops something went wrong!");
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
            if (!userService.PreValidate())
            {
                return View(loginViewModel);
            }
            User userDb = userService.GetUserByNameAndPassword(loginViewModel.Username, loginViewModel.Password);
            if (userDb == null)
            {
                userService.AddValidationError("", "Invalid username and/or password");
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