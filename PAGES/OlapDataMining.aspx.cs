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

        /*
         * Build mining structure
         */
        protected void ButtonStructure_Click(object sender, EventArgs e)
        {
            MiningManager mining = new MiningManager();
            mining.AddMiningStructure();
            LabelStatus.Text = mining.SResult;
        }

        /*
         * Update key attribute
         */
        protected void DropDownListKey_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /*
         * Update dimension list
         */
        protected void DropDownListDimensions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /*
         * Check / uncheck all fields
         */
        protected void ButtonInput_Click(object sender, EventArgs e)
        {

        }

        /*
         * Check / uncheck all fields
         */
        protected void ButtonPredict_Click(object sender, EventArgs e)
        {

        }

        /*
         * Show mining results
         */
        protected void ButtonResult_Click(object sender, EventArgs e)
        {

        }

        /*
         * Execute custom query
         */
        protected void ButtonExecute_Click(object sender, EventArgs e)
        {

        }
    }
}
