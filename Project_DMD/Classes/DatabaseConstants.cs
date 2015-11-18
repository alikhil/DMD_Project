using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Classes
{
    public class DatabaseConstants
    {
        public readonly static string ConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=PMS;COMMANDTIMEOUT=200;";

        public readonly static string AppUsersTableName = "Client";

        /// <summary>
        /// Template string in order to drop table.
        /// <para>You should use String.Formater, where </para>
        /// <para>{0} - Table Name.</para>
        /// </summary>
        public readonly static string DropTableTemplate = "DROP TABLE {0};";

        /// <summary>
        /// Template string for insertion into POSTGRESQL. 
        /// <para>You should use String.Formater, where </para>
        /// <para>{0} - Table Name </para>
        /// <para>{1} - Ordered Name Columns (For Example: Column1, Column2) </para>
        /// <para>{2} - Ordered Values For Columns (For Example: (Column1Value, Column2Value) ) </para>
        /// <para>{3} - Column Name of returning value. </para>
        /// </summary>
        public static readonly string InsertTableTemplate = "INSERT INTO {0} ({1}) VALUES {2} RETURNING {3};";

        /// <summary>
        /// Template string for insertion into POSTGRESQL. 
        /// <para>You should use String.Formater, where </para>
        /// <para>{0} - Table Name </para>
        /// <para>{1} - Ordered Values For Columns (For Example: (Column1Value, Column2Value) ) </para>
        /// <para>{2} - Column Name of returning value. </para>
        /// </summary>
        public static readonly string InsertTableTemplateWithoutColumns = "INSERT INTO {0} VALUES {1} RETURNING {2};";

        /// <summary>
        /// Template string for selection.
        /// <para>You should use String.Formater, where</para>
        /// <para>{0} - From what table we select (TableName)</para>
        /// <para>{1} - Selection Filters (ColumnName = SomeValue or ColumnName1 = SomeValue1...).</para>
        /// </summary>
        public static readonly string SelectFromTableWhereTemplate = "SELECT * FROM {0} WHERE {1};";

        /// <summary>
        /// Template string for selection.
        /// <para>You should use String.Formater, where</para>
        /// <para>{0} - From what table we select (TableName).</para>
        /// </summary>
        public static readonly string SelectAllFromTableTemplate = "SELECT * FROM {0};";

        /// <summary>
        /// Template string for update.
        /// <para>You should use String.Formater, where</para>
        /// <para>{0} - What table we refresh (TableName)</para>
        /// <para>{1} - Refresh expression (i.e Views = 0)</para>
        /// <para>{2} - Update filter, on what records refresh expression will be applied</para>.
        /// </summary>
        public static readonly string UpdateOnTemplate = "Update {0} SET {1} WHERE {2};";
    }
}