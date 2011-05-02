using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication_OLAP
{
    public partial class SqlReporting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonExecute_Click(object sender, EventArgs e)
        {
            SQLManager manager = new SQLManager();
            manager.GetQueryResult();
        }
    }
}
