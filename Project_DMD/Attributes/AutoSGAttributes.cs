using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AgsModel : Attribute
    {
        /// <summary>
        /// Name of table for model, default value the same as class name
        /// </summary>
        public string TableName { get; set; }

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AgsAttribute : Attribute
    {
        /// <summary>
        /// Name of column in table, default value the same as property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This attibute means thar property in database must be int type
        /// </summary>
        public bool Int { get; set; }

    }

    public class AgsPrimary : AgsAttribute
    {
        public bool Insert { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AgsForeign : Attribute
    {
        /// <summary>
        /// Name of id property of model, default value 'Name'Id
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of table where we can find this entity, if it not the same as table of model
        /// </summary>
        public string TableName { get; set; }
    }
    
}