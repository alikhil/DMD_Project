using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Classes.Extensions
{
    public static class StringExtensions
    {
        public static string PutIntoQuotes(this string value)
        {
            return "'" + value + "'";
        }

        public static string PutIntoDollar(this string value)
        {
            string alphabet = "qwertyuiopasdfghjklzxvbnmcQWERTYUIOPASDFGHJKLZXVBNMC";
            var random = new Random();
            int k = 3;
            var key = "$";
            for (var i = 0; i < k; i++)
                key += alphabet[random.Next(alphabet.Length)];
            key += '$';
            return key + value + key;
        }
    }
}