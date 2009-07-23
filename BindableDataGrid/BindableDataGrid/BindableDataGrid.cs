using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Reflection;
using BindableDataGrid.Data;

namespace BindableDataGrid
{
    /// <summary>
    /// New DataGrid that enables the use of DataSource property and DataBind method
    /// </summary>
    public class BindableDataGrid : DataGrid
    {
        #region "Properties"
        
        /// <summary>
        /// DataSource of the DataGrid
        /// </summary>
        public IDataSource DataSource { get; set; }

        /// <summary>
        /// Name of the member in the DataSource
        /// </summary>
        public string DataMember { get; set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Default constructor of the class. Will take care of formatting columns upon creation
        /// </summary>
        public BindableDataGrid()
        {
            this.AutoGeneratingColumn += new EventHandler<DataGridAutoGeneratingColumnEventArgs>(BindableDataGrid_AutoGeneratingColumn);
        }

        /// <summary>
        /// Parses the DataSource property and creates the necessary columns
        /// </summary>
        public void DataBind()
        {
            if (this.DataSource == null)
            {
                throw new NullReferenceException("BindableDataGrid: DataSource is null");
            }
            List<object> values = null;

            switch (this.DataSource.GetType().Name)
            {
                case "DataTable":
                    values = this.GetValuesFromDataTable((DataTable)this.DataSource);
                    break;
                case "DataSet":
                    if (String.IsNullOrEmpty(this.DataMember))
                    {
                        throw new NullReferenceException("BindableDataGrid: DataMember not specified");
                    }
                    DataTable tempDT = ((DataSet)this.DataSource).Tables[this.DataMember];
                    if (tempDT == null)
                    {
                        throw new NullReferenceException(String.Format("BindableDataGrid: DataMember '{0}' doesn't exist", this.DataMember));
                    }
                    values = this.GetValuesFromDataTable(tempDT);
                    break;
                default:
                    throw new NotSupportedException(String.Format("BindableDataGrid: DataSource type '{0}' not supported", this.DataSource.GetType().Name));
            }
            // Assign the list of objects to the ItemsSource property of the grid
            this.ItemsSource = values;
        }

        /// <summary>
        /// Process the DataTable and return a list of dynamic objects
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>List of objects representing the rows in the DataTable</returns>
        private List<object> GetValuesFromDataTable(DataTable dt)
        {
            List<object> values = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                // Create an object of the dynamic class that represents a row
                Assembly tmp = dr.EmitAssembly();
                object c = tmp.CreateInstance("DataRowObject");
                Type myObject = tmp.GetType("DataRowObject");
                // Add the values to the properties
                foreach (string key in dr.Items.Keys)
                {
                    PropertyInfo pi = myObject.GetProperty(key.ToUpper());
                    pi.SetValue(c, dr.Items[key], null);
                }
                // Add the object to the generic list
                values.Add(c);
            }
            return values;
        }

        /// <summary>
        /// Changes the properties of the columns upon generation
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void BindableDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataTable dt = null;
            switch (this.DataSource.GetType().Name)
            {
                case "DataTable":
                    dt = ((DataTable)this.DataSource);
                    break;
                case "DataSet":
                    if (String.IsNullOrEmpty(this.DataMember))
                    {
                        throw new NullReferenceException("BindableDataGrid: DataMember not specified");
                    }
                    dt = ((DataSet)this.DataSource).Tables[this.DataMember];
                    if (dt == null)
                    {
                        throw new NullReferenceException(String.Format("BindableDataGrid: DataMember '{0}' doesn't exist", this.DataMember));
                    }
                    dt = ((DataSet)this.DataSource).Tables[this.DataMember];
                    break;
                default:
                    throw new NotSupportedException(String.Format("BindableDataGrid: DataSource type '{0}' not supported", this.DataSource.GetType().Name));
            }
            // Assign the properties found in the DataColumnCollection of the table
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.ToUpper() == e.Column.Header.ToString())
                {
                    e.Column.Header = dc.Caption;
                    e.Column.IsReadOnly = dc.ReadOnly;
                    e.Column.CanUserResize = dc.AllowResize;
                    e.Column.CanUserSort = dc.AllowSort;
                    e.Column.CanUserReorder = dc.AllowReorder;
                    break;
                }
            }            
        }

        #endregion
    }
}