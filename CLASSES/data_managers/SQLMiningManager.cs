using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.AnalysisServices;
using System.Data.OleDb;
using System.Data;

namespace WebApplication_OLAP.classes.data_managers
{
    public class SQLMiningManager
    {
        //private const string sCatalog = "Adventure Works DW 2008";
        //private const string sServer = "CLARITY-7HYGMQM\\ANA";
        private const string sCatalog = "Adventure Works DW 2008";
        private const string sServer = "localhost";

        private string sStructureName = "MyMiningStructure";            // to be removed
        private string sModelName = "MyMiningModel";                    // to be removed

        public bool CreateMiningStructureIfCan()
        {
            try
            {
                // init server connection
                Server svr = new Server();
                svr.Connect("integrated security=SSPI;data source=" + sServer + ";persist security info=False;initial catalog=" + sCatalog);

                // Connect to the Analysis Service server
                Database currentDB = GetCurrentDatabase(sCatalog);
                currentDB.Refresh();

                // if current database doesn't exist, then create it
                //if (currentDB == null)
                //    CreateNewDatabase();

                // create a new mining structure
                MiningStructure currentStructure = CreateMiningStructure(currentDB);
                currentStructure.Refresh();

                // create a mining model for the selected structure
                CreateModels(currentStructure);

                // Process Database and structure
                currentStructure.Process();
                //ProcessDatabase(myDB);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return false;
        }

        /*
         * Create mining structure based on selection
         */
        public bool CreateMiningStructure(List<string> inputColumns, List<string> predictColumns, string sAlgorithm, string sTableName, string sKeyColumn, string sStructureName)
        {
            try
            {
                // Connect to the Analysis Service server
                Database currentDB = GetCurrentDatabase(sCatalog);

                // create a new mining structure
                MiningStructure currentStructure = CreateCustomMiningStructure(currentDB, sStructureName, sTableName, sKeyColumn, inputColumns, predictColumns);

                // create a mining model for the selected structure
                CreateCustomModel(currentStructure, sAlgorithm, sStructureName, sKeyColumn);

                // Process Database and structure
                currentStructure.Process();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return false;
        }

        /*
         * Return mining results from query
         */
        public Microsoft.AnalysisServices.AdomdClient.AdomdDataReader GetMiningResults(string sQuery)
        {
            try
            {
                string sConnString = "Data Source=" + sServer + "; Initial Catalog=" + sCatalog;
                Microsoft.AnalysisServices.AdomdClient.AdomdConnection objConn = new Microsoft.AnalysisServices.AdomdClient.AdomdConnection(sConnString);
                objConn.Open();
                Microsoft.AnalysisServices.AdomdClient.AdomdCommand objCmd = objConn.CreateCommand();
                objCmd.CommandText = sQuery;

                /*
                "SELECT FLATTENED PredictHistogram(Generation) " +
                "FROM [Generation Trees] " +
                "NATURAL PREDICTION JOIN " +
                "( SELECT " +
                " (SELECT ’Cinemax’ AS Channel UNION " +
                " SELECT ’Showtime’ AS Channel) AS PayChannels " +
                ") AS T ";*/

                //Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objReader = objCmd.ExecuteReader();
                //Microsoft.AnalysisServices.AdomdClient.AdomdDataAdapter objDataAdaptor = new Microsoft.AnalysisServices.AdomdClient.AdomdDataAdapter(objCmd);

                Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objDataReader = objCmd.ExecuteReader(CommandBehavior.CloseConnection);

                /*
                try
                {
                    for (int i = 0; i < objDataReader.FieldCount; i++)
                    {
                        Console.Write(objDataReader.GetName(i) + "\t");
                    }
                    Console.WriteLine();
                    while (objDataReader.Read())
                    {
                        for (int i = 0; i < objDataReader.FieldCount; i++)
                        {
                            object value = objDataReader.GetValue(i);
                            string strValue = (value == null) ?
                            string.Empty : value.ToString();
                            Console.Write(strValue + "\t");
                        }
                        Console.WriteLine();
                    }
                }
                finally
                {
                    objDataReader.Close();
                }
                */

                return objDataReader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        /*
         * Returns an existing DB
         */
        private Database GetCurrentDatabase(string sCatalogName)
        {
            try
            {
                Server srv = new Server();
                srv.Connect("integrated security=SSPI;data source=" + sServer + ";persist security info=False;initial catalog=" + sCatalog);
                Database myDB = srv.Databases[sCatalog];

                return myDB;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        /*
         * Return current list with mining structures
         */
        public List<string> GetExistingStructures(string sDbName)
        {
            List<string> lStructures = new List<string>();
            Database currentDB = GetCurrentDatabase(sDbName);

            foreach (MiningStructure objMiningStruc in currentDB.MiningStructures)
                lStructures.Add(objMiningStruc.Name.ToString());

            return lStructures;
        }

        /*
         * Drop an existing mining structure; used in order to avoid the crash if the user wants to add a structure with the same name
         */
        private void DropExistingStructures(Database objDataBase, string sName)
        {
            foreach (MiningStructure objMiningStruc in objDataBase.MiningStructures)
            {
                if (objMiningStruc.Name == sName)
                {
                    objMiningStruc.Drop();
                    break;
                }
            }
        }

        /*
         * Drop an existing mining model; used in order to avoid the crash if the user wants to add a model with the same name
         */
        private void DropExistingMiningModels(MiningStructure objStruct, string sName)
        {
            foreach (MiningModel objModel in objStruct.MiningModels)
            {
                if (objModel.Name == sName)
                {
                    objModel.Drop();
                    break;
                }
            }
        }

        /*
         * Demo code!
         */
        private void AddTable(DataSourceView dsv, OleDbConnection connection, string tableName, string dataSourceID)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter(
                "SELECT * FROM [dbo].[" + tableName + "] WHERE 1=0",
                connection);
            DataTable[] dataTables = adapter.FillSchema(dsv.Schema,
              SchemaType.Mapped, tableName);
            DataTable dataTable = dataTables[0];

            dataTable.ExtendedProperties.Add("TableType", "Table");
            dataTable.ExtendedProperties.Add("DbSchemaName", "dbo");
            dataTable.ExtendedProperties.Add("DbTableName", tableName);
            dataTable.ExtendedProperties.Add("FriendlyName", tableName);
            dataTable.ExtendedProperties.Add("DataSourceID", dataSourceID);
        }

        /*
         * Create mining structure with cusomt fields
         */
        private MiningStructure CreateCustomMiningStructure(Database objDatabase, string sStructName, string sTableName, string sKeyColumn, List<string> lsInputColumns, List<string> lsPredictColumns)
        {
            // drop the existing structures with the same name
            DropExistingStructures(objDatabase, sStructName);

            // Initialize a new mining structure
            MiningStructure currentMiningStruct = new MiningStructure(sStructName, sStructName);
            currentMiningStruct.Source = new DataSourceViewBinding("Adventure Works DW");

            // get data type for the selected column
            string sQueryText = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" +
                sTableName + "' AND COLUMN_NAME = '" + sKeyColumn + "'";

            // execute query
            SQLManager manager = new SQLManager("AdventureWorksDW");
            DataTable objTable = new DataTable();

            // get column data type
            objTable.Load(manager.GetQueryResult(sQueryText));
            string sDataType = objTable.Rows[0][0].ToString();

            manager.CloseConnection();

            // create key column
            ScalarMiningStructureColumn StructKey = new ScalarMiningStructureColumn(sKeyColumn, sKeyColumn);
            StructKey.Type = GetColumnStructureType(sDataType);
            StructKey.Content = MiningStructureColumnContents.Key;
            StructKey.IsKey = true;
            StructKey.KeyColumns.Add("dbo_" + sTableName, sKeyColumn, GetColumnDataType(sDataType));
            currentMiningStruct.Columns.Add(StructKey);

            // input columns
            for (int i = 0; i < lsInputColumns.Count; i++)
            {
                // get data type for the selected column
                sQueryText = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" +
                    sTableName + "' AND COLUMN_NAME = '" + lsInputColumns[i] + "'";

                // get column data type
                objTable = new DataTable();
                objTable.Load(manager.GetQueryResult(sQueryText));
                sDataType = objTable.Rows[0][0].ToString();

                // Generation column
                ScalarMiningStructureColumn Input = new ScalarMiningStructureColumn(lsInputColumns[i], lsInputColumns[i]);
                Input.Type = GetColumnStructureType(sDataType);
                if (Input.Type == MiningStructureColumnTypes.Long)
                    Input.Content = MiningStructureColumnContents.Continuous;
                else
                    Input.Content = MiningStructureColumnContents.Discrete;
                // Add data binding to the column
                Input.KeyColumns.Add("dbo_" + sTableName, lsInputColumns[i], GetColumnDataType(sDataType));
                // Add the column to the mining structure
                currentMiningStruct.Columns.Add(Input);

                manager.CloseConnection();
            }

            // Add the mining structure to the database
            objDatabase.MiningStructures.Add(currentMiningStruct);
            currentMiningStruct.Update();

            return currentMiningStruct;
        }

        /*
         * Get column type depending on the db table column data type
         */
        string GetColumnStructureType(string sData)
        {
            switch (sData)
            {
                case "smallint":
                case "int":
                case "tinyint":
                case "money":
                    return MiningStructureColumnTypes.Long;
                case "nvarchar":
                case "nchar":
                    return MiningStructureColumnTypes.Text;
                case "datetime":
                    return MiningStructureColumnTypes.Date;
                case "bit":                                 // this is not correct, but we are not expecting such values
                    return MiningStructureColumnTypes.Text;
            }

            return null;
        }

        /*
         * Get current Datatype
         */
        OleDbType GetColumnDataType(string sData)
        {
            switch (sData)
            {
                case "smallint":
                case "int":
                case "tinyint":
                case "datetime":
                case "bit":
                    return OleDbType.Integer;
                case "nvarchar":
                case "nchar":
                    return OleDbType.WChar;
                case "money":
                    return OleDbType.Currency;
            }

            return OleDbType.Error;
        }

        /*
         * Create mining model with custom fields and algorithm
         */
        private void CreateCustomModel(MiningStructure objStructure, string sAlgorithm, string sModelName, string sKeyColumn)
        {
            DropExistingMiningModels(objStructure, sModelName);

            switch (sAlgorithm)
            {
                case MiningModelAlgorithms.MicrosoftClustering:
                    MiningModel ClusterModel;

                    ClusterModel = objStructure.CreateMiningModel(true, sModelName);
                    ClusterModel.Algorithm = sAlgorithm;
                    ClusterModel.AlgorithmParameters.Add("CLUSTER_COUNT", 0);

                    ClusterModel.Update();

                    break;
            }
        }

        /*
         * Create mining structure for selected database
         */
        private MiningStructure CreateMiningStructure(Database objDatabase)
        {
            // drop the existing structures with the same name
            DropExistingStructures(objDatabase, sStructureName);

            // Initialize a new mining structure
            MiningStructure currentMiningStruct = new MiningStructure(sStructureName, sStructureName);
            currentMiningStruct.Source = new DataSourceViewBinding("Adventure Works DW");
            DataSourceView currentDataSource = new DataSourceView("Adventure Works DW");


            // Create the columns of the mining structure
            // setting the type, content and data binding
            // User Id column
            ScalarMiningStructureColumn StructKey = new ScalarMiningStructureColumn("StructKey", "StructKey");
            StructKey.Type = MiningStructureColumnTypes.Long;
            StructKey.Content = MiningStructureColumnContents.Key;
            StructKey.IsKey = true;
            // Add data binding to the column
            StructKey.KeyColumns.Add("dbo_DimCustomer", "CustomerKey", System.Data.OleDb.OleDbType.Integer);
            // Add the column to the mining structure
            currentMiningStruct.Columns.Add(StructKey);


            // Generation column
            ScalarMiningStructureColumn Gender = new ScalarMiningStructureColumn("Gender", "Gender");
            Gender.Type = MiningStructureColumnTypes.Text;
            Gender.Content = MiningStructureColumnContents.Discrete;
            // Add data binding to the column
            Gender.KeyColumns.Add("dbo_DimCustomer", "Gender", System.Data.OleDb.OleDbType.WChar);
            // Add the column to the mining structure
            currentMiningStruct.Columns.Add(Gender);


            // Add Nested table by creating a table column and adding
            // a key column to the nested table
            /*
            TableMiningStructureColumn PayChannels = new TableMiningStructureColumn("PayChannels", "PayChannels");
            PayChannels.ForeignKeyColumns.Add("dbo_Customer", "TotalChildren", System.Data.OleDb.OleDbType.Integer);

            ScalarMiningStructureColumn Channel = new ScalarMiningStructureColumn("Channel", "Channel");
            Channel.Type = MiningStructureColumnTypes.Text;
            Channel.Content = MiningStructureColumnContents.Key;
            Channel.IsKey = true;
            // Add data binding to the column
            Channel.KeyColumns.Add("dbo_Customer", "FirstName", System.Data.OleDb.OleDbType.WChar);
            PayChannels.Columns.Add(Channel);
            currentMiningStruct.Columns.Add(PayChannels);
             * */


            // Add the mining structure to the database
            objDatabase.MiningStructures.Add(currentMiningStruct);
            currentMiningStruct.Update();

            return currentMiningStruct;
        }

        /*
         * Create mining model for the selected mining strucutre
         */
        private void CreateModels(MiningStructure objStructure)
        {
            MiningModel ClusterModel;
            MiningModel TreeModel;
            MiningModelColumn mmc;

            // Create the Cluster model and set the algorithm
            // and parameters
            DropExistingMiningModels(objStructure, sModelName);
            ClusterModel = objStructure.CreateMiningModel(true, sModelName);
            ClusterModel.Algorithm = MiningModelAlgorithms.MicrosoftClustering;// "Microsoft_Clustering";
            ClusterModel.AlgorithmParameters.Add("CLUSTER_COUNT", 0);
            ClusterModel.Update();


            // The CreateMiningModel method adds
            // all the structure columns to the collection
            // Copy the Cluster model and change the necessary properties
            TreeModel = ClusterModel.Clone();
            DropExistingMiningModels(objStructure, sModelName + "Generation Trees");
            TreeModel.Name = sModelName + "Generation Trees";
            TreeModel.ID = sModelName + "Generation Trees";
            TreeModel.Algorithm = MiningModelAlgorithms.MicrosoftDecisionTrees;// "Microsoft_Decision_Trees";
            TreeModel.AlgorithmParameters.Clear();
            TreeModel.Columns["Gender"].Usage = "Predict";
            //TreeModel.Columns["PayChannels"].Usage = "Predict";

            // Add an aliased copy of the PayChannels table to the trees model
            mmc = TreeModel.Columns.Add("MaritalStatus");
            mmc.SourceColumnID = "MaritalStatus";
            mmc = mmc.Columns.Add("MaritalStatus");
            mmc.SourceColumnID = "MaritalStatus";
            mmc.Usage = "Key";
            // Now set a filter on the PayChannels_Hbo_Encore table and use it
            // as input to predict other channels

            //TreeModel.Columns["PayChannels_Hbo_Encore"].Filter = "Channel=’HBO’ OR Channel=’Encore’";
            // Set a complementary filter on the payChannels predictable
            // nested table

            //TreeModel.Columns["PayChannels"].Filter = "Channel<>’HBO’ AND Channel<>’Encore’";
            objStructure.MiningModels.Add(TreeModel);

            // Submit the models to the server
            // ToDo: fix this
            //TreeModel.Update();
        }

        /*
         * Process database
         */
        private void ProcessDatabase(Database objDatabase, Server objServer)
        {
            Trace t;
            TraceEvent e;
            // create the trace object to trace progress reports
            // and add the column containing the progress description
            t = objServer.Traces.Add();
            e = t.Events.Add(TraceEventClass.ProgressReportCurrent);
            e.Columns.Add(TraceColumn.TextData);
            t.Update();

            // Add the handler for the trace event
            t.OnEvent += new TraceEventHandler(ProgressReportHandler);

            try
            {
                // start the trace, process of the database, then stop it
                t.Start();
                objDatabase.Process(ProcessType.ProcessFull);
                t.Stop();
            }
            catch (System.Exception /*ex*/)
            {
            }
        }

        private void ProgressReportHandler(object sender, TraceEventArgs e)
        {
            Console.WriteLine(e[TraceColumn.TextData]);
        }

        /*
         * Discover mining services; for future use
         */
        private void DiscoverServices()
        {
            Microsoft.AnalysisServices.AdomdClient.AdomdConnection connection = new Microsoft.AnalysisServices.AdomdClient.AdomdConnection("Data Source=" + sServer + "; Initial Catalog=" + sCatalog);
            connection.Open();

            foreach (Microsoft.AnalysisServices.AdomdClient.MiningService ms in connection.MiningServices)
            {
                Console.WriteLine("Service: " + ms.Name);
                foreach (Microsoft.AnalysisServices.AdomdClient.MiningServiceParameter mp in ms.AvailableParameters)
                {
                    Console.WriteLine(" Parameter: " + mp.Name + " Default: " + mp.DefaultValue);
                }
            }
            connection.Close();
        }

        /*
         * Setup mining permissions; for future use
         */
        private void SetModelPermissions(Database objDb, MiningModel objModel)
        {
            // Create a new role and add members
            Role newRole = new Role("ModelReader", "ModelReader");
            newRole.Members.Add(new RoleMember("redmond\\jamiemac"));
            newRole.Members.Add(new RoleMember("redmond\\zhaotang"));
            newRole.Members.Add(new RoleMember("redmond\\bogdanc"));

            // Add the role to the database and updat
            objDb.Roles.Add(newRole);
            newRole.Update();

            // Create a permission object referring the role
            MiningModelPermission newMiningPermision = new MiningModelPermission();
            newMiningPermision.Name = "ModelReader";
            newMiningPermision.ID = "ModelReader";
            newMiningPermision.RoleID = "ModelReader";
            // Assign access rights to the permission
            newMiningPermision.Read = ReadAccess.Allowed;
            newMiningPermision.AllowBrowsing = true;
            newMiningPermision.AllowDrillThrough = true;
            newMiningPermision.ReadDefinition = ReadDefinitionAccess.Allowed;
            // Add permission to the model and update
            objModel.MiningModelPermissions.Add(newMiningPermision);
            newMiningPermision.Update();
        }

        /*
         * Create new database; for future use
         */
        private Database CreateDatabase(Server objServer)
        {
            // Create a database and set the properties
            Database newDB = new Database();
            newDB.Name = "Chapter 16";
            newDB.ID = "Chapter 16";
            // Add the database to the server and commit
            objServer.Databases.Add(newDB);
            newDB.Update();

            return newDB;
        }

        /*
         * Create new database objects; for future use
         */
        private void CreateDataAccessObjects(Database objDB)
        {
            // Create a relational data source
            // by specifying the name and the id
            RelationalDataSource ds = new RelationalDataSource("MovieClick", Utils.GetSyntacticallyValidID("MovieClick", typeof(Database)));
            ds.ConnectionString = "Provider=SQLNCLI10.1;Data Source=localhost;Integrated Security=SSPI;Initial Catalog=Chapter 16";
            objDB.DataSources.Add(ds);

            // Create connection to datasource to extract schema to a dataset
            DataSet dset = new DataSet();
            SqlConnection cn = new SqlConnection("Data Source=localhost; Initial Catalog=Chapter 16; Integrated Security=true");

            // Create data adapters from database tables and load schemas
            SqlDataAdapter daCustomers = new SqlDataAdapter("SELECT * FROM Customers", cn);
            daCustomers.FillSchema(dset, SchemaType.Mapped, "Customers");
            SqlDataAdapter daChannels = new SqlDataAdapter("SELECT * FROM Channels", cn);
            daChannels.FillSchema(dset, SchemaType.Mapped, "Channels");

            // Add relationship between Customers and Channels
            DataRelation drCustomerChannels = new DataRelation("Customer_Channels", dset.Tables["Customers"].Columns["SurveyTakenID"], dset.Tables["Channels"].Columns["SurveyTakenID"]);
            dset.Relations.Add(drCustomerChannels);

            // Create the DSV, ad the dataset and add to the database
            DataSourceView dsv = new DataSourceView("SimpleMovieClick", "SimpleMovieClick");
            dsv.DataSourceID = "MovieClick";
            dsv.Schema = dset.Clone();
            objDB.DataSourceViews.Add(dsv);

            // Update the database to create the objects on the server
            objDB.Update(UpdateOptions.ExpandFull);
        }

        /*
         * Create new database objects; for future use
         */
        void AddNewDataAccessObjects(Database db)
        {
            // Create connection to datasource cto extract schema to a dataset
            DataSet dset = new DataSet();
            SqlConnection cn = new SqlConnection("Data Source=localhost; Initial Catalog=Chapter 16; Integrated Security=true");
            // Create the Customers data adapter with the calculated appended
            SqlDataAdapter daCustomers = new SqlDataAdapter(
            "SELECT *, " +
            "(CASE WHEN (Age < 30) THEN ’GenY’ " +
            " WHEN (Age >= 30 AND Age < 40) THEN ’GenX’ " +
            "ELSE ’Baby Boomer’ END) AS Generation " +
            "FROM Customers", cn);
            daCustomers.FillSchema(dset, SchemaType.Mapped, "Customers");
            // Add Extended properties to the Generation column indicating to
            // Analysis Services that it is a calculated column
            DataColumn genColumn = dset.Tables["Customers"].Columns
            ["Generation"];
            genColumn.ExtendedProperties.Add("DbColumnName", "Generation");
            genColumn.ExtendedProperties.Add("Description",
            "Customer generation");
            genColumn.ExtendedProperties.Add("IsLogical", "true");
            genColumn.ExtendedProperties.Add("ComputedColumnExpression",
            "CASE WHEN (Age < 30) THEN ’GenY’ " +
            "WHEN (Age >= 30 AND Age < 40) THEN ’GenX’ " +
            "ELSE ’Baby Boomer’ END");
            // Create a ’Pay Channels’ data adapter with a customer query
            // for our named query
                        SqlDataAdapter daPayChannels = new SqlDataAdapter(
            "SELECT * FROM Channels " +
            "WHERE Channel IN (’Cinemax’, ’Encore’, ’HBO’, ’Showtime’, " +
            "’STARZ!’, ’The Movie Channel’)", cn);
            daPayChannels.FillSchema(dset, SchemaType.Mapped, "PayChannels");
            // Add Extended properties to the PayChannels table indicating to
            // Analysis Services that it is a named query
            DataTable pcTable = dset.Tables["PayChannels"];
            pcTable.ExtendedProperties.Add("IsLogical", "true");
            pcTable.ExtendedProperties.Add("Description",
            "Channels requiring an additional fee");
            pcTable.ExtendedProperties.Add("TableType", "View");
            pcTable.ExtendedProperties.Add("QueryDefinition",
            "SELECT * FROM Channels " +
            "WHERE Channel IN (’Cinemax’, ’Encore’, ’HBO’, ’Showtime’, " +
            "’STARZ!’, ’The Movie Channel’)");
            // Add relationship between Customers and PayChannels
            DataRelation drCustomerPayChannels = new DataRelation(
            "CustomerPayChannels",
            dset.Tables["Customers"].Columns["SurveyTakenID"],
            dset.Tables["PayChannels"].Columns["SurveyTakenID"]);
            dset.Relations.Add(drCustomerPayChannels);
            // Access the data source and the DSV created previously
            // by specifying the ID
            DataSourceView dsv = new DataSourceView("MovieClick", "MovieClick");
            dsv.DataSourceID = "MovieClick";
            dsv.Schema = dset.Clone();
            db.DataSourceViews.Add(dsv);
            // Update the database to create the objects on the server
            db.Update(UpdateOptions.ExpandFull);
        }

        public void SimplePredictionQuery()
        {
            Microsoft.AnalysisServices.AdomdClient.AdomdConnection connection = new Microsoft.AnalysisServices.AdomdClient.AdomdConnection();
            connection.ConnectionString =
            "Data Source=localhost; Initial Catalog=Chapter 16";
            connection.Open();
            Microsoft.AnalysisServices.AdomdClient.AdomdCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            "SELECT Predict(Generation) FROM [Generation Trees] " +
            "NATURAL PREDICTION JOIN " +
            "( SELECT " +
            " (SELECT ’Cinemax’ AS Channel UNION " +
            " SELECT ’Showtime’ AS Channel) AS PayChannels " +
            ") AS T ";
            // execute the command and display the prediction result
            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string predictedGeneration = reader.GetValue(0).ToString();
                Console.WriteLine(predictedGeneration);
            }
            reader.Close();
            connection.Close();
        }

        public void MultipleRowQuery(Microsoft.AnalysisServices.AdomdClient.AdomdConnection objConn)
        {
            Microsoft.AnalysisServices.AdomdClient.AdomdCommand cmd = objConn.CreateCommand();
            cmd.CommandText =
            "SELECT FLATTENED PredictHistogram(Generation) " +
            "FROM [Generation Trees] " +
            "NATURAL PREDICTION JOIN " +
            "( SELECT " +
            " (SELECT ’Cinemax’ AS Channel UNION " +
            " SELECT ’Showtime’ AS Channel) AS PayChannels " +
            ") AS T ";
            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader reader = cmd.ExecuteReader();
            try
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader.GetName(i) + "\t");
                }
                Console.WriteLine();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        object value = reader.GetValue(i);
                        string strValue = (value == null) ?
                        string.Empty : value.ToString();
                        Console.Write(strValue + "\t");
                    }
                    Console.WriteLine();
                }
            }
            finally
            {
                reader.Close();
            }

