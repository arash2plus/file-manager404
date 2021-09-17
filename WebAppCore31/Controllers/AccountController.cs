using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAppCore31.Models;
using WebAppCore31.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNet.Identity;

namespace WebAppCore31.Controllers
{
    public class AccountController : Controller
    {

        //private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;
        private readonly IAccount AcctDb;
        private IAccount Accrepository;
        private IHttpContextAccessor _httpContextAccessor;
        public AccountController(IAccount account, IAccount repo, IHttpContextAccessor httpContextAccessor)
        {
            this.AcctDb = account;
            Accrepository = repo;
            //var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _httpContextAccessor = httpContextAccessor;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LogOut()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var UserOk = await AcctDb.LoginAccount(model);

                //if (model.AccountId == "1" && model.Password == "123")
                if (UserOk.IsAuthenticated)
                {
                    List<Claim> claims = new List<Claim>
                        {
                            new Claim("AccountId",model.AccountId)

                        };



                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    Microsoft.AspNetCore.Authentication.AuthenticationProperties authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(principal, authProperties);
                    return RedirectToAction("Index", "FileManagerDb");


                    //await HttpContext.SignInAsync(
                    //                               CookieAuthenticationDefaults.AuthenticationScheme,
                    //                               new ClaimsPrincipal(identity),
                    //                               new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                    //                               {
                    //                                   IsPersistent = true,
                    //                                   ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                    //                               });

                    //await _httpContextAccessor.HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));
                    ////AuthenticationManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties()
                    ////{
                    ////    AllowRefresh = true,
                    ////    IsPersistent = false,
                    ////    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                    ////}, identity);

                    ////return RedirectToAction("Index", "Home");
                    //return RedirectToAction("Index", "FileManagerDb");


                }
            }
            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> InsertAccount(AccountModel acct)
        {
            bool Result = await AcctDb.InsertAccount(new AccountModel
            {
                Id = Convert.ToInt32(new Random()),
                AccountId = acct.AccountId,
                AccountName = acct.AccountName,
                Email = acct.Email,
                Password = acct.Password
            });

            return Ok(Result);
        }




    }
}
