using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.AnalysisServices;

namespace WebApplication_OLAP.classes.data_managers
{
    public class SQLMiningManager
    {
        private const string sCatalog = "Adventure Works DW 2008";
        private const string sServer = "CLARITY-7HYGMQM\\ANA";

        private SqlConnection objSqlConnection = null;

        public void Initialize()
        {
            // Connect to the Analysis Service server
            Database myDB = GetCurrentDatabase();
            CreateMiningStructure(myDB);
        }

        // start sql connection
        private void InitConnection()
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = "integrated security=SSPI;data source=" + sServer + ";persist security info=False;initial catalog=" + sCatalog;

            this.objSqlConnection = conn;
        }

        Database GetCurrentDatabase()
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
         * Create mining structure for selected database
         */
        MiningStructure CreateMiningStructure(Database db)
        {
            // Initialize a new mining structure
            MiningStructure ms = new MiningStructure("PayChannelAnalysis", "PayChannelAnalysis");
            ms.Source = new DataSourceViewBinding("Adventure Works DW");

            // Create the columns of the mining structure
            // setting the type, content and data binding
            // User Id column
            ScalarMiningStructureColumn UserID = new ScalarMiningStructureColumn("UserId", "UserId");
            UserID.Type = MiningStructureColumnTypes.Long;
            UserID.Content = MiningStructureColumnContents.Key;
            UserID.IsKey = true;

            // Add data binding to the column
            UserID.KeyColumns.Add("Customer", "CustomerKey", System.Data.OleDb.OleDbType.Integer);

            // Add the column to the mining structure
            ms.Columns.Add(UserID);

            // Generation column
            ScalarMiningStructureColumn Generation = new ScalarMiningStructureColumn("Generation", "Generation");
            Generation.Type = MiningStructureColumnTypes.Text;
            Generation.Content = MiningStructureColumnContents.Discrete;

            // Add data binding to the column
            Generation.KeyColumns.Add("Customers", "Generation", System.Data.OleDb.OleDbType.WChar);

            // Add the column to the mining structure
            ms.Columns.Add(Generation);

            // Add Nested table by creating a table column and adding
            // a key column to the nested table
            TableMiningStructureColumn PayChannels = new TableMiningStructureColumn("PayChannels", "PayChannels");
            PayChannels.ForeignKeyColumns.Add("PayChannels", "SurveyTakenID", System.Data.OleDb.OleDbType.Integer);
            ScalarMiningStructureColumn Channel = new ScalarMiningStructureColumn("Channel", "Channel");
            Channel.Type = MiningStructureColumnTypes.Text;
            Channel.Content = MiningStructureColumnContents.Key;
            Channel.IsKey = true;

            // Add data binding to the column
            Channel.KeyColumns.Add("PayChannels", "Channel", System.Data.OleDb.OleDbType.WChar);
            PayChannels.Columns.Add(Channel);
            ms.Columns.Add(PayChannels);

            // Add the mining structure to the database
            db.MiningStructures.Add(ms);
            ms.Update();
            return ms;
        }
    }
}
