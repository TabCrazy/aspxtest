using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class flowerMange : System.Web.UI.Page
{
    /// <summary>
    /// 绑定下拉列表
    /// </summary>
    private void BindDrp1 ()
    {
        DataSet ds = DBHelperAccess.GetList("select id,lxmc from type where lx='0'");
        drp1.DataValueField = "id";
        drp1.DataTextField = "lxmc";
        drp1.DataSource = InsertTotal(ds.Tables[0]);
        drp1.DataBind();
    }
    private void BindDrp2 ()
    {
        string value = drp1.SelectedValue;
        if (value != "0")
        {
            DataSet ds = DBHelperAccess.GetList("select id,lxmc from type where lx='" + value + "' and zl='1'");
            drp2.DataValueField = "id";
            drp2.DataTextField = "lxmc";
            drp2.DataSource = InsertTotal(ds.Tables[0]);
            drp2.DataBind();
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
        dr["lxmc"] = "全部";
        dt.Rows.InsertAt(dr, 0);
        return dt;
    }

    private void Query ()
    {
        string where = " 1=1 ";
        if (drp1.SelectedValue != "0")
        {
            where += " and lx=" + drp1.SelectedValue;
        }
        if (drp2.SelectedValue != "0")
        {
            where += " and zl=" + drp2.SelectedValue;
        }

        int totalCount = (int)DBHelperAccess.GetList("select count(1) as num from flower where " + where).Tables[0].Rows[0]["num"];   //获得总记录数
        int pageSize = 10;//显示数据条数
        int curPage = Convert.ToInt32(lblCurrentPage.Text.Trim());//这是页面显示的第几页那个几
        int tmpInt = totalCount / pageSize;
        int pages = totalCount % pageSize == 0 ? tmpInt : (tmpInt + 1);//总页数

        lblCount.Text = totalCount.ToString();
        lblPage.Text = pages.ToString();
        next.Enabled = true;
        up.Enabled = true;
        first.Enabled = true;
        last.Enabled = true;

        if (curPage == 1)
        {
            up.Enabled = false;
            first.Enabled = false;
        }
        if (curPage == pages)
        {
            next.Enabled = false;
            last.Enabled = false;
        }

        string sql = "";

        if (curPage == 1)
        {
            sql = "select top " + pageSize + " * from flower where " + where;
        }
        else
        {
            sql = "select top " + pageSize + " * from flower where " + where + " and id not in(select top " + pageSize * (curPage - 1) + " id from flower where " + where + " order by id) order by id";
        }
        DataSet ds = DBHelperAccess.GetList(sql);

        Repeater1.DataSource = ds;
        Repeater1.DataBind();
    }
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
            lblCurrentPage.Text = "1"; // 初始化页数
            BindDrp1();
            BindDrp2();
            Query();
        }
    }

    protected void btn_query_Click (object sender, EventArgs e)
    {
        lblCurrentPage.Text = "1";
        Query();
    }

    #region 第一页
    protected void first_Click (object sender, EventArgs e)
    {
        lblCurrentPage.Text = "1";
        Query();
    }
    #endregion

    #region 下一页
    protected void next_Click (object sender, EventArgs e)
    {
        int page = int.Parse(lblCurrentPage.Text.Trim());
        lblCurrentPage.Text = (page + 1).ToString();
        Query();
    }
    #endregion

    #region 上一页
    protected void up_Click (object sender, EventArgs e)
    {
        int page = int.Parse(lblCurrentPage.Text.Trim());
        lblCurrentPage.Text = (page - 1).ToString();
        Query();
    }
    #endregion

    #region 最后页
    protected void last_Click (object sender, EventArgs e)
    {
        int page = int.Parse(lblPage.Text.Trim());
        lblCurrentPage.Text = page.ToString();
        Query();
    }
    #endregion

    #region 跳转到第n页
    protected void btnGo_Click (object sender, EventArgs e)
    {
        //转到第N页代码
        if (!Regex.IsMatch(txtPage.Text.Trim(), @"^([1-9]\d*)$"))//验证输入数字是否合法
        {
            txtPage.Text = "1";
        }

        int curPage = int.Parse(txtPage.Text.Trim());
        int allPage = int.Parse(lblPage.Text.Trim());
        if (curPage > allPage)
            curPage = allPage;
        txtPage.Text = lblCurrentPage.Text = curPage.ToString();
        Query();
    }
    #endregion

    #region Repeater1_ItemCommand事件
    protected void Repeater1_ItemCommand (object source, RepeaterCommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();

        if (e.CommandName == "Edit" && !String.IsNullOrEmpty(id))
        {
            Response.Redirect("addFlower.aspx?id=" + id);
        }
        else if (e.CommandName == "Delete" && !String.IsNullOrEmpty(id))
        {
            int i = DBHelperAccess.ExecuteSql("delete from flower where id=" + id);
            DBHelperAccess.ExecuteSql("delete from price where flowerid=" + id);  // 删除相关数据

            if (i == 0)
            {
                Response.Write("<script>alert('删除失败！')</script>");
            }
            else
            {
                Query();
            }
        }
        else
        {
            return;
        }
    }
    #endregion

    protected void drp1_TextChanged (object sender, EventArgs e)
    {
        BindDrp2();
        Query();
    }

    protected void drp2_TextChanged (object sender, EventArgs e)
    {
        Query();
    }
}