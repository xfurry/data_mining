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
using WebApplication_OLAP.classes;

namespace WebApplication_OLAP.pages
{
    public partial class SqlDataMining : System.Web.UI.Page
    {
        //private const string sCatalog = "Adventure Works DW 2008";
        private const string sServer = "CLARITY-7HYGMQM\\ANA";
        //private const string sCatalog = "Adventure Works DW 2008";
        private const string sCatalog = "MyDataBase";
        //private const string sServer = "localhost";

        private List<string> lNodesNames = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // load existing mining structures
                if (!Page.IsPostBack)
                {
                    LoadExistingStructures();
                    InitTableNames();
                    InitColumnsControlls();
                }

                // get from session
                if (Session != null)
                {
                    // initial query data
                    DataTable objTable = new DataTable();

                    if (Session["queryData"] != null)
                    {
                        objTable = (DataTable)Session["queryData"];
                        //GridViewData.DataSource = objTable;
                        //GridViewData.DataBind();
                        // initialize column list
                        //InitializeColumns(objTable);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            // register event
            DropDownListTables.SelectedIndexChanged += new EventHandler(DropDownListTables_SelectedIndexChanged);
            DropDownListKey.SelectedIndexChanged += new EventHandler(DropDownListKey_SelectedIndexChanged);
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
            PanelViewer.Visible = true;
        }

        /*
         * Create mining structure for selected data
         */
        protected void Button1_Click(object sender, EventArgs e)
        {
            // execute mining from olap
            //if (DropDownListSources.SelectedIndex == 1)
            //{
            //    MiningManager objMiner = new MiningManager();
            //    if (objMiner.CreateCubeMiningStructure("OlapStruct", MiningModelAlgorithms.MicrosoftClustering, "Adventure Works", "Customer", ""))
            //        LabelStatus.Text += "Success!";
            //    else
            //        LabelStatus.Text += "Failed!";

            //    return;
            //}

            // Create mining structure based on column and table selection
            List<string> lsInputItems = new List<string>();
            List<string> lsPredictItems = new List<string>();
            List<bool> lbPredictItems = new List<bool>();

            // add values to list
            foreach (ListItem objItem in CheckBoxListInputColumns.Items)
            {
                if (objItem.Selected)
                    lsInputItems.Add(objItem.Text);
            }

            foreach (ListItem objItem in CheckBoxListPredictColumns.Items)
            {
                if (objItem.Selected)
                {
                    // check input column
                    bool bIsPredictOnly = true;

                    // iterate input list and if the item is found in the list then set bool to false
                    for (int i = 0; i < lsInputItems.Count; i++)
                    {
                        if (lsInputItems[i] == objItem.Text)
                        {
                            bIsPredictOnly = false;
                            break;
                        }
                    }

                    lsPredictItems.Add(objItem.Text);
                    lbPredictItems.Add(bIsPredictOnly);
                }
            }

            // check clustering parameter
            try
            {
                int x = Convert.ToInt32(TextBoxCount.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LabelStatus.Text = "Please make sure that the parameter is a number!";
            }

            string sStructName = TextBoxName.Text;
            if (sStructName == "")
                sStructName = "MyMiningStructure";

            string objAlgorithm = null;

            // parameters
            int parOne = 0;
            int parTwo = 0;

            // create mining structure
            if (DropDownListAlgorithm.SelectedIndex == 0)
            {
                objAlgorithm = MiningModelAlgorithms.MicrosoftClustering;
                parOne = DropDownListMethod.SelectedIndex + 1;
                parTwo = Convert.ToInt32(TextBoxCount.Text);
            }
            else if (DropDownListAlgorithm.SelectedIndex == 1)
            {
                objAlgorithm = MiningModelAlgorithms.MicrosoftDecisionTrees;
                parOne = DropDownListScore.SelectedIndex + 1;
                parTwo = DropDownListSplit.SelectedIndex + 1;
            }
            else if (DropDownListAlgorithm.SelectedIndex == 2)
                objAlgorithm = MiningModelAlgorithms.MicrosoftNaiveBayes;
            else if (DropDownListAlgorithm.SelectedIndex == 3)
                objAlgorithm = MiningModelAlgorithms.MicrosoftTimeSeries;

            // warn at missing input column
            if (lsInputItems.Count == 0)
            {
                LabelStatus.Text = "Please select at least one input column!";
                return;
            }

            // warn at prediction column missing for naive bayes and decision trees
            if (objAlgorithm == MiningModelAlgorithms.MicrosoftNaiveBayes || objAlgorithm == MiningModelAlgorithms.MicrosoftDecisionTrees)
            {
                if (lsInputItems.Count == 0 || lsPredictItems.Count == 0)
                {
                    LabelStatus.Text = "Please select at least one input column and at least one prediction column!";
                    return;
                }
            }

            SQLMiningManager objMiningManager = new SQLMiningManager();

            // Create mining query from the existing results
            string sResult = objMiningManager.CreateMiningStructure(lsInputItems, lsPredictItems, objAlgorithm,
                DropDownListTables.SelectedItem.Text, DropDownListKey.SelectedItem.Text, sStructName, lbPredictItems, parOne, parTwo);
            if (sResult == "Success")
            {
                LabelStatus.Text = "Rezultatul procesului: Success!";
                LoadExistingStructures();
            }
            else
                LabelStatus.Text = "Rezultatul procesului: Eroare - " + sResult;
        }

        /*
         * Get results for the currect mining structures
         */
        protected void Button2_Click(object sender, EventArgs e)
        {
            // Query uses mining model name!
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
            // reset list
            DropDownListStructures.DataSource = null;
            DropDownListStructures.DataBind();

            SQLMiningManager objMiningManager = new SQLMiningManager();

            List<string> lStructs = objMiningManager.GetExistingStructures(sCatalog);

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
                    // hide viewer panel and show grid table
                    GridViewDistribution.Visible = true;
                    PanelViewer.Visible = false;

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


        /*
         * Init all table names into a listbox
         */
        private void InitTableNames()
        {
            // show DB tables
            SQLManager manager = new SQLManager("MyDataBase");
            string sQuery = "Select name, id from sysobjects where xtype='U'";

            // handle errors
            //if (manager.GetQueryDataSet(sQuery) == null)
            //{
            //    HandleQueryError();
            //    return;
            //}

            DataSet objSet = manager.GetQueryDataSet(sQuery);
            DropDownListTables.DataSource = objSet;
            DropDownListTables.DataTextField = "name";
            DropDownListTables.DataValueField = "id";
            DropDownListTables.DataBind();
            manager.CloseConnection();
        }

        /*
         * On table list index changed
         */
        protected void DropDownListTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListTables.SelectedIndex <= 0)
                return;

            // init column controlls
            InitColumnsControlls();
        }

        /*
         * Init all columns
         */
        private void InitColumnsControlls()
        {
            // clear current query
            DropDownListKey.DataSource = null;
            DropDownListKey.DataBind();

            // list selected table: to be removed
            //Label1.Text = ListBoxTables.SelectedItem.ToString();

            string sQueryText = "SELECT COLUMN_NAME, ORDINAL_POSITION FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" +
                DropDownListTables.SelectedItem.ToString() + "'ORDER BY ORDINAL_POSITION";

            // execute query
            SQLManager manager = new SQLManager(sCatalog);
            DataTable objTable = new DataTable();

            // handle errors
            //if (manager.GetQueryResult(sQueryText) == null)
            //{
            //    HandleQueryError();
            //    return;
            //}

            objTable.Load(manager.GetQueryResult(sQueryText));

            DropDownListKey.DataSource = objTable;
            DropDownListKey.DataTextField = "COLUMN_NAME";
            DropDownListKey.DataValueField = "ORDINAL_POSITION";
            DropDownListKey.DataBind();

            manager.CloseConnection();

            // init input columns
            InitCheckboxFields();
        }

        /*
         * On key column index changed
         */
        protected void DropDownListKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListKey.SelectedIndex <= 0)
                return;

