using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebApplication_OLAP.classes.data_managers;
using Microsoft.AnalysisServices.DataMiningHtmlViewers;
using Microsoft.AnalysisServices;
using System.Data.OleDb;

namespace WebApplication_OLAP.pages
{
    public partial class SqlDataMining : System.Web.UI.Page
    {
        private List<string> lNodesNames = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // load existing mining structures
            if (!Page.IsPostBack)
                LoadExistingStructures();

            // get from session
            if (Session != null)
            {
                // initial query data
                DataTable objTable = new DataTable();

                if (Session["queryData"] != null)
                {
                    objTable = (DataTable)Session["queryData"];
                    GridViewData.DataSource = objTable;
                    GridViewData.DataBind();
                    // initialize column list
                    InitializeColumns(objTable);
                }

                // mining query data
                if (Session["queryMining"] != null)
                {
                    objTable = (DataTable)Session["queryMining"];
                    GridViewResults.DataSource = objTable;
                    GridViewResults.DataBind();
                    // load viewer for the current model
                    LoadViewer();
                }

                // node query data
                if (Session["queryNode"] != null)
                {
                    objTable = (DataTable)Session["queryNode"];
                    GridViewDistribution.DataSource = objTable;
                    GridViewDistribution.DataBind();
                }
            }
        }

        /*
         * Load mining viewer for the selected structure
         */
        private void LoadViewer()
        {
            if (DropDownListStructures.SelectedItem == null)
                return;

            // clear all the controls in order to avoid adding the same control twice
            PanelViewer.Controls.Clear();

            // define objects
            DMHtmlViewer objViewer = null;
            Microsoft.AnalysisServices.AdomdClient.MiningModel objModel = null;
            Microsoft.AnalysisServices.AdomdClient.MiningService objService = null;

            // Todo: make this dynamic
            string sCatalog = "Adventure Works DW 2008";
            string sServer = "CLARITY-7HYGMQM\\ANA";
            string sConnString = "Data Source=" + sServer + "; Initial Catalog=" + sCatalog;
            Microsoft.AnalysisServices.AdomdClient.AdomdConnection objConn = new Microsoft.AnalysisServices.AdomdClient.AdomdConnection(sConnString);

            objConn.Open();
            objModel = objConn.MiningModels[DropDownListStructures.SelectedItem.ToString()];
            objService = objConn.MiningServices[objModel.Algorithm];

            // Viewer types
            /*
            if (service.ViewerType == "Microsoft_Cluster_Viewer")
                viewer = new ClusterViewer();
            else if (service.ViewerType == "Microsoft_Tree_Viewer")
                viewer = new TreeViewer();
            else if (service.ViewerType == "Microsoft_NaiveBayesian_Viewer")
                viewer = new NaiveBayesViewer();
            else if (service.ViewerType == "Microsoft_SequenceCluster_Viewer")
                viewer = new SequenceClusterViewer();
            else if (service.ViewerType == "Microsoft_TimeSeries_Viewer")
                viewer = new TimeSeriesViewer();
            else if (service.ViewerType == "Microsoft_AssociationRules_Viewer")
                viewer = new AssociationViewer();
            else if (service.ViewerType == "Microsoft_NeuralNetwork_Viewer")
                viewer = new NeuralNetViewer();
            else throw new System.Exception("Custom Viewers not supported");
            */

            // switch mining service
            switch (objService.ViewerType)
            {
                case "Microsoft_Cluster_Viewer":
                    objViewer = new DMClusterViewer();
                    break;
                case "Microsoft_Tree_Viewer":
                    objViewer = new DMDecisionTreeViewer();
                    break;
                case "Microsoft_NaiveBayesian_Viewer":
                    objViewer = new DMNaiveBayesViewer();
                    break;
                default:
                    // if none of the above then return
                    return;
            }

            // init data for the current viewer type
            objViewer.Server = sServer;
            objViewer.Database = sCatalog;
            objViewer.Model = DropDownListStructures.SelectedItem.ToString();
            objViewer.DataBind();

            PanelViewer.Controls.Add(objViewer);
        }

        /*
         * Create mining structure for selected data
         */
        protected void Button1_Click(object sender, EventArgs e)
        {
            // get selected key column and selected input columns
            string sKeyColumn = DropDownListKey.SelectedItem.ToString();
            List<string> lInputColumns = new List<string>();

            for (int i = 0; i < CheckBoxListInputColumns.Items.Count; i++)
            {
                if (CheckBoxListInputColumns.Items[i].Selected)
                    lInputColumns.Add(CheckBoxListInputColumns.Items[i].ToString());
            }

            // create mining structure for the current data with the selected columns and key
            // ToDo:

            SQLMiningManager objMiningManager = new SQLMiningManager();
            // return mining result
            if (objMiningManager.CreateMiningStructureIfCan())
            {
                LabelStatus.Text = LabelStatus.Text + "Success!";
                LoadExistingStructures();
            }
            else
                LabelStatus.Text = LabelStatus.Text + "Failed!";
        }

