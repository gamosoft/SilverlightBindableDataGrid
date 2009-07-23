using System;
using System.Windows.Controls;
using BindableDataGrid.Data;
using System.Collections.Generic;

namespace DataGridDemo
{
    /// <summary>
    /// Main page of the application to show how this works
    /// </summary>
    public partial class MainPage : UserControl
    {
        #region "Methods"

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle the click event of the button to create dummy data
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void btnCreateData_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.LoadData();
        }

        /// <summary>
        /// Sample method to load random data into the grid
        /// </summary>
        public void LoadData()
        {
            // Create some sample names
            List<string> firstNames = new List<string>() { "Peter", "Frank", "Joe", "Lewis", "Jack", "Andrew", "Susan", "Marie", "Linda", "Anne", "Claire", "Debra" };
            List<string> lastNames = new List<string>() { "Smith", "Brown", "Green", "Parker", "Johnson", "Jackson", "Ford", "Sullivan" };

            // Create a sample DataTable
            DataTable dt = new DataTable("MyDataTable");

            // Create a column
            DataColumn dc1 = new DataColumn("col1");
            dc1.Caption = "First Name";
            dc1.ReadOnly = true;
            dc1.DataType = typeof(String);
            dc1.AllowResize = true;
            dc1.AllowSort = true;
            dc1.AllowReorder = true;
            dt.Columns.Add(dc1);

            // Create a column
            DataColumn dc2 = new DataColumn("col2");
            dc2.Caption = "Last Name";
            dc2.ReadOnly = true;
            dc2.DataType = typeof(String);
            dc2.AllowResize = true;
            dc2.AllowSort = true;
            dc2.AllowReorder = true;
            dt.Columns.Add(dc2);

            // Create a column
            DataColumn dc3 = new DataColumn("col3");
            dc3.Caption = "Age";
            dc3.ReadOnly = false;
            dc3.DataType = typeof(Int32);
            dt.Columns.Add(dc3);

            // Create a column
            DataColumn dc4 = new DataColumn("col4", "Married", true, true, true, true);
            dt.Columns.Add(dc4);

            // Create a column
            DataColumn dc5 = new DataColumn("col5", "Membership expiration", true, true, true, true);
            dt.Columns.Add(dc5);

            // Add sample rows to the table
            Random r = new Random();
            for (int i = 0; i < 15; i++)
            {
                DataRow dr = new DataRow();
                dr["col1"] = firstNames[r.Next(firstNames.Count)];
                dr["col2"] = lastNames[r.Next(lastNames.Count)];
                dr["col3"] = r.Next(20, 81);
                dr["col4"] = (r.Next(0,2) == 1);
                dr["col5"] = DateTime.Now.AddDays(r.Next(10));
                dt.Rows.Add(dr);
            }

            // Create a DataSet and add the table to it
            DataSet ds = new DataSet("MyDataSet");
            ds.Tables.Add(dt);

            // Do the binding
            myBindableDG.DataSource = ds;
            myBindableDG.DataMember = "MyDataTable";
            myBindableDG.DataBind();

            // We could do this as well
            // myBindableDG.DataSource = dt;
            // myBindableDG.DataBind();

        }

        #endregion
    }
}