using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class remarks : System.Web.UI.Page
{
    protected void Page_Load (object sender, EventArgs e)
    {
        Query();
    }

    private void Query ()
    {
        DataSet ds = DBHelperAccess.GetList("select * from remarks ");
        Repeater1.DataSource = ds;
        Repeater1.DataBind();
    }

    #region Repeater1_ItemCommand事件
    protected void Repeater1_ItemCommand (object source, RepeaterCommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();

        if (e.CommandName == "Edit" && !String.IsNullOrEmpty(id))
        {
            Response.Redirect("EditRemarks.aspx?id=" + id);
        }
        else
        {
            return;
        }
    }
    #endregion
}