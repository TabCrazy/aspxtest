using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class addQuotation : System.Web.UI.Page
{
    private void SetMC()
    {
        string dates = Convert.ToDateTime(txt_rq.Value).ToString("yyyy-MM-dd");
        string drpText = drp1.SelectedItem.Text;
        string str = "陈砦花卉" + dates + drpText + "报价";
        txt_mc.Value = str;
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
            BindDrplb();
            txt_rq.Value = DateTime.Now.ToString("yyyy-MM-dd");
            string date = txt_rq.Value;
        }
        SetMC();
        if (!IsPostBack)
        {
            Query();
        }
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))  // 编辑的时候不允许修改下拉列表
        {
            drp1.Enabled = false;
            this.txt_rq.Attributes.Add("ReadOnly", "true");
        }

    }

    private void BindDrplb ()
    {
        DataSet ds = DBHelperAccess.GetList("select id,lxmc from type where lx='0'");
        if (ds != null && ds.Tables.Count > 0)
        {
            drp1.DataValueField = "id";
            drp1.DataTextField = "lxmc";
            drp1.DataSource = ds.Tables[0];
            drp1.DataBind();
        }
    }

    private void Query ()
    {
        string where = " f.lx=" + drp1.SelectedValue + " ";
        string sql = "";
        DataSet ds = new DataSet();
        if (string.IsNullOrEmpty(Request.QueryString["id"]))  // 新增,自动获取上一天的数据
        {
            // 获取上一天的日期
            string maxDateSql = "select top 1 * from priceInfo f where lxName='" + drp1.SelectedValue + "' and cdate(bjdate)<#"+ txt_rq.Value + "# order by cdate(bjdate) desc ";
            DataSet maxds = DBHelperAccess.GetList(maxDateSql);
            if (maxds == null || maxds.Tables.Count == 0 || maxds.Tables[0].Rows.Count == 0)// 如果不存在上一天数据
            {
                sql = "select t.lxmc, f.id as fid,0 as id, f.mc,0 as jg,'' as cd,'' as bz,0 as dj,1 as trend from flower f inner join type t on t.id=f.zl where " + where+" order by f.zl,f.id ";
            }
            else
            {
                string id = maxds.Tables[0].Rows[0]["id"].ToString();
                sql = "select t.lxmc, f.id as fid,0 as id, f.mc,p.jg as jg,p.cd as cd,p.bz as bz,dj,trend " +
                    "from (flower f inner join price p on f.id=p.flowerid) " +
                    "inner join type t on t.id=f.zl " +
                    "where " + where+ " and priceInfoId=" + id+ " order by f.zl,f.id ";
            }
            ds = DBHelperAccess.GetList(sql);
        }
        else                                                    // 修改
        {
            string id = Request.QueryString["id"].ToString();
            DataSet princeInfoDs = DBHelperAccess.GetList("select * from priceInfo where id=" + id);

            if (princeInfoDs != null && princeInfoDs.Tables.Count > 0)
            {
                DataTable priceInfoDt = princeInfoDs.Tables[0];
                txt_mc.Value = priceInfoDt.Rows[0]["bjName"].ToString();
                txt_rq.Value = priceInfoDt.Rows[0]["bjDate"].ToString();
                drp1.SelectedValue = priceInfoDt.Rows[0]["lxName"].ToString();
            }

            sql = "select t.lxmc, f.id as fid, p.id,f.mc,p.jg as jg,p.cd,p.bz,p.dj,trend " +
                "from (flower f inner join type t on t.id=f.zl) " +
                "left join (select * from price where priceInfoId=" + id + ") p on p.flowerId= f.id " +
                "where f.lx=" + drp1.SelectedValue + " order by f.zl,f.id ";
            ds = DBHelperAccess.GetList(sql);
        }
        Repeater1.DataSource = ds;
        Repeater1.DataBind();
    }

    protected void btn_query_Click (object sender, EventArgs e)
    {
        SetMC();
    }

    private bool CheckData ()
    {
        if (txt_mc.Value.Trim() == "")
        {
            Response.Write("<script>alert('报价名称不能为空！')</script>");
            return false;
        }
        if (txt_rq.Value.Trim() == "")
        {
            Response.Write("<script>alert('报价日期不能为空！')</script>");
            return false;
        }

        string sql = "select count(1) from priceInfo where cdate( bjDate)=#" + txt_rq.Value + "# and lxName='" + drp1.SelectedValue+"'";
        object obj = DBHelperAccess.GetSingle(sql);
        if (Convert.ToInt32(obj) > 0 && string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            Response.Write("<script>alert('该日期下已存在该花类报价,请跟换日期或花类！')</script>");
            return false;
        }
        return true;
    }

    protected void btn_save_Click (object sender, EventArgs e)
    {
        if (CheckData())
        {
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                Add();
            }
            else
            {
                Update();
            }
        }
    }

    private void Add ()
    {
        string insertList = "";

        string bjname = txt_mc.Value;  // 名称
        string bjrq = txt_rq.Value;    // 日期

        DBHelperAccess.ExecuteSql("insert into priceInfo(bjName,bjdate,lxName) values('" + bjname + "','" + bjrq + "'," + drp1.SelectedValue + ")");

        DataSet princeInfoDs = DBHelperAccess.GetList("select top 1 id from priceInfo where bjName='" + bjname + "' and bjdate='" + bjrq + "'");

        if (princeInfoDs == null || princeInfoDs.Tables.Count == 0)
        {
            throw new Exception("错误，请从新打开网站");
        }

        string priceInfoId = princeInfoDs.Tables[0].Rows[0]["id"].ToString();

        foreach (RepeaterItem ri in this.Repeater1.Items)
        {
            if (ri.ItemType != ListItemType.Header && ri.ItemType != ListItemType.Footer)
            {
                HiddenField hiddenfId = ri.FindControl("hd_fid") as HiddenField;
                TextBox textBox = ri.FindControl("txt_jg") as TextBox;
                TextBox txtCd = ri.FindControl("txt_cd") as TextBox;
                TextBox txtBz = ri.FindControl("txt_bz") as TextBox;
                DropDownList drpdj = ri.FindControl("drp_dj") as DropDownList;
                DropDownList drop = ri.FindControl("drp_qs") as DropDownList;

                string fid = hiddenfId.Value;
                string cd = txtCd.Text;
                string bz = txtBz.Text;

                string jg = textBox.Text == "" ? "0" : textBox.Text.Trim();
                string dj = drpdj.SelectedValue;
                string trend = drop.SelectedValue;

                insertList = "insert into price(flowerId,jg,cd,bz,dj,priceinfoId,trend) " +
                    "values(" + fid + ",'" + jg + "','"+cd+"','"+bz+"','"+ dj + "','" + priceInfoId + "','" + trend + "'); ";
                int i = DBHelperAccess.ExecuteSql(insertList);
            }
        }
        Response.Write("<script>alert('添加完成！')</script>");
        Response.Redirect("index.aspx");
    }

    private void Update ()
    {
        string id = Request.QueryString["id"].ToString();

        string bjname = txt_mc.Value;  // 名称
        string bjrq = txt_rq.Value;    // 日期

        DBHelperAccess.ExecuteSql("update priceInfo set bjName='" + bjname + "' where id=" + id);

        foreach (RepeaterItem ri in Repeater1.Items)
        {
            string updateList = "";

            HiddenField hidden = ri.FindControl("hd_id") as HiddenField;
             HiddenField fidHidden = ri.FindControl("hd_fid") as HiddenField;
            TextBox txtJg = ri.FindControl("txt_jg") as TextBox;
            TextBox txtCd = ri.FindControl("txt_cd") as TextBox;
            TextBox txtBz = ri.FindControl("txt_bz") as TextBox;
            DropDownList drpdj = ri.FindControl("drp_dj") as DropDownList;
            DropDownList drpQs = ri.FindControl("drp_qs") as DropDownList;

            string jg = txtJg.Text == "" ? "0" : txtJg.Text;
            string cd = txtCd.Text;
            string bz = txtBz.Text;
            string dj = drpdj.SelectedValue;
            string qs = drpQs.SelectedValue;
            string hiddenValue = hidden.Value;
            string fid = fidHidden.Value;

            if (!string.IsNullOrEmpty(hiddenValue))
            {
                updateList = "update price set jg='" + jg + "',cd='"+cd+"',bz='"+bz+"',dj='"+dj+"',trend='" + qs + "' where id= " + hiddenValue + "";
            }
            else
            {// 没有id，说明是新数据，则新增
                updateList = "insert into price(flowerId,jg,cd,bz,dj,priceinfoId,trend) " +
                    "values(" + fid + ",'" + jg + "','"+cd+"','"+bz+"','"+dj+"','" + id + "','" + qs + "'); ";
            }

            int i = DBHelperAccess.ExecuteSql(updateList);
        }

        Response.Write("<script>alert('修改完成！')</script>");
        Response.Redirect("index.aspx");
    }

    /// <summary>
    /// 获取级别
    /// </summary>
    /// <returns></returns>
    private DataTable GetLevel ()
    {
        string str = "select id,lxmc from type where zl='2' and lx='"+drp1.SelectedValue+"'";
        DataSet ds = DBHelperAccess.GetList(str);
        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            DataRow dr = dt.NewRow();
            dr["id"] = "0";
            dr["lxmc"] = "无";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("lxmc");

            DataRow dr = dt.NewRow();
            dr["id"] = "0";
            dr["lxmc"] = "无";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
    }

    protected void Repeater1_ItemDataBound (object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView rowv = (DataRowView)e.Item.DataItem;
            DropDownList drpdj = (DropDownList)e.Item.FindControl("drp_dj");
            drpdj.DataSource = GetLevel();
            drpdj.DataValueField = "id";
            drpdj.DataTextField = "lxmc";
            drpdj.DataBind() ;

            DropDownList DListNewsIsShow = (DropDownList)e.Item.FindControl("drp_qs");
            string djvale= rowv["dj"].ToString();
            string selectedValue = rowv["trend"].ToString();
            if (string.IsNullOrEmpty(selectedValue))
            {
                selectedValue = "2";
            }
            DListNewsIsShow.Items.FindByValue(selectedValue).Selected = true;
            if (string.IsNullOrEmpty(djvale))
            {
                djvale = "0";
            }
            drpdj.Items.FindByValue(djvale).Selected = true;
        }
    }

    protected void drp1_TextChanged (object sender, EventArgs e)
    {
        Query();
    }
}