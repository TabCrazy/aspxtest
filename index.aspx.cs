﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    public class PriceInfoList
    {
        public int Id { get; set; }
        public string bjName { get; set; }
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
            Query();
        }
    }

    protected void btn_query_Click (object sender, EventArgs e)
    {
        Query();
    }

    private void Query ()
    {
        int totalCount = (int)DBHelperAccess.GetList("select count(1) as num from priceInfo ").Tables[0].Rows[0]["num"];   //获得总记录数
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

        string sql = "select * from priceInfo order by cdate(bjDate) desc ";

        DataSet ds = DBHelperAccess.GetList(sql);

        int minRow = pageSize * (curPage - 1);
        int maxRow = pageSize * curPage;

        Repeater1.DataSource = ds.Tables.Count == 0 ? null : GetCurPageDate(ds.Tables[0], minRow, maxRow);
        Repeater1.DataBind();
    }

    private DataTable GetCurPageDate (DataTable dt, int minRow, int maxRow)
    {
        List<PriceInfoList> priceInfos = new List<PriceInfoList>();
        int count = dt.Rows.Count > maxRow ? maxRow : dt.Rows.Count;

        for (int i = minRow; i < count; i++)
        {
            PriceInfoList flowerList = new PriceInfoList() {
                Id = (int)dt.Rows[i]["id"],
                bjName = dt.Rows[i]["bjName"].ToString(),
            };
            priceInfos.Add(flowerList);
        }

        return FunctionModel.ToDataTable(priceInfos);
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
            Response.Redirect("addQuotation.aspx?id=" + id);
        }
        else if (e.CommandName == "Delete" && !String.IsNullOrEmpty(id))
        {
            int i = DBHelperAccess.ExecuteSql("delete from priceInfo where id=" + id);
            i = DBHelperAccess.ExecuteSql("delete from price where priceInfoId=" + id);

            Query();
        }
        else
        {
            return;
        }
    }
    #endregion
}