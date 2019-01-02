using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    protected void Page_Load (object sender, EventArgs e)
    {

    }

    protected void btn_save_Click (object sender, EventArgs e)
    {
        if (txt_userName.Value.Trim() == "")
        {
            Response.Write("<script>alert('用户不能为空！')</script>");
            return;
        }
        if (txt_password.Value.Trim() == "")
        {
            Response.Write("<script>alert('密码不能为空！')</script>");
            return;
        }
        string userName = txt_userName.Value;
        string password = txt_password.Value;
        object obj = DBHelperAccess.GetSingle("select * from userinfo where uname='" + userName.Trim() + "' and password='" + password + "'");
        if (obj != null)
        {
            Session["userName"] = userName;
            Response.Redirect("index.aspx");
        }
        else
        {
            Response.Write("<script>alert('登录失败,账号名或密码错误！')</script>");
        }
    }
}