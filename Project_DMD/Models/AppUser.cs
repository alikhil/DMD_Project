using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_DMD.Attributes;

namespace Project_DMD.Models
{
    /// <summary>
    /// Application User class
    /// </summary>
    [AgsModel(TableName = "Client")]
    public class AppUser : IUser<string>
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
    }
}