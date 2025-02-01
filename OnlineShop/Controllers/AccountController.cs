using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Services;
using OnlineShop.Data;
using OnlineShop.ViewModels;


namespace OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly ValidationService _validationService;
        private readonly UserService _userService;
        private readonly EmailService _emailService;

        public AccountController(ValidationService validationService, UserService userService, EmailService emailService)
        {
            _validationService = validationService;
            _userService = userService;
            _emailService = emailService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (!_validationService.IsValidEmail(user.Email))
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(user);
            }

            if (_userService.IsEmailDuplicate(user.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(user);
            }

            _userService.CreateUser(user);

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var foundUser = _userService.AuthenticateUser(user.Email, user.Password);
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email or password is not valid!");
                return View(user);
            }

            var claimsPrincipal = _userService.GenerateClaimsPrincipal(foundUser);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return Redirect("/");
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult RecoveryPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecoveryPassword(ResetPasswordViewModel recoveryPassword)
        {
   
            if (!_validationService.IsValidEmail(recoveryPassword.Email))
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(recoveryPassword);
            }

            var foundUser = _userService.GetUserByEmail(recoveryPassword.Email);
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email does not exist");
                return View(recoveryPassword);
            }

            var recoveryCode = _userService.GenerateRecoveryCode(foundUser);
            _emailService.SendRecoveryCode(foundUser.Email, recoveryCode);

            return Redirect($"/Account/ResetPassword?email={foundUser.Email}");
        }

        public IActionResult ResetPassword(string email)
        {
            return View(new ResetPasswordViewModel { Email = email });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            var isValid = _userService.VerifyRecoveryCode(resetPassword.Email, resetPassword.RecoveryCode.Value);
            if (!isValid)
            {
                ModelState.AddModelError("RecoveryCode", "Email or recovery code is not valid");
                return View(resetPassword);
            }

            _userService.UpdatePassword(resetPassword.Email, resetPassword.NewPassword);

            return RedirectToAction("Login");
        }
    }
}
