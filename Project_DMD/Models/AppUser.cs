using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_DMD.Attributes;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project_DMD.Models
{
    /// <summary>
    /// Application User class
    /// </summary>
    [AgsModel(TableName = "Client")]
    public class AppUser : IUser
    {
        private string _temp;

        [AgsPrimary(Name = "UserID", Insert = false)]
        public string Id { get; set; }

        [AgsAttribute]
        public string Email { get; set; }
        
        [AgsAttribute]
        public string PasswordHash { get; set; }

        [AgsAttribute]
        public string FirstName { get; set; }

        [AgsAttribute]
        public string LastName { get; set; }
        /// <summary>
        /// It doesn't necessary, but need for IUser<string>
        /// </summary>
        public string UserName {
            get { return Email; }
            set { _temp = value; }}

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }
}