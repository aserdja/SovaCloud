using Microsoft.AspNetCore.Mvc;
using SovaCloud.Data;
using SovaCloud.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using SovaCloud.Repositories;

namespace SovaCloud.Controllers
{
    public class AccountController : Controller
    {
        private readonly SovaCloudDbContext _context;

        public AccountController(SovaCloudDbContext context)
        {
            _context = context;
        }

        public IActionResult SignIn()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("HomeLogined", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] User user)
        {
            var userRepository = new UserRepository(_context);
            ValidateAuthorizationFormFields(user);
            if (userRepository.GetByEmailAndPassword(user.EmailAddress, user.Password).Result != null)
            {
                List<Claim> claims = new() { new Claim(ClaimTypes.Email, user.EmailAddress) };
                ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new() { AllowRefresh = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),
                    properties);

                return RedirectToAction("HomeLogined", "Home");
            }
            return View();
        }

        public IActionResult SignUp()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("HomeLogined", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<User>> SignUp([FromForm] User user)
        {
            ValidateRegistrationFormFields(user);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    List<Claim> claims = new() { new Claim(ClaimTypes.Email, user.EmailAddress) };
                    ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties properties = new() { AllowRefresh = true };
                    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),
                        properties);

                    return RedirectToAction("HomeLogined", "Home");
                }
                catch 
                {
                    return View(user);
                }
            }
            else
            {
                return View(user);
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn", "Account");
        }

        private void ValidateRegistrationFormFields(User user)
        {
            if (string.IsNullOrEmpty(user.EmailAddress))
            {
                ModelState.AddModelError(nameof(user.EmailAddress), "Enter a correct E-Mail address");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError(nameof(user.Password), "The password field must contain at least 8 and no more than 20 characters");
            }

            if (user.PasswordConfirmation != user.Password)
            {
                ModelState.AddModelError(nameof(user.Password), "The values ​​of the \"Password\" and \"Confirm Password\" fields are not identical");
            }
        }

        private void ValidateAuthorizationFormFields(User user)
        {
            if (string.IsNullOrEmpty(user.EmailAddress))
            {
				ModelState.AddModelError(nameof(user.EmailAddress), "Enter a correct E-Mail address");
			}
            if (!string.IsNullOrEmpty(user.Password)) 
            {
				ModelState.AddModelError(nameof(user.Password), "Enter a correct Password");
			}
        }
    }
}
