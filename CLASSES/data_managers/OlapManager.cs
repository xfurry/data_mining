using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using Microsoft.AnalysisServices.AdomdClient;

namespace WebApplication_OLAP
{
    public class OlapManager
    {
        //private const string sCatalog = "Adventure Works DW 2008";
        private const string sServer = "CLARITY-7HYGMQM\\ANA";
        private const string sCatalog = "MyFinalDataBase";
        //private const string sServer = "localhost";

        private AdomdConnection objConnection = null;
        private AdomdCommand objCommand = null;
        private List<CubeDef> lCubes = new List<CubeDef>();
        private List<string> lDim = new List<string>();

        /*
         * Getters and setters
         */

        public AdomdConnection ObjConnection
        {
            get { return objConnection; }
            set { objConnection = value; }
        }

        public List<CubeDef> LCubes
        {
            get { return lCubes; }
            set { lCubes = value; }
        }

        public List<string> LDim
        {
            get { return lDim; }
            set { lDim = value; }
        }

        public AdomdCommand ObjCommand
        {
            get { return objCommand; }
            set { objCommand = value; }
        }

        /*
         * Methods
         */
        // Init OLAP connection
        private void InitConnection()
        {
            string sConnString = "Data Source=" + sServer + "; Initial Catalog=" + sCatalog;
            this.objConnection = new AdomdConnection(sConnString);
        }

        // Close connection
        public void CloseConnection()
        {
            if (this.objConnection != null)
                objConnection.Close();
        }

        // OleDB connection
        // note: better use ADOMD connection
        private OleDbConnection DBConnection()
        {
            OleDbConnection objConneciton = null;
            objConneciton = new OleDbConnection();
            objConneciton.ConnectionString =
              "Provider=MSOLAP.3; Data Source=CLARITY-7HYGMQM\\ANA;" +"Initial Catalog=Adventure Works DW 2008";
            objConneciton.Open();

            return objConneciton;
        }

        // Execute query and return cell sets
        public CellSet GetQueryResult(string sQuery)
        {
            CellSet objResult = null;

            if (this.objConnection == null)
                this.InitConnection();

            try
            {
                this.objConnection.Open();

                ObjCommand = new AdomdCommand(sQuery, this.objConnection);
                objResult = ObjCommand.ExecuteCellSet();

                return objResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        // get local cubes
        public void GetCubes()
        {
            if (this.objConnection == null)
                this.InitConnection();

            this.objConnection.Open();

            //Loop through every cube
            foreach (CubeDef objCube in objConnection.Cubes)
            {
                //Skip hidden cubes.
                if (objCube.Name.StartsWith("$"))
                    continue;

                //Write the cube name
                LCubes.Add(objCube);
            }
        }

        // get cube dimensions
        public void GetDimension(CubeDef objInputCube)
        {
            //Write out all dimensions, indented by a tab
            foreach (Dimension objDim in objInputCube.Dimensions)
                LDim.Add(objDim.Name);
        }
    }
}
