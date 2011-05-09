using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.AnalysisServices;
using System.Data.OleDb;

namespace WebApplication_OLAP.classes.data_managers
{
    public class SQLMiningManager
    {
        private const string sCatalog = "Adventure Works DW 2008";
        private const string sServer = "CLARITY-7HYGMQM\\ANA";

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
                Database currentDB = GetCurrentDatabase();
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
         * Returns an existing DB
         */
        private Database GetCurrentDatabase()
        {
            try
            {
                Server srv = new Server();
                srv.Connect("integrated security=SSPI;data source=" + sServer + ";persist security info=False;initial catalog=" + sCatalog);
                Database myDB = srv.Databases["Adventure Works DW 2008"];

                return myDB;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        /*
         * Drop an existing mining structure; used in order to avoid the crash if the user wants to add a structure with the same name
         */
        void DropExistingStructures(Database objDataBase, string sName)
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
        void DropExistingMiningModels(MiningStructure objStruct, string sName)
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
         * Create mining structure for selected database
         */
        MiningStructure CreateMiningStructure(Database objDatabase)
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
            PayChannels.ForeignKeyColumns.Add("Customer", "Phone", System.Data.OleDb.OleDbType.Integer);

            ScalarMiningStructureColumn Channel = new ScalarMiningStructureColumn("Channel", "Channel");
            Channel.Type = MiningStructureColumnTypes.Text;
            Channel.Content = MiningStructureColumnContents.Key;
            Channel.IsKey = true;
            // Add data binding to the column
            Channel.KeyColumns.Add("Customer", "FirstName", System.Data.OleDb.OleDbType.WChar);
            PayChannels.Columns.Add(Channel);
            currentMiningStruct.Columns.Add(PayChannels);
            */


            // Add the mining structure to the database
            objDatabase.MiningStructures.Add(currentMiningStruct);
            currentMiningStruct.Update();

            return currentMiningStruct;
        }

        /*
         * Create mining model for the selected mining strucutre
         */
        void CreateModels(MiningStructure objStructure)
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
        void ProcessDatabase(Database objDatabase, Server objServer)
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

        void ProgressReportHandler(object sender, TraceEventArgs e)
        {
            Console.WriteLine(e[TraceColumn.TextData]);
        }

        //public void DiscoverServices()
        //{
        //    AdomdConnection connection = new AdomdConnection(
        //    "Data Source=localhost");
        //    connection.Open();
        //    foreach (MiningService ms in connection.MiningServices)
        //    {
        //        Console.WriteLine("Service: " + ms.Name);
        //        foreach (MiningServiceParameter mp in
        //        ms.AvailableParameters)
        //        {
        //            Console.WriteLine(" Parameter: " + mp.Name +
        //            " Default: " + mp.DefaultValue);
        //        }
        //    }
        //    connection.Close();
        //}
    }
}
