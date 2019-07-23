using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using ConstructionNew.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Twitter.Messages;

namespace ConstructionNew.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result =
                await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            string userName = model.UserName;

            //selects user id to check suspended status
            var user = UserManager.Users.Where(u => u.UserName == userName).SingleOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            if (user.Suspended)
            {
                // If the user is marked as suspended, they will be logged out and directed to the Lockout view so they can't access the dashboard
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return View("Lockout");
            }

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Dashboard");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, model.RememberMe});
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync()) return View("Error");
            return View(new VerifyCodeViewModel {Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe,
                model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }


        // Check the username and confirmation code
        [AllowAnonymous]
        public ActionResult CheckCode()
        {
            return View();
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CheckCode(CreateUserRequest model)
        {
            if (ModelState.IsValid)
            {
                var createUserRequest =
                    (from requests in db.CreateUserRequests
                        where requests.UserName == model.UserName
                        where requests.ConfirmationCode == model.ConfirmationCode
                        select requests).First();
                if (createUserRequest != null)
                {
                    return RedirectToAction("Register", createUserRequest) ;
                }
            }

            return View(model);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(CreateUserRequest createUserRequest)
        {
            if (ModelState.IsValid)
            {
                // Initialize the model with the username and roles. 
                return View(new RegisterViewModel
                    {UserName = createUserRequest.UserName, UserRoles = createUserRequest.UserRoles});
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FName = model.FName,
                    LName = model.LName,
                    // Get user role
                    UserRole = model.UserRoles
                };
                var identityResult = await UserManager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                    // assign user to role
                    await UserManager.AddToRoleAsync(user.Id, user.UserRole);

                    await SignInManager.SignInAsync(user, false, false); //#1 comment this line of code to test email confirmation link below #2

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //#2 uncomment lines of code below to test email confirmation link
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    // Delete CreateUserRequest
                    using (db)
                    {
                        var createUserRequest = db.CreateUserRequests.First(x => x.UserName == user.UserName);
                        if (createUserRequest != null) db.CreateUserRequests.Remove(createUserRequest);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", "Dashboard");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) return View("Error");
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !await UserManager.IsEmailConfirmedAsync(user.Id))
                    return View("ForgotPasswordConfirmation");

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                //to be able to test password reset link at code below, email confirmation link in register method should be enabled first
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null) return RedirectToAction("ResetPasswordConfirmation", "Account");
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded) return RedirectToAction("ResetPasswordConfirmation", "Account");
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null) return View("Error");
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem {Text = purpose, Value = purpose})
                .ToList();
            return View(new SendCodeViewModel
                {Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid) return View();

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider)) return View("Error");
            return RedirectToAction("VerifyCode",
                new {Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe});
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) return RedirectToAction("Login");

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = false});
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel {Email = loginInfo.Email});
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Manage");

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null) return View("ExternalLoginFailure");
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false, false);
                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        public ActionResult _UserListPartial()
        {
            //Queries to determine what pop-up shows on suspension submission
            var currentFutureUsers = (from u in db.Users
                                      join s in db.Schedules on u.Id equals s.Person.Id
                                      where s.EndDate > DateTime.Now
                                      select u).Distinct().ToList();
            ViewBag.currentFutureUsers = currentFutureUsers;

            var suspendedUsers = (from a in db.Users
                                      where a.Suspended
                                      select a).ToList();
            ViewBag.SuspendedUsers = suspendedUsers;

            return PartialView(db.Users.AsEnumerable());
        }

        public ActionResult _SuspendedUserPartial()
        {
            return PartialView(db.Users.AsEnumerable());
        }

        public ActionResult EditRole(string id, string UserRole)
        {
            var user = (from ApplicationUser in db.Users where ApplicationUser.Id == id select ApplicationUser).FirstOrDefault();
            var current = user.UserRole;
            user.UserRole = UserRole;

            UserManager.RemoveFromRole(user.Id, current);
            UserManager.AddToRole(user.Id, UserRole);
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Dashboard");
            }
            return RedirectToAction("Index", "Dashboard");
        }


        public ActionResult EditWork(string id, string workType)
        {
            var user = (from ApplicationUser in db.Users where ApplicationUser.Id == id select ApplicationUser).FirstOrDefault();
            if (workType == "Unselected")
            {
                user.WorkType = Enums.WorkType.Unselected;
            }
            if (workType == "LeadMan" || workType == "Lead Man")
            {
                user.WorkType = Enums.WorkType.LeadMan;
            }
            if (workType == "Foreman")
            {
                user.WorkType = Enums.WorkType.Foreman;
            }
            if (workType == "ExpMBA" || workType == "Experienced MBA")
            {
                user.WorkType = Enums.WorkType.ExpMBA;
            }
            if (workType == "NewMBA" || workType == "New MBA")
            {
                user.WorkType = Enums.WorkType.NewMBA;
            }

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Dashboard");
            }
            return RedirectToAction("Index", "Dashboard");
        }

        
        [HttpPost]
        public ActionResult Suspend(string id, bool suspended = false)
        {
            //fetch user to suspend or unsuspend
            var user = (from ApplicationUser in db.Users where ApplicationUser.Id == id select ApplicationUser).FirstOrDefault();
            //fetch list of users schedules
            var schedules = (from s in db.Schedules
                             where s.Person.Id == user.Id
                             select s).ToList();
            if (suspended)
            {
                user.Suspended = true;
                
                //if user has schedule items
                if (schedules.Count > 0)
                {
                    foreach (var schedule in schedules)
                    {
                        //if schedule item has not ended yet, end it immediately
                        if (schedule.StartDate < DateTime.Now && schedule.EndDate > DateTime.Now)
                        {
                            schedule.EndDate = DateTime.Now;
                        }
                        //deletes all future schedules
                        if (schedule.StartDate > DateTime.Now)
                        {
                            db.Schedules.Remove(schedule);
                        }
                    }
                }
            }
            if (!suspended)
            {
                user.Suspended = false;                              
            }
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Dashboard");
            }
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Delete(string id)
        {
            try
            {
                var del = (from ApplicationUser in db.Users where ApplicationUser.Id == id select ApplicationUser).FirstOrDefault();
                db.Users.Remove(del);
                db.SaveChanges();
            }
            catch (Exception)
            {
                TempData["shortMessage"] = "This user is on the schedule and therefore cannot be deleted.";
            }
            return RedirectToAction("Index", "Dashboard");
        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("", error);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null) properties.Dictionary[XsrfKey] = UserId;
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion

        private bool checkIfRegisterCodeExists(CreateUserRequest createUserRequest)
        {
            bool checkCode = false;
            using (var db = new ApplicationDbContext())
            {
                checkCode = db.CreateUserRequests.Any(x =>
                    x.UserName == createUserRequest.UserName &&
                    x.ConfirmationCode == createUserRequest.ConfirmationCode);
            }

            return checkCode;
        }
    }
}