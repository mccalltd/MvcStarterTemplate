﻿using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using StarterTemplate.Core;
using StarterTemplate.Core.Data;
using StarterTemplate.Core.Domain;
using StarterTemplate.Core.Extensions;
using StarterTemplate.Core.Services;
using StarterTemplate.Models;

namespace StarterTemplate.Controllers
{
    [RoutePrefix("auth")]
    public class AuthController : ApplicationControllerBase
    {
        private readonly IRepository _repository;
        private readonly ISecurityService _securityService;
        private readonly MailController _mailer;
        private readonly CurrentUserContext _currentUserContext;

        public AuthController(
            IRepository repository,
            ISecurityService securityService,
            MailController mailer,
            CurrentUserContext currentUserContext
            )
        {
            _repository = repository;
            _securityService = securityService;
            _mailer = mailer;
            _currentUserContext = currentUserContext;
        }

        [GET("")]
        public ActionResult Index()
        {
            if (_currentUserContext.IsAuthenticated)
                return Redirect("/");

            return View();
        }

        [GET("log-in")]
        public ActionResult LogIn(string returnUrl)
        {
            var model = new AuthIndexModel
            {
                ActivePanel = "log-in",
                ReturnUrl = returnUrl
            };

            return JsonHtml("_Auth", model);
        }

        [POST("log-in")]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn([Bind(Prefix = "LogInModel")] AuthLogInModel model, string returnUrl)
        {
            Member member = null;

            if (ModelState.IsValid
                && Try(() => member = _securityService.ValidateLogin(model.UsernameOrEmailAddress, model.Password)))
            {
                Authenticate(member.Username);

                // Prompt the user to change their password if logging in with reset password.
                if (member.MustChangePasswordOnNextLogin)
                    return JsonSuccess(new { mustChangePassword = true });

                if (returnUrl.HasValue())
                    return JsonRedirect(returnUrl);

                return JsonSuccess();
            }

            return JsonFailure();
        }

        [GET("log-out")]
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [GET("sign-up")]
        public ActionResult SignUp()
        {
            var model = new AuthIndexModel
            {
                ActivePanel = "sign-up"
            };

            return JsonHtml("_Auth", model);
        }

        [POST("sign-up")]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Prefix = "SignUpModel")] AuthSignUpModel model)
        {
            Member member = null;

            if (ModelState.IsValid
                && Try(() =>
                {
                    member = _securityService.SignUp(model.Username, model.EmailAddress, model.Password);
                    _mailer.SendWelcomeMessage(member).DeliverAsync();
                }))
            {
                Authenticate(member.Username);
                return JsonSuccess();
            }

            return JsonFailure();
        }

        private void Authenticate(string username)
        {
            FormsAuthentication.SetAuthCookie(username, true);
            _currentUserContext.Authenticate(username);
        }

        [POST("forgot-password")]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword([Bind(Prefix = "ForgotPasswordModel")] AuthForgotPasswordModel model)
        {
            if (ModelState.IsValid
                && Try(() =>
                {
                    var emailAddress = model.EmailAddress;
                    var newPassword = _securityService.ResetPassword(emailAddress);
                    _mailer.SendResetPasswordMessage(emailAddress, newPassword).DeliverAsync();
                }))
            {
                return JsonSuccess();                
            }

            return JsonFailure();
        }

        [POST("change-password")]
        [Authorize]
        public ActionResult ChangePassword([Bind(Prefix = "ChangePasswordModel")] AuthChangePasswordModel model)
        {
            if (ModelState.IsValid
                && Try(() => _securityService.ChangePassword(model.NewPassword)))
            {
                return JsonSuccess();                
            }

            return JsonFailure();
        }
    }
}
