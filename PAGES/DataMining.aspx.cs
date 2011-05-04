using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Clustering;
using System.Data;

namespace WebApplication_OLAP.classes
{
    public partial class DataMining : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // get from session
            if (Session != null)
            {
                DataTable objTable = (DataTable)Session["queryData"];
                GridView1.DataSource = objTable;
                GridView1.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            MiningManager mining = new MiningManager();
            mining.AddMiningStructure();
            Label1.Text = mining.SResult;

            //System.Threading.Thread.Sleep(5000);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //CMeansAlgorithm miner = new CMeansAlgorithm();
            //miner.Run(5);
        }
    }
}
