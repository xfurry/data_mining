using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.AnalysisServices.DataMiningHtmlViewers;
using Microsoft.AnalysisServices;
using System.Data.OleDb;
using WebApplication_OLAP.classes.data_managers;

namespace WebApplication_OLAP.classes
{
    public partial class DataMining : System.Web.UI.Page
    {
        private const string sServer = "CLARITY-7HYGMQM\\ANA";
        private const string sCatalog = "Adventure Works DW 2008";
        //private const string sServer = "localhost";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // load existing mining structures
                if (!Page.IsPostBack)
                {
                    LoadExistingStructures();
                    InitCubes();
                    InitDimensionNames();
                    InitAttributes();
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
            DropDownListCubes.SelectedIndexChanged += new EventHandler(DropDownListCubes_SelectedIndexChanged);
            DropDownListDimensions.SelectedIndexChanged += new EventHandler(DropDownListDimensions_SelectedIndexChanged);
            DropDownListKey.SelectedIndexChanged += new EventHandler(DropDownListKey_SelectedIndexChanged);
        }

        /*
         * Build mining structure
         */
        protected void ButtonStructure_Click(object sender, EventArgs e)
        {
            // Create mining structure based on column and table selection
            List<string> lsInputItems = new List<string>();
            List<string> lsPredictItems = new List<string>();
            List<string> lsInputMeasures = new List<string>();
            List<string> lsPredictMeasures = new List<string>();

            // input items
            foreach (GridViewRow row in GridViewAttributes.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("CheckBoxAtrInput");

                if (cb != null && cb.Checked)
                    lsInputItems.Add(row.Cells[2].Text);
            }

            foreach (GridViewRow row in GridViewMeasures.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("CheckBoxMeasureInput");

                if (cb != null && cb.Checked)
                    lsInputMeasures.Add(row.Cells[2].Text);
            }

            // predict items
            foreach (GridViewRow row in GridViewAttributes.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("CheckBoxAtrPredict");

                if (cb != null && cb.Checked)
                    lsPredictItems.Add(row.Cells[2].Text);
            }


            foreach (GridViewRow row in GridViewMeasures.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("CheckBoxMeasurePredict");

                if (cb != null && cb.Checked)
                    lsPredictMeasures.Add(row.Cells[2].Text);
            }

            string sStructName = TextBoxName.Text;
            if (sStructName == "")
                sStructName = "MyMiningStructure";

            string objAlgorithm = null;

            // create mining structure
            if (DropDownListAlgorithm.SelectedIndex == 0)
                objAlgorithm = MiningModelAlgorithms.MicrosoftClustering;
            else if (DropDownListAlgorithm.SelectedIndex == 1)
                objAlgorithm = MiningModelAlgorithms.MicrosoftDecisionTrees;
            else if (DropDownListAlgorithm.SelectedIndex == 2)
                objAlgorithm = MiningModelAlgorithms.MicrosoftNaiveBayes;
            else if (DropDownListAlgorithm.SelectedIndex == 3)
                objAlgorithm = MiningModelAlgorithms.MicrosoftTimeSeries;

            // warn at missing input column
            if (lsInputItems.Count == 0)
            {
                LabelStatus.Text = LabelStatus.Text + "Please select at least one input column!";
                return;
            }

            // warn at prediction column missing for naive bayes and decision trees
            if (objAlgorithm == MiningModelAlgorithms.MicrosoftNaiveBayes || objAlgorithm == MiningModelAlgorithms.MicrosoftDecisionTrees)
            {
                if (lsInputItems.Count == 0 || lsPredictItems.Count == 0)
                {
                    LabelStatus.Text = LabelStatus.Text + "Please select at least one input column and at least one prediction column!";
                    return;
                }
            }

            MiningManager objMiningManager = new MiningManager();

            //// Create mining query from the existing results
            //if (objMiningManager.CreateMiningStructure(lsInputItems, lsPredictItems, objAlgorithm,
            //    DropDownListTables.SelectedItem.Text, DropDownListKey.SelectedItem.Text, sStructName))
            //{
            //    LabelStatus.Text = LabelStatus.Text + "Success!";
            //    LoadExistingStructures();
            //}
            //else
            //    LabelStatus.Text = LabelStatus.Text + "Failed!\r\n" + objMiningManager.SMiningError;



        }

        /*
         * Update key attribute
         */
        protected void DropDownListKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListKey.SelectedIndex <= 0)
                return;

