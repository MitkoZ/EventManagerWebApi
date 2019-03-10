using DataAccess.Models;
using EventManager.ViewModels.Home;
using EventManagerWebApi.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Linq;

namespace EventManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Constructors and fields
        private readonly UserService userService;
        private readonly ModelStateWrapper modelStateWrapper;
        private readonly SecurityService securityService;

        public AuthController(UserService userService, SecurityService securityService)
        {
            this.modelStateWrapper = new ModelStateWrapper(ModelState);
            userService.ValidationDictionary = modelStateWrapper;
            this.userService = userService;

            this.securityService = securityService;
        }
        #endregion

        [HttpPost("Register")]
        public ActionResult Register([FromBody]RegisterViewModel registerViewModel)
        {
            if (!userService.PreValidate())
            {
                return BadRequest(ModelState);
            }

            User user = userService.GetAll(x => x.Username == registerViewModel.Username).FirstOrDefault();
            if (user != null)
            {
                return BadRequest("A user with this username already exists!");
            }

            User userDb = new User
            {
                Username = registerViewModel.Username
            };
            bool isSaved = userService.RegisterUser(userDb, registerViewModel.Password) > 0;
            if (!isSaved)
            {
                return BadRequest();
            }
            return Ok(registerViewModel);
        }

        [HttpPost("GenerateToken")]
        public ActionResult GenerateToken([FromBody]LoginViewModel loginViewModel)
        {
            if (!userService.PreValidate())
            {
                return BadRequest(this.modelStateWrapper.ModelStateDictionary);
            }
            User userDb = userService.GetUserByNameAndPassword(loginViewModel.Username, loginViewModel.Password);
            if (userDb == null)
            {
                userService.AddValidationError("credentials", "Invalid username and/or password");
                return BadRequest(this.modelStateWrapper.ModelStateDictionary);
            }

            TokenViewModel tokenViewModel = new TokenViewModel();
            tokenViewModel.Token = securityService.GenerateToken(userDb);

            return Ok(tokenViewModel);
        }
    }
}