using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class addType : System.Web.UI.Page
{
    protected void Page_Load (object sender, EventArgs e)
    {
        if (Session["userName"] == null)
        {
            Response.Write("<script>alert('登录超时，请从新登录！')</script>");
            Response.Redirect("login.aspx");
            return;
        }
        if (!IsPostBack)
        {
            InitData();

            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                DataSet ds = DBHelperAccess.GetList("select * from type where id =" + id);
                string lx= ds.Tables[0].Rows[0]["lx"].ToString();
                drp1.SelectedValue = lx;

                if (lx!="0")
                {
                    drp2.Visible = true;
                }
                drp2.SelectedValue = (ds.Tables[0].Rows[0]["zl"]).ToString();
                ipt_lxmc.Value = (ds.Tables[0].Rows[0]["lxmc"]).ToString();
                drp1.Enabled = false;
            }

            drp1_SelectedIndexChanged(null, null);
        }
    }

    protected void InitData ()
    {
        BindDDL();
    }

    private void BindDDL ()
    {
        string sql = "select * from type where lx='0'"; // 查询出所有一级节点
        DataSet ds = DBHelperAccess.GetList(sql);
        if (ds != null && ds.Tables.Count > 0)
        {
            drp1.DataValueField = "id";
            drp1.DataTextField = "lxmc";
            drp1.DataSource = InsertTotal(ds.Tables[0]);
            drp1.DataBind();
        }
    }

    /// <summary>
    /// 添加全部项
    /// </summary>
    /// <returns></returns>
    private DataTable InsertTotal (DataTable dt)
    {
        DataRow dr = dt.NewRow();
        dr["id"] = "0";
        dr["lxmc"] = "鲜花类型";
        dt.Rows.InsertAt(dr, 0);
        return dt;
    }

    /* 添加鲜花 */
    protected void btn_save_Click (object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            Update();
        }
        else
        {
            Add();
        }
    }

    private void Update ()
    {
        if (ipt_lxmc.Value == "")
        {
            Response.Write("<script>alert('请填写类型名称！')</script>");
            return;
        }

        string lxmc = ipt_lxmc.Value;
        string lx = drp1.SelectedValue;
        string zl = GetZL();

        string sql = "update type set lxmc='" + lxmc + "',lx='" + lx + "',zl='" + zl + "' where id =" + Request.QueryString["id"];

        object include = DBHelperAccess.GetSingle("select * from type where lxmc='" + lxmc + "' and lx='" + lx + "' and zl='" + zl + "' and id<>" + Request.QueryString["id"]);
        if (include != null)
        {
            Response.Write("<script>alert('类别已存在，不能添加！')</script>");
            return;
        }

        int i = DBHelperAccess.ExecuteSql(sql);

        if (i > 0)
        {
            Response.Redirect("typeList.aspx");
        }
        else
        {
            Response.Write("<script>alert('修改失败！')</script>");
        }
    }

    private void Add ()
    {
        if (ipt_lxmc.Value == "")
        {
            Response.Write("<script>alert('请填写类型名称！')</script>");
            return;
        }

        string lxmc = ipt_lxmc.Value;
        string lx = drp1.SelectedValue;
        string zl = GetZL();

        string sql = "insert into type(lxmc,lx,zl) values('" + ipt_lxmc.Value + "','" + lx + "','" + zl + "')";

        object include = DBHelperAccess.GetSingle("select * from type where lxmc='" + lxmc + "' and lx='" + lx + "' and zl='" + zl + "'");
        if (include != null)
        {
            Response.Write("<script>alert('类别已存在，不能添加！')</script>");
            return;
        }

        int i = DBHelperAccess.ExecuteSql(sql);
        if (i > 0)
        {
            Response.Write("<script>alert('添加成功')</script>");
            Response.Redirect("typeList.aspx");
        }
        else
        {
            Response.Write("<script>alert('添加失败')</script>");
        }
    }

    /// <summary>
    /// 获取系列字段
    /// </summary>
    /// <returns></returns>
    private string GetZL ()
    {
        if (drp1.SelectedValue == "0")
        {
            return "0";
        }
        else
        {
            return drp2.SelectedValue;
        }
    }

    protected void drp1_SelectedIndexChanged (object sender, EventArgs e)
    {
        string drp1value = drp1.SelectedValue;

        if(drp1value=="0")
        {
            leveSelect.Visible = false;
            drp2.Visible = false;
        }
        else
        {
            leveSelect.Visible = true;
            drp2.Visible = true;
        }
    }
}