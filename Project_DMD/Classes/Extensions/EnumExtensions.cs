using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_DMD.Classes.Extensions
{
    public static class EnumExtensions
    {
        public static Dictionary<int, string> ToDictionary<TEnum>()
    where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("Type must be an enumeration");
            var type = typeof(TEnum);
            return Enum.GetValues(type).Cast<int>().ToDictionary(e => e, e => Enum.GetName(type, e));
        }

        public static SelectList ToSelectList<TEnum>() where TEnum : struct
        {
            return new SelectList(ToDictionary<TEnum>(), "Key", "Value");
        }
    }
}