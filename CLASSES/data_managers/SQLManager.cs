using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication_OLAP
{
    public class SQLManager
    {
        private string sCatalog = "AdventureWorksDW";
        private const string sServer = "CLARITY-7HYGMQM\\ANA";
        //private const string sServer = "localhost";

        private SqlConnection objSqlConnection = null;

        public SQLManager() { }

        // override constructor to set current database
        public SQLManager(string sCatalog)
        {
            this.sCatalog = sCatalog;
        }

        // start sql connection
        private void InitConnection()
        {
            System.Data.SqlClient.SqlConnection conn =
                new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString =
             "integrated security=SSPI;data source=" + sServer + ";persist security info=False;initial catalog=" + sCatalog;

            this.objSqlConnection = conn;
        }

        // Close connection
        public void CloseConnection()
        {
            if (this.objSqlConnection != null)
                objSqlConnection.Close();
        }

        // Retrieve a data set from the DB
        public DataSet GetQueryDataSet(string sQuery)
        {
            if (this.objSqlConnection == null)
                this.InitConnection();

            try
            {
                objSqlConnection.Open();

                SqlCommand command = new SqlCommand(sQuery, objSqlConnection);
                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(command);
                DataSet myDataSet = new DataSet();
                mySqlDataAdapter.Fill(myDataSet);

                return myDataSet;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        // get a query result
        public SqlDataReader GetQueryResult(string sQuery)
        {
            if (this.objSqlConnection == null)
                this.InitConnection();

            try
            {
                objSqlConnection.Open();

                SqlCommand command = new SqlCommand(sQuery, objSqlConnection);
                SqlDataReader dataReader = command.ExecuteReader();
                return dataReader;

                // Keep for further use
                /*command.ExecuteReader();
                while (r.Read())
                {
                    string username = (string)r["username"];
                    int userID = (int)r["userid"];
                    Debug.WriteLine(username + "(" + userID + ")");
                }
                r.Close();*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }
    }
}
