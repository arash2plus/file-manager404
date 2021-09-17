using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore31.Models;
using Dapper;
using System.Data;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Authentication;
////////sing Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNet.Identity;
//using PlatformAuthenticationOptions = Microsoft.Owin.Security.AuthenticationOptions;//??
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
//using System.Security.Claims.ClaimsIdentity = System.se
using System.Linq;


namespace WebAppCore31.Repository
{
    public class Account : IAccount
    {

        //private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;
        //private IAuthenticationManager AuthenticationManager => AuthenticationHttpContextExtensions.AuthenticateAsync(this._httpContext);
        public ClaimsPrincipal _ClaimPrincipal = new ClaimsPrincipal();
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private static IHttpContextAccessor _httpContextAccessor;
        public HttpContext _httpContext;
        public async Task<bool> InsertAccount(AccountModel model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\f.safinejad\\Sedna_FileStream.mdf;Integrated Security=True;Connect Timeout=30"))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@P_Id", model.Id);
                    parameters.Add("@P_AcctId", model.AccountId);
                    parameters.Add("@P_AcctName", model.AccountName);
                    parameters.Add("@P_AcctPassword", model.Password);
                    parameters.Add("@P_AcctEmail", model.Email);

                    int a = await connection.ExecuteAsync("Stp_InsertAcct", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        //public async bool LoginAccount(LoginViewModel model)
        //{

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\f.safinejad\\Sedna_FileStream.mdf;Integrated Security=True;Connect Timeout=30"))
        //        {
        //            DynamicParameters parameters = new DynamicParameters();

        //            parameters.Add("@P_AcctId", model.AccountId);
        //            parameters.Add("@P_AcctPassword", model.Password);

        //            int a = await connection.ExecuteAsync("Stp_GetAcct", parameters, commandType: System.Data.CommandType.StoredProcedure);
        //            return true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

        public async Task<ClaimsIdentity> LoginAccount(LoginViewModel model)
        {
            ClaimsIdentity objidentity = new ClaimsIdentity();
            HttpContextAccessor _httpContextAccessor2 = new HttpContextAccessor();

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=safinezhad-pc\\;Initial Catalog=Sedna_FileStream;Integrated Security=True"))
                //using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\f.safinejad\\Sedna_FileStream.mdf;Integrated Security=True;Connect Timeout=30"))
                {

                    SqlCommand command = new SqlCommand();
                    connection.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@P_AcctId", model.AccountId);
                    parameters.Add("@P_Password", model.Password);

                    command.Connection = connection;
                    command.CommandText = "Stp_GetAcct";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@P_AcctId", SqlDbType.NVarChar, 18).Value = model.AccountId;
                    command.Parameters.Add("@P_Password", SqlDbType.NVarChar, 18).Value = model.Password;

                    SqlDataReader readIn = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);



                    List<Claim> claims = new List<Claim>
                        {
                            new Claim("AccountId", model.AccountId),
                            new Claim("Password" , model.Password)
                        };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    AuthenticationProperties authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                        IsPersistent = true
                    };

                    // await HttpContext.SignInAsync(principal, authProperties);
                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


                    while (readIn.Read())
                    {
                        var claims2 = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, readIn["AccountId"].ToString()),
                            new Claim(ClaimTypes.NameIdentifier, readIn["Id"].ToString()),
                            new Claim(ClaimTypes.Email, readIn["Email"].ToString()),
                        };
                        objidentity = new ClaimsIdentity(claims2, DefaultAuthenticationTypes.ApplicationCookie);
                    }
                    //return true;
                    return objidentity;
                }
            }
            catch (Exception e)
            {
                return null;// false;
            }
        }

    }
}
