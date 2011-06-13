using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using Microsoft.AnalysisServices.AdomdClient;
using System.Data;
using System.IO;
using System.Threading;

namespace WebApplication_OLAP
{
    public partial class _Default : System.Web.UI.Page
    {
        private List<string> lMeta = new List<string>();
        private List<string[]> lData = new List<string[]>();
        private DataTable objMainTable = new DataTable();

        /*
         * Getters and setters
         */
        public DataTable ObjMainTable
        {
            get { return objMainTable; }
            set { objMainTable = value; }
        }

        /*
         * Page loader
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            // init all cubes for the current server
            InitCubes();
            // init dimensions
            InitDimensions();

            // load tree
            //LoadTree();
        }

        private void LoadTree()
        {
            TreeViewMembers.DataSource = this.ObjMainTable;
            TreeViewMembers.DataBind();
            TreeNode treeNode = new TreeNode("Windows");
            TreeViewMembers.Nodes.Add(treeNode);
            //
            // Another node following the first node.
            //
            treeNode = new TreeNode("Linux");
            TreeViewMembers.Nodes.Add(treeNode);
            //
            // Create two child nodes and put them in an array.
            // ... Add the third node, and specify these as its children.
            //
            TreeNode node2 = new TreeNode("C#");
            TreeNode node3 = new TreeNode("VB.NET");
            TreeNode[] array = new TreeNode[] { node2, node3 };
            //
            // Final node.
            //
            //treeNode = new TreeNode("Dot Net Perls", array);
            //TreeView1.Nodes.Add(treeNode);

        }

        // init all cubes
        private void InitCubes()
        {
            OlapManager objOlapManager = new OlapManager();
            objOlapManager.GetCubes();

            // clear items to avoid duplicates
            DropDownListCubes.Items.Clear();

            List<CubeDef> lCubeList = objOlapManager.LCubes;
            for (int i = 0; i < lCubeList.Count; i++)
            {
                string myItem = lCubeList[i].Name;
                DropDownListCubes.Items.Add(myItem);
            }

            objOlapManager.CloseConnection();
        }

        private static void InitThread()
        {
            //Thread tOlap = new Thread(ExecuteOlapQuery);
        }

        private void ExecuteOlapQuery()
        {
            OlapManager objOlapManager = new OlapManager();
            CellSet objCellSet = objOlapManager.GetQueryResult(TextBoxQuery.Text);

            AdomdDataAdapter objDataAdaptor = new AdomdDataAdapter(objOlapManager.ObjCommand);
            AdomdDataReader objDataReader = objOlapManager.ObjCommand.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable objTable = new DataTable();
            DataColumn myColumn = new DataColumn();
            DataRow myRow = null;

            DataTable objSchemaTable = objDataReader.GetSchemaTable();
            List<string> lMeta = new List<string>();

            // init meta values
            for (int i = 0; i < objSchemaTable.Rows.Count; i++)
                lMeta.Add(objSchemaTable.Rows[i][0].ToString());

            // add columns and column captions
            for (int i = 0; i < objDataReader.FieldCount; i++)
            {
                myColumn = new DataColumn(lMeta[i]);
                objTable.Columns.Add(myColumn);
            }

            // output the rows in the DataReader
            while (objDataReader.Read())
            {
                // new row
                myRow = objTable.NewRow();
                // set the row values
                for (int i = 0; i < objDataReader.FieldCount; i++)
                    myRow[i] = objDataReader[i];

                // add row to the table
                objTable.Rows.Add(myRow);
            }
            // close reader
            objDataReader.Close();

            GridViewData.DataSource = objTable;
            GridViewData.DataBind();

            // export TEST
            //ExportDataTableToExcel(objTable);

            objOlapManager.CloseConnection();

            // load the main table data
            this.ObjMainTable = objTable;
            Session.Add("queryData", objMainTable);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ExecuteOlapQuery();
            /*
            foreach (Position pRow in objCellSet.Axes[1].Positions)
            {
                foreach (Position pCol in objCellSet.Axes[0].Positions)
                {
                    // get the formatted value based on the row and column positions
                    //objDataTable.set
                    Console.Write(objCellSet[pCol.Ordinal, pRow.Ordinal].FormattedValue + ", ");
                }
                Console.WriteLine();
            }*/

