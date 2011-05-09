using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WebApplication_OLAP
{
    public class SQLManager
    {
        private const string sCatalog = "AdventureWorks";
        private const string sServer = "CLARITY-7HYGMQM\\ANA";

        private SqlConnection objSqlConnection = null;

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