            // Demo code
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    // Check for nested table columns
                    if (reader.GetFieldType(i) == typeof(Microsoft.AnalysisServices.AdomdClient.AdomdDataReader))
                    {
                        // fetch the nested data reader
                        Microsoft.AnalysisServices.AdomdClient.AdomdDataReader nestedReader = reader.GetDataReader(i);
                        while (nestedReader.Read())
                        {
                            for (int j = 0; j < nestedReader.FieldCount; j++)
                            {
                                object value = nestedReader.GetValue(j);
                                string strValue = (value == null) ?
                                string.Empty : value.ToString();
                                Console.Write(strValue);
                            }
                            Console.WriteLine();
                        }
                        // close the nested reader
                        nestedReader.Close();
                    }
                }
            }

            cmd.CommandText =
            "SELECT Predict(Generation) FROM [Generation Trees] " +
            "NATURAL PREDICTION JOIN " +
            "( SELECT " +
            " (SELECT @Channel1 AS Channel UNION " +
            " SELECT @Channel2 AS Channel) AS PayChannels " +
            ") AS T ";
            Microsoft.AnalysisServices.AdomdClient.AdomdParameter p1 = new Microsoft.AnalysisServices.AdomdClient.AdomdParameter();
            p1.ParameterName = "Channel1";
            p1.Value = "Cinemax";
            cmd.Parameters.Add(p1);
            Microsoft.AnalysisServices.AdomdClient.AdomdParameter p2 = new Microsoft.AnalysisServices.AdomdClient.AdomdParameter();
            p2.ParameterName = "Channel2";
            p2.Value = "Showtime";
            cmd.Parameters.Add(p2);


            Microsoft.AnalysisServices.AdomdClient.AdomdCommand cmd2 = objConn.CreateCommand();
            cmd2.CommandText =
            "SELECT Predict(Generation) FROM [Generation Trees] " +
            "NATURAL PREDICTION JOIN " +
            "SHAPE { @CaseTable } " +
            " APPEND( { @NestedTable } RELATE CustID TO CustID) " +
            " AS PayChannels " +
            "AS T ";
            DataTable caseTable = new DataTable();
            caseTable.Columns.Add("CustID", typeof(int));
            caseTable.Rows.Add(0);
            DataTable nestedTable = new DataTable();
            nestedTable.Columns.Add("CustID", typeof(int));
            nestedTable.Columns.Add("Channel", typeof(string));
            nestedTable.Rows.Add(0, "Cinemax");
            nestedTable.Rows.Add(0, "Showtime");
            Microsoft.AnalysisServices.AdomdClient.AdomdParameter p3 = new Microsoft.AnalysisServices.AdomdClient.AdomdParameter();
            p3.ParameterName = "CaseTable";
            p3.Value = caseTable;
            cmd.Parameters.Add(p3);
            Microsoft.AnalysisServices.AdomdClient.AdomdParameter p4 = new Microsoft.AnalysisServices.AdomdClient.AdomdParameter();

            p4.ParameterName = "NestedTable";
            p4.Value = nestedTable;
            cmd.Parameters.Add(p4);
            // execute the command and display the prediction result
            Microsoft.AnalysisServices.AdomdClient.AdomdDataReader reader2 = cmd.ExecuteReader();
            if (reader2.Read())
            {
                string predictedGeneration = reader2.GetValue(0).ToString();
                Console.WriteLine(predictedGeneration);
            }
            reader2.Close();
        }

        // Identify all the attributes that split on a specified attribute
        public void FindSplits(string ModelID, string AttributeName)
        {
            // Access the model and throw an exception if not found
            // The error text will be propagated to the client
            //MiningModel model = objConn.MiningModels[ModelID];
            //if (model == null)
            //{
            //    throw new System.Exception("Model not found");
            //}
            //// Look for the attribute in all model trees
            //foreach (MiningContentNode node in model.Content[0].Children)
            //{
            //    if (node.Type == MiningNodeType.Tree)
            //    {
            //        FindSplits(node, AttributeName);
            //    }
            //}
        }

        // Recursively search for the attribute among content nodes
        // return when children are exhausted or attribute is found
        private void FindSplits(Microsoft.AnalysisServices.AdomdClient.MiningContentNode node, string AttributeName)
        {
            // Check for the attribute in the MarginalRule
            // and add row to the table if found
            if (node.MarginalRule.Contains(AttributeName))
            {
                Console.WriteLine(node.Attribute.Name);
                return;
            }
            // recurse over child nodes
            //foreach (MiningContentNode childNode in node.Children)
            //{
            //    FindSplits(childNode, AttributeName);
            //}
        }

        //public string GetPredictionReason(string ModelID, string NodeID, Microsoft.AnalysisServices.AdomdClient.AdomdConnection objConn)
        //{
        //    // return the node description
        //    if (connection.MiningModels[ModelID] == null)
        //        throw new Exception("Model not found");
        //    MiningContentNode node = objConn.MiningModels[ModelID].GetNodeFromUniqueName(NodeID);
        //    if (node == null)
        //        throw new Exception("Node not found");
        //    return node.Description;
        //}
    }
}
