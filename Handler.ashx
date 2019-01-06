<%@ WebHandler Language="C#" Class="Handler" %>

using Newtonsoft.Json;
using System;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Configuration;

public class Handler : IHttpHandler
{
    public void ProcessRequest (HttpContext context)
    {
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.ContentType = "text/plain";

        if (string.IsNullOrEmpty(context.Request.QueryString["method"]))
        {
            context.Response.Write("[]");
            return;
        }

        string method = context.Request.QueryString["method"].ToString();

        if (method.ToUpper() == "GetCategory".ToUpper())  // 返回筛选类别  ok
        {
            GetCategory(context);
        }
        else if (method.ToUpper() == "GetTrend".ToUpper()) // 根据鲜花类型、鲜花级别、鲜花子类、开始时间、结束时间查询报价  ok
        {
            GetTrend(context);
        }
        else if (method.ToUpper() == "GetFlowerPrice".ToUpper())  //ok  只接受id
        {
            GetFlowerPrice(context);
        }
        else if (method.ToUpper() == "GetRemarks".ToUpper())  // 获取备注信息
        {
            GetRemarks(context);
        }
        else if(method.ToUpper() == "GetUrl".ToUpper())
        {
            string uri = context.Request.Url.Authority;
            context.Response.Write(uri);
        }
        else
        {
            context.Response.Write("{'error':1}");
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void GetRemarks (HttpContext context)  // 获取备注信息
    {
        string sql = "select * from remarks";
        DataSet ds = DBHelperAccess.GetList(sql);
        context.Response.Write(DataTableToJsonWithJavaScriptSerializer(ds.Tables[0]));
    }

    public void GetFlowerPrice (HttpContext context)
    {
        string where = " 1=1 ";
        if (!string.IsNullOrEmpty(context.Request.QueryString["id"]))  //每日报价id
        {
            where += " and priceInfoId=" + context.Request.QueryString["id"].ToString();
        }
        if (!string.IsNullOrEmpty(context.Request.QueryString["lb"]))  //类别
        {
            where += " and f.lx=" + context.Request.QueryString["lb"].ToString();
        }

        string host = HttpContext.Current.Request.Url.Host + "/img/";
        // string path = ConfigurationManager.AppSettings["url"].ToString() + "/img/";
        string path = "http://" + context.Request.Url.Authority + "/img/";

        string sql = "select p.id,f.lx,t.lxmc as jb,f.zl,f.mc,'" + path + "'+f.tp as tp,f.gg,p.cd,p.bz,p.jg as price,p.trend from (price p inner join flower f on f.id=p.flowerid) inner join type t on f.zl=t.id where " + where+" order by f.lx,f.zl,f.id";
        DataSet ds = DBHelperAccess.GetList(sql);

        if (ds == null || ds.Tables.Count == 0)
        {
            context.Response.Write("[]");
        }
        else
        {
            DataTable dt = ds.Tables[0];
            context.Response.Write(DataTableToJsonWithJavaScriptSerializer(dt));
        }
    }

    /// <summary>
    /// 获得鲜花分类
    /// </summary>
    /// <param name="context"></param>
    public void GetCategory (HttpContext context)
    {
        string id = context.Request.QueryString["id"];
        if (id == null || id=="") // 如果没有参数返回3个类
        {
            id = "0";
        }

        string sql = "select id,lxmc,lx,zl from type where lx ='" + id+"'";

        DataSet ds = DBHelperAccess.GetList(sql);

        if (ds == null || ds.Tables.Count == 0)
        {
            context.Response.Write("[]");
        }
        else
        {
            DataTable dt = ds.Tables[0];
            context.Response.Write(DataTableToJsonWithJavaScriptSerializer(dt));
        }
    }

    ///// <summary>
    ///// 按条件返回报价
    ///// </summary>
    ///// <param name="context"></param>
    //public void GetCategoryFlower (HttpContext context)
    //{
    //    string where = " 1=1 ";
    //    if (!string.IsNullOrEmpty(context.Request.QueryString["lx"]))  // 类型
    //    {
    //        where += " and lx=" + context.Request.QueryString["lx"].ToString();
    //    }
    //    if (!string.IsNullOrEmpty(context.Request.QueryString["jb"]))  // 级别
    //    {
    //        where += " and jb=" + context.Request.QueryString["jb"].ToString();
    //    }
    //    if (!string.IsNullOrEmpty(context.Request.QueryString["zl"]))  //子类
    //    {
    //        where += " and zl=" + context.Request.QueryString["zl"].ToString();
    //    }

    //    string sql = "select * from flower where " + where;
    //    DataSet ds = DBHelperAccess.GetList(sql);

    //    if (ds == null || ds.Tables.Count == 0)
    //    {
    //        context.Response.Write("[]");
    //    }
    //    else
    //    {
    //        DataTable dt = ds.Tables[0];
    //        context.Response.Write(DataTableToJsonWithJavaScriptSerializer(dt));
    //    }
    //}


    ///// <summary>
    ///// 根据日期查询报价信息
    ///// </summary>
    ///// <param name="context"></param>
    //public void GetOneDayPrice (HttpContext context)
    //{
    //    string where = " 1=1 ";
    //    if (!string.IsNullOrEmpty(context.Request.QueryString["bjrq"]))  //报价日期
    //    {
    //        where += " and bjrq= #" + context.Request.QueryString["bjrq"].ToString() + "#";
    //    }

    //    string sql = "select bjmc,bjrq from price where " + where;
    //    DataSet ds = DBHelperAccess.GetList(sql);

    //    if (ds == null || ds.Tables.Count == 0)
    //    {
    //        context.Response.Write("[]");
    //    }
    //    else
    //    {
    //        DataTable dt = ds.Tables[0];
    //        context.Response.Write(DataTableToJsonWithJavaScriptSerializer(dt));
    //    }
    //}

    ///// <summary>
    ///// 查询所有报价列表
    ///// </summary>
    ///// <param name="context"></param>
    //public void GetPrice (HttpContext context)
    //{
    //    string sql = "select distinct bjmc,bjrq from price";
    //    DataSet ds = DBHelperAccess.GetList(sql);

    //    if (ds == null || ds.Tables.Count == 0)
    //    {
    //        context.Response.Write("[]");
    //    }
    //    else
    //    {
    //        DataTable dt = ds.Tables[0];
    //        context.Response.Write(DataTableToJsonWithJavaScriptSerializer(dt));
    //    }
    //}

    public void GetTrend (HttpContext context)
    {
        List<Model.GetTrendModel> models = new List<Model.GetTrendModel>();

        string priceInfoWhere = " 1=1 ";

        if (!string.IsNullOrEmpty(context.Request.QueryString["startDate"]))  // 开始日期
        {
            priceInfoWhere += " and cdate(bjDate)>= #" + context.Request.QueryString["startDate"].ToString() + "# ";
        }
        if (!string.IsNullOrEmpty(context.Request.QueryString["endDate"]))  // 结束日期
        {
            priceInfoWhere += " and cdate(bjDate)<= #" + context.Request.QueryString["endDate"].ToString() + "# ";
        }
        if (!string.IsNullOrEmpty(context.Request.QueryString["lx"]))  // 类型
        {
            priceInfoWhere += " and lxName='" + context.Request.QueryString["lx"].ToString() + "' ";
        }

        string pageSize = "100000";
        if (!string.IsNullOrEmpty(context.Request.QueryString["pagesize"]))  // 查出条数
        {
            pageSize = context.Request.QueryString["pagesize"].ToString();
        }

        string priceInfoSql = "select top " + pageSize + " * from priceInfo where" + priceInfoWhere + " order by cdate(bjDate) desc ";
        DataSet ds = DBHelperAccess.GetList(priceInfoSql);

        if (ds == null || ds.Tables.Count == 0)
        {
            context.Response.Write("[]");
        }
        else // 查询子数据
        {
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                Model.GetTrendModel model = new Model.GetTrendModel();

                string priceWhere = " p.priceInfoId = " + dr["id"];
                if (!string.IsNullOrEmpty(context.Request.QueryString["lx"]))  // 类型
                {
                    priceWhere += " and f.lx=" + context.Request.QueryString["lx"].ToString();
                }
                if (!string.IsNullOrEmpty(context.Request.QueryString["jb"]))  // 级别
                {
                    priceWhere += " and f.jb=" + context.Request.QueryString["jb"].ToString();
                }
                if (!string.IsNullOrEmpty(context.Request.QueryString["zl"]))  //子类
                {
                    priceWhere += " and f.zl=" + context.Request.QueryString["zl"].ToString();
                }
                if (!string.IsNullOrEmpty(context.Request.QueryString["isRecommend"]))  // 是否推荐
                {
                    priceWhere += " and f.tj=" + context.Request.QueryString["isRecommend"].ToString();
                }

                string priceSql = "select f.mc,f.id,p.jg as price,t.lxmc as jb,p.cd,p.bz from (flower f inner join type t on t.id=f.zl) inner join price p on f.id=p.flowerid where " + priceWhere+" order by f.lx, f.zl,f.id";
                DataSet priceDs = DBHelperAccess.GetList(priceSql);

                model.title = dr["bjName"].ToString();  // 报价名
                model.date = dr["bjdate"].ToString();   // 报价日期
                model.id = dr["id"].ToString();

                if (priceDs == null || priceDs.Tables.Count == 0)
                {
                    model.list = null;
                }
                else
                {
                    model.list = priceDs.Tables[0];
                }

                models.Add(model);
            }
            context.Response.Write(JsonConvert.SerializeObject(models));
        }
    }

    /// <summary>
    /// datatable转json
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public string DataTableToJsonWithJavaScriptSerializer (DataTable table)
    {
        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
        Dictionary<string, object> childRow;
        foreach (DataRow row in table.Rows)
        {
            childRow = new Dictionary<string, object>();
            foreach (DataColumn col in table.Columns)
            {
                childRow.Add(col.ColumnName, row[col]);
            }
            parentRow.Add(childRow);
        }
        return jsSerializer.Serialize(parentRow);
    }
}