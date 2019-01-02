using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditRemarks : System.Web.UI.Page
{
    protected void Page_Load (object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Query();
        }
    }

    private void Query ()
    {
        string id = Request.QueryString["id"];
        string val = DBHelperAccess.GetList("select top 1 id,bz from remarks where id=" + id).Tables[0].Rows[0]["bz"].ToString();
        txt_bz.Value = val;
    }

    protected void btn_save_Click (object sender, EventArgs e)
    {
        string bz = txt_bz.Value;
        string id = Request.QueryString["id"].ToString();

        int i = DBHelperAccess.ExecuteSql("update remarks set bz='" + bz + "' where id=" + id);
        if (i > 0)
        {
            Response.Write("<script>alert('修改成功！')</script>");
            Response.Redirect("remarks.aspx");
        }
        else
        {
            Response.Write("<script>alert('修改失败！')</script>");
        }
    }
}