        /*
         * Get results for the currect mining structures
         */
        protected void Button2_Click(object sender, EventArgs e)
        {
            string sQuery = "select NODE_NAME, NODE_TYPE, ATTRIBUTE_NAME, " +
                "[PARENT_UNIQUE_NAME], [CHILDREN_CARDINALITY], NODE_PROBABILITY, MARGINAL_PROBABILITY, NODE_SUPPORT, " +
                "MSOLAP_MODEL_COLUMN, MSOLAP_NODE_SCORE from [" + DropDownListStructures.SelectedItem.ToString() + "].CONTENT";
            InitDataTable(sQuery);
            // load viewer for the current model
            LoadViewer();
        }

        /*
         * Load all the current mining structures
         */
        private void LoadExistingStructures()
        {
            SQLMiningManager objMiningManager = new SQLMiningManager();

            List<string> lStructs = objMiningManager.GetExistingStructures("Adventure Works DW 2008");

            for (int i = 0; i < lStructs.Count; i++)
                DropDownListStructures.Items.Add(lStructs[i].ToString());

            DropDownListStructures.DataBind();
        }

        /*
         * Show distribution node for selected row
         */
        private void DisplayDistributionNode(string sNodeName)
        {
            SQLMiningManager objMiningManager = new SQLMiningManager();

            string sQuery = "select NODE_DISTRIBUTION from [" + DropDownListStructures.SelectedItem.ToString() + "].CONTENT where NODE_NAME ='" + sNodeName + "'";
            // display results
            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objMiningData = objMiningManager.GetMiningResults(sQuery);

            // return for invalid data
            if (objMiningData == null)
                return;

            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objNode = null;

            // output the rows in the DataReader
            while (objMiningData.Read())
            {
                for (int j = 0; j < objMiningData.FieldCount; j++)
                {
                    objNode = (Microsoft.AnalysisServices.AdomdClient.AdomdDataReader)objMiningData[j];

                    // table defines
                    DataTable objTable = new DataTable();
                    DataColumn myColumn = new DataColumn();
                    DataRow myRow = null;

                    // Get the node meta
                    DataTable objSchemaTable = objNode.GetSchemaTable();
                    List<string> lMeta = new List<string>();

                    // init meta values
                    for (int i = 0; i < objSchemaTable.Rows.Count; i++)
                        lMeta.Add(objSchemaTable.Rows[i][0].ToString());

                    // add columns and column captions
                    for (int i = 0; i < objNode.FieldCount; i++)
                    {
                        myColumn = new DataColumn(lMeta[i]);
                        objTable.Columns.Add(myColumn);
                    }

                    // read the node
                    while (objNode.Read())
                    {
                        // new row
                        myRow = objTable.NewRow();
                        // set the row values
                        for (int i = 0; i < objNode.FieldCount; i++)
                            myRow[i] = objNode[i];

                        // add row to the table
                        objTable.Rows.Add(myRow);
                    }
                    // close reader
                    objNode.Close();

                    GridViewDistribution.DataSource = objTable;
                    GridViewDistribution.DataBind();

                    // load the main table data
                    Session.Add("queryNode", objTable);
                }
            }
            // close reader
            objMiningData.Close();
        }

        /*
         * Get selected table row
         */
        protected void GridViewResults_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "NodeView")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button clicked
                // name is always on index = 1
                GridViewRow row = GridViewResults.Rows[index];
                DisplayDistributionNode(row.Cells[1].Text);

                // for remove
                //LabelStatus.Text = row.Cells[5].Text;
            }
        }

        /*
         * Execute custom query
         */
        protected void ButtonQuery_Click(object sender, EventArgs e)
        {
            string sQuery = TextBoxQuery.Text;
            InitDataTable(sQuery);
        }

        /*
         * Execute custom query
         */
        private void InitDataTable(string sQuery)
        {
            SQLMiningManager objMiningManager = new SQLMiningManager();

            // clear node table
            GridViewDistribution.DataSource = null;
            GridViewDistribution.DataBind();

            // display results
            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objMiningData = objMiningManager.GetMiningResults(sQuery);

            if (objMiningData == null)
                return;

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

            GridViewResults.DataSource = objTable;
            GridViewResults.DataBind();

            // load the main table data
            Session.Add("queryMining", objTable);
        }

        /*
         * Initialize column names in checkbox list and dropdown box
         */
        void InitializeColumns(DataTable objMainTable)
        {
            if (objMainTable == null)
                return;

            // reset controlls
            DropDownListKey.DataSource = null;
            DropDownListKey.DataBind();

            CheckBoxListInputColumns.DataSource = null;
            CheckBoxListInputColumns.DataBind();

            // add columns
            for (int i = 0; i < objMainTable.Columns.Count; i++)
            {
                DropDownListKey.Items.Add(objMainTable.Columns[i].ColumnName.ToString());
                CheckBoxListInputColumns.Items.Add(objMainTable.Columns[i].ColumnName.ToString());
            }

            DropDownListKey.DataBind();
            CheckBoxListInputColumns.DataBind();
        }
    }
}