            // init input columns
            string sQuery = "SELECT HIERARCHY_NAME FROM $system.mdschema_hierarchies WHERE CUBE_NAME = '" +
                DropDownListCubes.SelectedItem.Text + "' AND [DIMENSION_UNIQUE_NAME] = '[" +
                DropDownListDimensions.SelectedItem.Text + "]' AND HIERARCHY_NAME <> '" + DropDownListKey.SelectedItem.Text + "'";
            InitAttributeAndMeasures(sQuery, GridViewAttributes);
        }

        /*
         * Update dimension list
         */
        protected void DropDownListDimensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListDimensions.SelectedIndex <= 0)
                return;

            // init column controlls
            InitAttributes();
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
         * Show mining results
         */
        protected void ButtonResult_Click(object sender, EventArgs e)
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
         * Execute custom query
         */
        protected void ButtonExecute_Click(object sender, EventArgs e)
        {
            string sQuery = TextBoxQuery.Text;
            InitDataTable(sQuery);
        }

        /*
         * Init all attributes
         */
        private void InitAttributes()
        {
            // clear current query
            DropDownListKey.DataSource = null;
            DropDownListKey.DataBind();

            MiningManager objMiningManager = new MiningManager();

            string sQuery = "SELECT HIERARCHY_NAME FROM $system.mdschema_hierarchies WHERE CUBE_NAME = '" +
                DropDownListCubes.SelectedItem.Text + "' AND [DIMENSION_UNIQUE_NAME] = '[" +
                DropDownListDimensions.SelectedItem.Text + "]'";
            // display results
            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objMiningData = objMiningManager.GetQueryResult(sQuery);

            List<string> sCubes = new List<string>();

            try
            {
                while (objMiningData.Read())
                {
                    for (int i = 0; i < objMiningData.FieldCount; i++)
                    {
                        object value = objMiningData.GetValue(i);
                        string strValue = (value == null) ? string.Empty : value.ToString();
                        sCubes.Add(strValue);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            DropDownListKey.DataSource = sCubes;
            DropDownListKey.DataBind();

            // init input columns
            sQuery = "SELECT HIERARCHY_NAME FROM $system.mdschema_hierarchies WHERE CUBE_NAME = '" +
                DropDownListCubes.SelectedItem.Text + "' AND [DIMENSION_UNIQUE_NAME] = '[" +
                DropDownListDimensions.SelectedItem.Text + "]' AND HIERARCHY_NAME <> '" + DropDownListKey.SelectedItem.Text + "'";
            InitAttributeAndMeasures(sQuery, GridViewAttributes);

            // init measures
            sQuery = "SELECT MEASURE_NAME FROM $system.mdschema_measures WHERE CUBE_NAME = '" +
                DropDownListCubes.SelectedItem.Text + "'";
            InitAttributeAndMeasures(sQuery, GridViewMeasures);
        }

        /*
         * Refresh dimensions
         */
        protected void DropDownListCubes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCubes.SelectedIndex <= 0)
                return;

            // init dimensions
            InitDimensionNames();
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
         * Execute query
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
         * Init all dimension names into a listbox
         */
        private void InitDimensionNames()
        {
            MiningManager objMiningManager = new MiningManager();

            string sQuery = "SELECT DIMENSION_NAME FROM $system.mdschema_dimensions WHERE CUBE_NAME = '" +
                DropDownListCubes.SelectedItem.Text + "'";
            // display results
            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objMiningData = objMiningManager.GetQueryResult(sQuery);

            List<string> sCubes = new List<string>();
            try
            {
                while (objMiningData.Read())
                {
                    for (int i = 0; i < objMiningData.FieldCount; i++)
                    {
                        object value = objMiningData.GetValue(i);
                        string strValue = (value == null) ? string.Empty : value.ToString();
                        sCubes.Add(strValue);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            DropDownListDimensions.DataSource = sCubes;
            DropDownListDimensions.DataBind();
        }

        /*
         * Init all cubes
         */
        private void InitCubes()
        {
            MiningManager objMiningManager = new MiningManager();

            string sQuery = "SELECT CUBE_NAME FROM $system.mdschema_cubes WHERE left(CUBE_NAME, 1) <> '$' AND len(BASE_CUBE_NAME) = 0";
            // display results
            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objMiningData = objMiningManager.GetQueryResult(sQuery);

            List<string> sCubes = new List<string>();
            try
            {
                while (objMiningData.Read())
                {
                    for (int i = 0; i < objMiningData.FieldCount; i++)
                    {
                        object value = objMiningData.GetValue(i);
                        string strValue = (value == null) ? string.Empty : value.ToString();
                        sCubes.Add(strValue);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            DropDownListCubes.DataSource = sCubes;
            DropDownListCubes.DataBind();

        }

        /*
         * Init checkbox fields to exclude the key attribute
         */
        void InitAttributeAndMeasures(string sQuery, GridView objGrid)
        {
            MiningManager objMiningManager = new MiningManager();

            // display results
            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objMiningData = objMiningManager.GetQueryResult(sQuery);

            List<string> sAtributes = new List<string>();
            try
            {
                while (objMiningData.Read())
                {
                    for (int i = 0; i < objMiningData.FieldCount; i++)
                    {
                        object value = objMiningData.GetValue(i);
                        string strValue = (value == null) ? string.Empty : value.ToString();
                        sAtributes.Add(strValue);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            objGrid.DataSource = sAtributes;
            objGrid.DataBind();
        }
    }
}
