using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class addFlower : System.Web.UI.Page
{
    /// <summary>
    /// 绑定下拉列表
    /// </summary>
    private void BindDrp1 ()
    {
        DataSet ds = DBHelperAccess.GetList("select * from type where lx='0'");
        drp1.DataValueField = "id";
        drp1.DataTextField = "lxmc";
        drp1.DataSource = ds.Tables[0];
        drp1.DataBind();
    }
    private void BindDrp2 ()
    {
        DataSet ds = DBHelperAccess.GetList("select * from type where lx='"+drp1.SelectedValue+"'");
        drp2.DataValueField = "id";
        drp2.DataTextField = "lxmc";
        drp2.DataSource = ds.Tables[0];
        drp2.DataBind();
    }

    protected void Page_Load (object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDrp1();
            BindDrp2();

            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                DataSet dataSet = DBHelperAccess.GetList("select top 1 * from flower where id=" + id);
                drp1.SelectedValue = dataSet.Tables[0].Rows[0]["lx"].ToString();
                BindDrp2();
                drp2.SelectedValue = dataSet.Tables[0].Rows[0]["jb"].ToString();
                txt_mc.Value = dataSet.Tables[0].Rows[0]["mc"].ToString();
                txt_gg.Value = dataSet.Tables[0].Rows[0]["gg"].ToString();

                ck_tj.Checked = Convert.ToBoolean(dataSet.Tables[0].Rows[0]["tj"] ?? false);
                //BindData(id.ToString());
            }
        }
    }

    protected void btn_save_Click (object sender, EventArgs e)
    {
        string lx = drp1.SelectedValue;
        string zl = drp2.SelectedValue;

        string mc = txt_mc.Value.Trim();
        string gg = txt_gg.Value.Trim();
        string tp = img_tp.PostedFile.FileName;   // 图片
        bool tj = ck_tj.Checked;

        if (mc == "")
        {
            Response.Write("<script>alert('鲜花名称不能为空！')</script>");
            return;
        }
        else if (gg == "")
        {
            Response.Write("<script>alert('鲜花规格不能为空！')</script>");
            return;
        }

        if (UpImg() == 0)
        {
            return;
        }

        string str = "";
        string tip = "";
        string id = Request.QueryString["id"];

        int i = 0;
        if (!string.IsNullOrEmpty(id)) // 有id，是修改数据操作，没id，是新增操作
        {
            // 验证重名
            int haveFlower = Convert.ToInt32(DBHelperAccess.GetSingle("select count(1) from flower where mc='" + mc + "' and id<>" + id));
            if (haveFlower > 0)
            {
                Response.Write("<script>alert('该鲜花名已存在，请跟换花名！')</script>");
                return;
            }

            tip = "修改";
            if (string.IsNullOrEmpty(img_tp.PostedFile.FileName))
            {
                str = "update flower set lx=" + lx + ",zl=" + zl + ",mc='" + mc + "',gg='" + gg + "',tj=" + ck_tj.Checked + " where id=" + id.ToString();
            }
            else
            {
                str = "update flower set lx=" + lx + ",zl=" + zl + ",mc='" + mc + "',gg='" + gg + "',tp='" + tp + "',tj=" + ck_tj.Checked + " where id=" + id.ToString();
            }
            i = DBHelperAccess.ExecuteSql(str);
        }
        else
        {
            // 验证重名
            int haveFlower = Convert.ToInt32(DBHelperAccess.GetSingle("select count(1) from flower where mc='" + mc + "'"));
            if (haveFlower > 0)
            {
                Response.Write("<script>alert('该鲜花名已存在，不能添加！')</script>");
                return;
            }

            tip = "添加";
            str = "insert into flower(lx,zl,mc,gg,tp,tj) values(" + lx + "," + zl + ",'" + mc + "','" + gg + "','" + tp + "'," + ck_tj.Checked + ")";
            i = DBHelperAccess.ExecuteSql(str);

            if (i > 0)
            {
                /* 给每日报价添加这个花类 */
                DataSet fidDs = DBHelperAccess.GetList("select max(id) as fid from flower where mc='" + mc + "'");
                if (fidDs != null && fidDs.Tables.Count > 0)
                {
                    string fid = fidDs.Tables[0].Rows[0]["fid"].ToString();
                    DBHelperAccess.ExecuteSql("insert into price(flowerid,priceInfoId) select " + fid + ",id from priceInfo");
                }
            }
        }

        if (i > 0)
        {
            Response.Write("<script>alert('" + tip + "成功！')</script>");
            Response.Redirect("flowerMange.aspx");
        }
        else
        {
            Response.Write("<script>alert('" + tip + "失败！')</script>");
        }
    }

    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="upload"></param>
    /// <returns></returns>
    private int UpImg ()
    {
        string tp = img_tp.PostedFile.FileName;   // 图片

        if (tp.Trim() == "")
        {
            return 1;
        }
        else
        {
            try
            {
                FileInfo fi = new FileInfo(tp);
                string[] upImgType = new string[] { ".jpg", ".jpeg", ".gif", ".bmp", ".png" };  // 允许上传图片的类型

                if (upImgType.Count(t => t.ToLower() == fi.Extension.ToLower()) < 1)
                {
                    Response.Write("<script>alert('不能上传该格式的图片！')</script>");
                    return 0;
                }

                string savePath = Server.MapPath("~\\img"); //图片保存的文件夹
                img_tp.PostedFile.SaveAs(savePath + "\\" + fi.Name);//保存路径
                //this.Image1.ImageUrl = "~\\excel" + "\\" + name;//显示图片
                return 1;
            }
            catch
            {
                Response.Write("<script>alert('图片上传失败！')</script>");
                return 0;
            }
        }
    }

    protected void drp1_TextChanged (object sender, EventArgs e)
    {
        BindDrp2();
    }
}