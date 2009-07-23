using System.Collections.Generic;
using System;

namespace BindableDataGrid.Data
{
    /// <summary>
    /// Collection of columns in a DataTable
    /// </summary>
    public class DataColumnCollection : List<DataColumn>
    {
        #region "Methods"

        /// <summary>
        /// Adds a nerw column to the collection checking for duplicates in the name
        /// </summary>
        /// <param name="dc">New column to add</param>
        public new void Add(DataColumn dc)
        {
            foreach (DataColumn curColumn in this)
            {
                if (dc.ColumnName == curColumn.ColumnName)
                {
                    throw new Exception(String.Format("DataColumnCollection: Column with name '{0}' already exists", dc.ColumnName));
                }
            }
            base.Add(dc);
        }

        #endregion
    }
}