using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class typeList : System.Web.UI.Page
{
    private void Query ()
    {
        string where = " 1=1 ";
        if (query_lb.Value != "0")
        {
            where += " and lx='" + query_lb.Value + "'";
        }
        if (query_zl.Value != "0")
        {
            where += " and zl='" + query_zl.Value + "' ";
        }
        DataSet ds = DBHelperAccess.GetList("select * from type where " + where);
        Repeater1.DataSource = ds;
        Repeater1.DataBind();
    }
    protected void Page_Load (object sender, EventArgs e)
    {
        //if (Session["userName"] == null)
        //{
        //    Response.Write("<script>alert('登录超时，请从新登录！')</script>");
        //    Response.Redirect("login.aspx");
        //    return;
        //}

        if (!IsPostBack)
        {
            InitData();
            InitData2();
            Query();
        }
    }

    private void InitData ()
    {
        string sql = "select * from type where lx='0'";
        DataSet ds = DBHelperAccess.GetList(sql);

        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dt = InsertTotal(ds.Tables[0]);

            query_lb.DataSource = ds.Tables[0];
            query_lb.DataValueField = "id";
            query_lb.DataTextField = "lxmc";
            query_lb.DataBind();
        }
    }

    private void InitData2 ()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("id");
        dt.Columns.Add("lxmc");

        DataRow dr1 = dt.NewRow();
        dr1["id"] = "0";
        dr1["lxmc"] = "全部";
        dt.Rows.Add(dr1);

        if (query_lb.Value != "0")
        {
            DataRow dr2 = dt.NewRow();
            dr2["id"] = "1";
            dr2["lxmc"] = "子类";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["id"] = "2";
            dr3["lxmc"] = "级别";
            dt.Rows.Add(dr3);
        }

        query_zl.DataSource = dt;
        query_zl.DataValueField = "id";
        query_zl.DataTextField = "lxmc";
        query_zl.DataBind();
    }

    /// <summary>
    /// 添加全部项
    /// </summary>
    /// <returns></returns>
    private DataTable InsertTotal (DataTable dt)
    {
        DataRow dr = dt.NewRow();
        dr["id"] = "0";
        dr["lxmc"] = "全部";
        dt.Rows.InsertAt(dr, 0);
        return dt;
    }

    protected void btn_query_Click (object sender, EventArgs e)
    {
        InitData2();
        Query();
    }

    #region Repeater1_ItemCommand事件
    protected void Repeater1_ItemCommand (object source, RepeaterCommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();

        if (e.CommandName == "Edit" && !String.IsNullOrEmpty(id))
        {
            Response.Redirect("addType.aspx?id=" + id);
        }
        else if (e.CommandName == "Delete" && !String.IsNullOrEmpty(id))
        {
            int i = DBHelperAccess.ExecuteSql("delete from type where id=" + id);

            if (i == 0)
            {
                Response.Write("<script>alert('删除失败！')</script>");
            }
            else
            {
                // 删除相关数据
                DBHelperAccess.ExecuteSql("delete from type where lx='" + id+"'");
                Query();
            }
        }
        else
        {
            return;
        }
    }
    #endregion

    protected void btn_query2_Click (object sender, EventArgs e)
    {
        Query();
    }
}