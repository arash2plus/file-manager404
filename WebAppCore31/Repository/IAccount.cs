using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppCore31.Models;

namespace WebAppCore31.Repository
{
    public interface IAccount
    {
        Task<bool> InsertAccount(AccountModel model);
        Task<ClaimsIdentity> LoginAccount(LoginViewModel model);

        //Task<AccountModel> GetAccount();
    }

}
