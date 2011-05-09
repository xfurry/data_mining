using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebApplication_OLAP.classes.data_managers;

namespace WebApplication_OLAP.pages
{
    public partial class SqlDataMining : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // get from session
            if (Session != null)
            {
                DataTable objTable = (DataTable)Session["queryData"];
                GridView1.DataSource = objTable;
                GridView1.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SQLMiningManager objMiningManager = new SQLMiningManager();
            // return mining result
            if (objMiningManager.CreateMiningStructureIfCan())
                LabelStatus.Text = LabelStatus.Text + "Success!";
            else
                LabelStatus.Text = LabelStatus.Text + "Failed!";
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            SQLMiningManager objMiningManager = new SQLMiningManager();

            // display results
            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objMiningData = objMiningManager.GetMiningResults("Customer");

            DataTable objTable = new DataTable();
            DataColumn myColumn = new DataColumn();
            DataRow myRow = null;

            DataTable objSchemaTable = objMiningData.GetSchemaTable();
            List<string> lMeta = new List<string>();

            // init meta values
            for (int i = 0; i < objSchemaTable.Rows.Count; i++)
                lMeta.Add(objSchemaTable.Rows[i][0].ToString());

            // add columns and column captions
            for (int i = 0; i < objMiningData.FieldCount; i++)
            {
                myColumn = new DataColumn(lMeta[i]);
                objTable.Columns.Add(myColumn);
            }

            // output the rows in the DataReader
            while (objMiningData.Read())
            {
                // new row
                myRow = objTable.NewRow();
                // set the row values
                for (int i = 0; i < objMiningData.FieldCount; i++)
                    myRow[i] = objMiningData[i];

                // add row to the table
                objTable.Rows.Add(myRow);
            }
            // close reader
            objMiningData.Close();

            GridView1.DataSource = objTable;
            GridView1.DataBind();

            // load the main table data
            Session.Add("queryData", objTable);
        }
    }
}
