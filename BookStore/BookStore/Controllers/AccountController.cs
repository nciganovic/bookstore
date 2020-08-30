using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Tables;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;

namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private IPersonRepository personRepository;
        private ILogger<AccountController> logger;

        public AccountController(UserManager<ApplicationUser> userManager, 
                                SignInManager<ApplicationUser> signInManager, 
                                IPersonRepository personRepository, 
                                ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.personRepository = personRepository;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                Person person = personRepository.Add(model.person);

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PersonId = person.Id
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, confirmationLink);

                    ViewBag.Title = "Registration successfull";
                    ViewBag.Message = "Before you login, you need to confirm your email. To confirm your email, you need to click on the link we sent you.";
                    return View("Views/Home/Message.cshtml");
                }
                else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl) 
        {
            LoginViewModel viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
        {
            model.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.EmailConfirmed && (await userManager.CheckPasswordAsync(user, model.Password))) 
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet.");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(ReturnUrl) && !String.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else {
                        return RedirectToAction("index", "home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }
            }

            return View(model);
        }

        [HttpPost][HttpGet]
        public async Task<IActionResult> IsEmailTaken(string email) {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else {
                return Json($"Email {email} is already in use.");
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl) 
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null) {

            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View(loginViewModel);
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null) {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");

                return View(loginViewModel);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = null;

            if (email != null) 
            {
                user = await userManager.FindByEmailAsync(email);

                if (user != null && !user.EmailConfirmed) {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet.");
                    return View("Views/Account/Login.cshtml", loginViewModel);
                }
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else {

                if (email != null) {

                    Person person = new Person
                    {
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)
                    };

                    Person addPerson = personRepository.Add(person);

                    if (user == null) {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            PersonId = addPerson.Id
                        };

                        await userManager.CreateAsync(user);

                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                        logger.Log(LogLevel.Warning, confirmationLink);

                        ViewBag.Title = "Registration successfull";
                        ViewBag.Message = "Before you login, you need to confirm your email. To confirm your email, you need to click on the link we sent you.";
                        return View("Views/Home/Message.cshtml");
                    }

                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
            }

            ViewBag.ErrorTitle = $"Email claim not recived from: {info.LoginProvider}";
            ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";

            return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token) 
        {
            if (userId == null || token == null) {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null) 
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid.";
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                ViewBag.Title = "Email confirmed";
                ViewBag.Message = "Your email is confirmed successfully.";
                return View("Views/Home/Message.cshtml");
            }
            else
            {
                ViewBag.Title = "Email not confirmed";
                ViewBag.Message = "Your email confirmation failed.";
                return View("Views/Home/Message.cshtml");
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword() {
            ForgotPasswordViewModel viewModel = new ForgotPasswordViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(viewModel.Email);

                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = viewModel.Email, token = token }, Request.Scheme);
                    logger.Log(LogLevel.Warning, passwordResetLink);
                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (token == null || email == null) {
                ModelState.AddModelError(String.Empty, "Invalid password reset token.");
            }

            ResetPasswordViewModel viewModel = new ResetPasswordViewModel 
            {
               Token = token,
               Email = email
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid) 
            {
                var user = await userManager.FindByEmailAsync(viewModel.Email);
                if (user != null) 
                {
                    var result = await userManager.ResetPasswordAsync(user, viewModel.Token, viewModel.Password);

                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }

                    return View(viewModel);
                }

                return View("ResetPasswordConfirmation");
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword() 
        {
            ChangePasswordViewModel viewModel = new ChangePasswordViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid) 
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null) 
                {
                    return RedirectToAction("Login");
                }

                var result = await userManager.ChangePasswordAsync(user, viewModel.OldPassword, viewModel.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else 
                {
                    await signInManager.RefreshSignInAsync(user);
                    ViewBag.Title = "Password changed successfully";
                    ViewBag.Message = "Your password has been changed successfully.";
                    return View("Views/Home/Message.cshtml");
                }
            }


            return View(viewModel);
        }
    }
}
