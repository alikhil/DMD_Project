using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Models
{
    /// <summary>
    /// Application User class
    /// </summary>
    public class AppUser : IUser<string>
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        /// <summary>
        /// It doesn't necessary, but need for IUser<string>
        /// </summary>
        public string UserName { get; set; }
    }
}