using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Project_DMD.Classes
{
    public static class Cloner
    {
        public static object Clone(this object ob)
        {
            Type t = ob.GetType();
            object result = Activator.CreateInstance(t);
            var t2 = result.GetType();
            var aFields = t.GetProperties();
            var bFields = t2.GetProperties();

            for (int i = 0; i < aFields.Length; i++)
            {
                bFields[i].SetValue(result, bFields[i].GetValue(ob));
            }
            return result;
        }
    }
}