            // init input columns
            InitCheckboxFields();
        }

        /*
         * Init checkbox fields to exclude the key column
         */
        void InitCheckboxFields()
        {
            CheckBoxListInputColumns.DataSource = null;
            CheckBoxListInputColumns.DataBind();

            CheckBoxListPredictColumns.DataSource = null;
            CheckBoxListPredictColumns.DataBind();

            // execute query
            SQLManager manager = new SQLManager(sCatalog);
            DataTable objTable = new DataTable();

            // handle errors
            //if (manager.GetQueryResult(sQueryText) == null)
            //{
            //    HandleQueryError();
            //    return;
            //}

            // exclude the key column
            string sQueryTextExclude = "SELECT COLUMN_NAME, ORDINAL_POSITION FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" +
                DropDownListTables.SelectedItem.ToString() + "' AND COLUMN_NAME <> '" + DropDownListKey.SelectedItem.ToString() + " 'ORDER BY ORDINAL_POSITION";

            objTable.Load(manager.GetQueryResult(sQueryTextExclude));

            CheckBoxListInputColumns.DataSource = objTable;
            CheckBoxListInputColumns.DataTextField = "COLUMN_NAME";
            CheckBoxListInputColumns.DataValueField = "ORDINAL_POSITION";
            CheckBoxListInputColumns.DataBind();

            CheckBoxListPredictColumns.DataSource = objTable;
            CheckBoxListPredictColumns.DataTextField = "COLUMN_NAME";
            CheckBoxListPredictColumns.DataValueField = "ORDINAL_POSITION";
            CheckBoxListPredictColumns.DataBind();

            manager.CloseConnection();
        }

