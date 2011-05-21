using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
                GridViewMain.DataSource = objTable;
                GridViewMain.DataBind();
            }
        }

        protected void ButtonStructure_Click(object sender, EventArgs e)
        {
            MiningManager mining = new MiningManager();
            mining.AddMiningStructure();
            LabelStatus.Text = mining.SResult;
        }
    }
}
