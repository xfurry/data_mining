using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication_OLAP
{
    public partial class SqlReporting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // show DB details on load; ToDo
        }

        protected void ButtonExecute_Click(object sender, EventArgs e)
        {
            SQLManager manager = new SQLManager();
            DataTable objTable = new DataTable();
            objTable.Load(manager.GetQueryResult(TextBoxQuery.Text));
            GridView1.DataSource = objTable;
            GridView1.DataBind();
            manager.CloseConnection();

            // store the data table and prepare the mining link
            Session.Add("queryData", objTable);
            HyperLinkMining.Visible = true;
        }

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
    }
}