        /*
         * Check or uncheck input boxes
         */
        protected void ButtonInput_Click(object sender, EventArgs e)
        {
            ChangeState(CheckBoxListInputColumns);
            if (ButtonInput.Text == "Check All")
                ButtonInput.Text = "Uncheck All";
            else
                ButtonInput.Text = "Check All";
        }

        /*
         * Modify the state of the selected
         */
        private void ChangeState(CheckBoxList objList)
        {
            foreach (ListItem objItem in objList.Items)
            {
                if (objItem.Selected)
                    objItem.Selected = false;
                else
                    objItem.Selected = true;
            }
        }

        /*
         * Check or uncheck predict boxes
         */
        protected void ButtonPredict_Click(object sender, EventArgs e)
        {
            ChangeState(CheckBoxListPredictColumns);
            if (ButtonPredict.Text == "Check All")
                ButtonPredict.Text = "Uncheck All";
            else
                ButtonPredict.Text = "Check All";
        }

        /*
         * Customize algorithm
         */
        protected void DropDownListAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListAlgorithm.SelectedIndex >= 0)
                LoadCustomization();
        }

        /*
         * Load customization fields
         */
        void LoadCustomization()
        {
            switch (DropDownListAlgorithm.SelectedIndex)
            {
                case 0:
                    // clustering
                    LabelScore.Visible = false;
                    DropDownListScore.Visible = false;
                    LabelSplit.Visible = false;
                    DropDownListSplit.Visible = false;


                    LabelMethod.Visible = true;
                    DropDownListMethod.Visible = true;
                    LabelCount.Visible = true;
                    TextBoxCount.Visible = true;
                    break;
                case 1:
                    // decision trees
                    LabelMethod.Visible = false;
                    DropDownListMethod.Visible = false;
                    LabelCount.Visible = false;
                    TextBoxCount.Visible = false;


                    LabelScore.Visible = true;
                    DropDownListScore.Visible = true;
                    LabelSplit.Visible = true;
                    DropDownListSplit.Visible = true;
                    break;
                case 2:
                    // naive bayes
                    LabelMethod.Visible = false;
                    DropDownListMethod.Visible = false;
                    LabelCount.Visible = false;
                    TextBoxCount.Visible = false;
                    LabelScore.Visible = false;
                    DropDownListScore.Visible = false;
                    LabelSplit.Visible = false;
                    DropDownListSplit.Visible = false;

                    break;
            }

        }

        /*
         * Export report to excel
         */
        protected void ButtonExport_Click(object sender, EventArgs e)
        {
            // get from session
            if (Session != null)
            {
                DataTable objTable = (DataTable)Session["queryMining"];
                if (objTable == null)
                    return;

                DataTable objTableNodes = (DataTable)Session["queryNode"];

                ExportDataTableToExcel(objTable, objTableNodes);
            }
        }

        /*
         * Exports the selected query data to excel; use random file name by timestamp
         */
        private void ExportDataTableToExcel(DataTable sInputTable, DataTable objNodeTable)
        {
            // export to Excel
            // create random timestamp
            TimeSpan sTime = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0);
            string timeStamp = ((long)sTime.TotalMilliseconds).ToString();

            ExcelManager em = new ExcelManager();
            if (em.ExcelExport(sInputTable, objNodeTable, "MiningReport_" + timeStamp + ".xls"))
                LabelStatus.Text = "Success!";
            else
                LabelStatus.Text = "Failed!";
        }
    }
}
