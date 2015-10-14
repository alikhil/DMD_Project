using Microsoft.AspNet.Identity;
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
        public System.Threading.Tasks.Task<AppUser> FindByNameAsync(string userName)
        {
            AppUser user = AppUserContext.Instance.DummyUsersList.Find(item => item.Email == userName);
            return Task.FromResult<AppUser>(user);
        }

        public System.Threading.Tasks.Task CreateAsync(AppUser user)
        {
            user.Id = Guid.NewGuid().ToString();
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
        public System.Threading.Tasks.Task DeleteAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<AppUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}