            /*String CreateModel = "Create mining model MemberCard_Prediction" +
"(" +
"CustomerID long key," +
"Gender text discrete," +
"Age long continuous," +
"Profession text discrete," +
"Income long continuous," +
"Houseowner text discrete," +
"MemberCard text discrete predict" +
")" +
"Using Microsoft_Decision_Trees";
            OleDbCommand CMD = new OleDbCommand(CreateModel, conn);
            CMD.ExecuteNonQuery();


            String PipeDataToModel = "INSERT  INTO MemberCard_Prediction"
 + "(CustomerId, Gender, Age, Profession, Income, HouseOwner, MemberCard)"
 + "OpenQuery(customerdbsource,"
 + "'Select CustomerId, gender, age, profession, income, houseowner, membercard
   FROM customers')"; 
OleDbCommand CMD = new OleDbCommand(PipeDataToModel, conn);
CMD.ExecuteNonQuery();


            String PredictModel =
 "Select T.CustomerID, MemberCard_Prediction .MemberCard From MemberCard_Prediction" +
 " Natural Prediction Join OpenQuery (Customers, 'select * from NewCustomers) As T" +
 " Where T.Age > 30" +
 " And PredictProbability(MemberCard, 'Gold') >0.75";
OleDbCommand CMD = new OleDbCommand(PredictModel, conn);
OleDbDataReader myReader; myReader = CMD.ExecuteReader(); 
while (myReader.Read()) { 
//Write out data here
}
myReader.Close();*/

        }

        // Fill the data table
        private DataTable GetResultsTable()
        {
            // Create the output table.
            DataTable objTable = new DataTable();

            // Loop through all process names; on rows
            for (int i = 0; i < this.lData.Count; i++)
            {
                // The current process name.
                string sName = this.lMeta[i];

                // Add the program name to our columns.
                //objTable.Columns.Add(sName);

                // Add all of the memory numbers to an object list.
                /*List<object> objectNumbers = new List<object>();

                // Put every column's numbers in this List.
                foreach (string sNumber in this.lData[i])
                    objectNumbers.Add(sNumber);*/

                // Keep adding rows until we have enough.
                while (objTable.Columns.Count < this.lData[i].Length - 1)
                    objTable.Columns.Add();

                // Add each item to the cells in the column.
                for (int a = 0; a < lData[i].Length; a++)
                    objTable.Rows[a][i] = lData[i];
            }

            return objTable;
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // reload dimensions list
            InitDimensions();
        }

        void InitDimensions()
        {
            OlapManager objOlapManager = new OlapManager();

            objOlapManager.GetCubes();
            string selection = DropDownListCubes.SelectedItem.ToString();
            List<CubeDef> lCubeList = objOlapManager.LCubes;

            // scan for selected cube
            for (int i = 0; i < lCubeList.Count; i++)
            {
                string myItem = lCubeList[i].Name;
                if (myItem == selection)
                {
                    objOlapManager.GetDimension(lCubeList[i]);
                    break;
                }
            }

            // clear items to avoid duplicates
            ListBoxDimensions.Items.Clear();
            List<string> lDimensions = objOlapManager.LDim;
            ListBoxDimensions.Rows = lDimensions.Count;
            for (int i = 0; i < lDimensions.Count; i++)
            {
                string myItem = lDimensions[i];
                ListBoxDimensions.Items.Add(myItem);
            }
            // reload component
            ListBoxDimensions.DataBind();

            objOlapManager.CloseConnection();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            // get from session
            if (Session != null)
            {
                ObjMainTable = (DataTable)Session["queryData"];
                // call export method
                ExportDataTableToExcel(this.objMainTable);
            }
        }

        private void ExportDataTableToExcel(DataTable sInputTable)
        {
            // export to Excel
            // create random timestamp
            TimeSpan sTime = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0);
            string timeStamp = ((long)sTime.TotalMilliseconds).ToString();

            ExcelManager em = new ExcelManager();
            if (em.ExcelExport(sInputTable, "OlapReport_" + timeStamp + ".xls", "D5"))
                LabelStatus.Text = "Success!";
        }

        /*protected void btnMapLocation_OnClick(object sender, EventArgs e)
        {
            string top = "var homeSpatialInfo = {";
            string bottom = "};";
            decimal latitude = Convert.ToDecimal(25.774252);
            decimal longitude = Convert.ToDecimal(-80.190262);
            string homeSpatialInfo = String.Format("\"latitude\" : " + latitude + ", \"longitude\" : " + longitude + ", \"zoom\" : \"{2}\"", 0, 0, 13);
            homeSpatialInfo = String.Concat(top, homeSpatialInfo, bottom, Environment.NewLine);
            ScriptManager.RegisterClientScriptBlock(btnMapLocation, btnMapLocation.GetType(), "homeSpatialInfo", homeSpatialInfo, true);
        }*/
    }
}
