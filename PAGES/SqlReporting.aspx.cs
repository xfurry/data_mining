using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace WebApplication_OLAP
{
    public partial class SqlReporting : System.Web.UI.Page
    {
        private string sSelectedDB = "AdventureWorksDW";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Load listbox on first use
            if (!Page.IsPostBack)
            {
                InitDatabases();
                //InitTableNames();
            }

            sSelectedDB = DropDownListDatabases.SelectedItem.ToString();

            // register event handler
            DropDownListDatabases.SelectedIndexChanged += new EventHandler(DropDownListDatabases_SelectedIndexChanged);
            ListBoxTables.SelectedIndexChanged += new EventHandler(ListBoxTables_SelectedIndexChanged);
        }

        /*
         * Init all table names into a listbox
         */
        private void InitTableNames()
        {
            // show DB tables
            SQLManager manager = new SQLManager(sSelectedDB);
            DataSet objSet = manager.GetQueryDataSet("Select name, id from sysobjects where xtype='U'");
            ListBoxTables.DataSource = objSet;
            ListBoxTables.DataTextField = "name";
            ListBoxTables.DataValueField = "id";
            ListBoxTables.DataBind();
            manager.CloseConnection();
        }

        /*
         * Init databases from the server
         */
        private void InitDatabases()
        {
            SQLManager manager = new SQLManager();
            DataSet objSet = manager.GetQueryDataSet("SELECT name, dbid FROM master..sysdatabases");
            DropDownListDatabases.DataSource = objSet;
            DropDownListDatabases.DataTextField = "name";
            DropDownListDatabases.DataValueField = "dbid";
            DropDownListDatabases.DataBind();
            manager.CloseConnection();
        }

        /*
         * Exports the handmade query
         */
        protected void ButtonExecute_Click(object sender, EventArgs e)
        {
            ExecuteRelationalQuery(TextBoxQuery.Text);
        }

        /*
         * Exports the selected query data to excel; use random file name by timestamp
         */
        private void ExportDataTableToExcel(DataTable sInputTable)
        {
            // export to Excel
            // create random timestamp
            TimeSpan sTime = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0);
            string timeStamp = ((long)sTime.TotalMilliseconds).ToString();

            ExcelManager em = new ExcelManager();
            if (em.ExcelExport(sInputTable, "RelationalReport_" + timeStamp + ".xls"))
                Label1.Text = "Success!";
        }

        /*
         * Execute handmade query
         */
        protected void Button1_Click(object sender, EventArgs e)
        {
            // get from session
            if (Session != null)
            {
                DataTable objTable = (DataTable)Session["queryData"];
                // call export method
                ExportDataTableToExcel(objTable);
            }
        }

        /*
         * Select all the column names for the selected table
         */
        protected void ListBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Sets the array count variable makes sure index is not -1.
            if (ListBoxTables.SelectedIndex >= 0)
            {
                // clear current query
                GridViewData.DataSource = null;
                GridViewData.DataBind();

                // list selected table: to be removed
                //Label1.Text = ListBoxTables.SelectedItem.ToString();

                string sQueryText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" +
                    ListBoxTables.SelectedItem.ToString() + "'ORDER BY ORDINAL_POSITION";

                // execute query
                SQLManager manager = new SQLManager(sSelectedDB);
                DataTable objTable = new DataTable();
                objTable.Load(manager.GetQueryResult(sQueryText));
                GridViewMain.DataSource = objTable;
                GridViewMain.DataBind();
                manager.CloseConnection();
            }
        }

        /*
         * Take all the selected fields and execute the query
         */
        protected void ButtonRunFields_Click(object sender, EventArgs e)
        {
            StringBuilder objBuilder = new StringBuilder();
            int iCounter = 0;

            // Iterate through the Products.Rows property
            foreach (GridViewRow row in GridViewMain.Rows)
            {
                // Access the CheckBox
                CheckBox cb = (CheckBox)row.FindControl("CheckBoxColumn");

                if (cb != null && cb.Checked)
                {
                    ++iCounter;
                    // if there are more than 1 rows selected then add a comma for each row value
                    if (iCounter > 1)
                        objBuilder.Append(",");

                    // First, get the ProductID for the selected row; The names column index is always 1
                    string sColumnName = GridViewMain.Rows[row.RowIndex].Cells[1].Text;
                    objBuilder.Append(sColumnName);
                }
            }

            // Execute query
            string sQuery = "Select top 1000" + objBuilder.ToString() + " from " + ListBoxTables.SelectedItem.ToString() + ";";
            ExecuteRelationalQuery(sQuery);

            // list column names; for remove
            //Label1.Text = objBuilder.ToString();
        }

        /*
         * Check all grid view fields
         */
        protected void ButtonCheck_Click(object sender, EventArgs e)
        {
            ToggleCheckState(true);
        }

        /*
         * Unckeck all grid view fields
         */
        protected void ButtonUncheck_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
        }

        /*
         * Modify checked state for the column names
         */
        private void ToggleCheckState(bool checkState)
        {
            // Iterate through the Products.Rows property
            foreach (GridViewRow row in GridViewMain.Rows)
            {
                // Access the CheckBox 
                CheckBox cb = (CheckBox)row.FindControl("CheckBoxColumn");
                if (cb != null)
                    cb.Checked = checkState;
            }
        }

        /*
         * Run selected query and populate the data grid
         */
        private void ExecuteRelationalQuery(string sQuery)
        {
            SQLManager manager = new SQLManager(sSelectedDB);
            DataTable objTable = new DataTable();
            objTable.Load(manager.GetQueryResult(sQuery));
            GridViewData.DataSource = objTable;
            GridViewData.DataBind();
            manager.CloseConnection();

            // store the data table and prepare the mining link
            Session.Add("queryData", objTable);
            HyperLinkMining.Visible = true;
        }

        /*
         * Load the current database items
         */
        protected void DropDownListDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Sets the array count variable makes sure index is not -1.
            if (DropDownListDatabases.SelectedIndex >= 0)
            {
                this.sSelectedDB = DropDownListDatabases.SelectedItem.ToString();
                InitTableNames();
            }
        }
    }
}
