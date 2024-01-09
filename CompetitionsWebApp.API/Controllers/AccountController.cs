using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CompetitionsWebApp.API.Repositories;
using CompetitionsWebApp.Common.ViewModels;
using CompetitionsWebApp.Common.Exceptions;
using CompetitionsWebApp.Common.Models;

namespace CompetitionsWebApp.API.Controllers
{
    [Controller]
    public class AccountController : Controller
    {
        private readonly UserRepository userRepository;
        public AccountController(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("~/Account/Login/{ReturnUrl?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await userRepository.GetUserByLoginModelAsync(loginViewModel);
                    if (user != null)
                    {
                        await Authenticate(user);
                        HttpContext.Response.Cookies.Append("id", user.Id.ToString());
                        return Redirect("~/Home/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                        User _user = await userRepository.GetUserByLoginModelAsync(loginViewModel);
                    }
                }
                catch (NotFoundException)
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(loginViewModel);
        }

        [Route("~/Account/Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [Route("~/Account/Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new();
                if (await userRepository.UserIsInDatabase(registerViewModel))
                {
                    ModelState.AddModelError("", "Такой пользователь уже существует!");
                }
                else
                {
                    user.Login = registerViewModel.Login;
                    user.Password = registerViewModel.Password;

                    await userRepository.AddNewUser(user);

                    //await Authenticate(account);
                    //HttpContext.Response.Cookies.Append("id", account.Id.ToString());
                    return Redirect("~/Account/Login");
                }
            }
            return View(registerViewModel);
        }

        private async Task Authenticate(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
