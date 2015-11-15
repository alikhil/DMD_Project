using Microsoft.AspNet.Identity;
using Project_DMD.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

/**
 * Custom Account authorization code have taken from Codeproject.com
 * http://www.codeproject.com/Articles/796940/Understanding-Using-and-Customizing-ASP-NET-Identi
 * */
namespace Project_DMD.Models
{
    public class CustomUserStore : IUserStore<AppUser>, IUserPasswordStore<AppUser>
    {
        public Task<AppUser> FindByNameAsync(string userName)
        {
            AppUser user = AppUserContext.Instance.Find(userName);
            return Task.FromResult<AppUser>(user);
        }

        public Task CreateAsync(AppUser user)
        {
            return Task.FromResult<bool>(AppUserContext.Instance.Add(user));
        }

        public Task<string> GetPasswordHashAsync(AppUser user)
        {
            return Task.FromResult<string>(user.PasswordHash.ToString());
        }
        public Task SetPasswordHashAsync(AppUser user, string passwordHash)
        {
            return Task.FromResult<string>(user.PasswordHash = passwordHash);
        }

        #region Not implemented methods
        public Task DeleteAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> FindByIdAsync(string userId)
        {
            return Task.FromResult<AppUser>(AppUserContext.Instance.FindById(userId));
        }

        public Task UpdateAsync(AppUser user)
        {
            return Task.FromResult(QueryExecutor.Instance.UpdateAppUser(user));
        }

        public Task<bool> HasPasswordAsync(AppUser user)
        {
            return Task.FromResult<bool>(!String.IsNullOrEmpty(user.PasswordHash));
        }

        public void Dispose()
        {

        }
        #endregion
    }
}