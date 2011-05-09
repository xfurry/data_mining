using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WebApplication_OLAP.pages
{
    public partial class SqlDataMining : System.Web.UI.Page
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
    }
}
