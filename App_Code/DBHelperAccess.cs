using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

public class DBHelperAccess
{
    public DBHelperAccess ()
    {
    }

    public static string strAccess = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source =" + AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["conn"].ToString() + ";Persist Security Info=False;";
    private static OleDbConnection conn;
    private static OleDbDataAdapter oda = new OleDbDataAdapter();
    private OleDbCommand cmd;

    /// <summary>
    /// 执行查询语句，返回DataSet
    /// </summary>
    /// <param name="SQLString">查询语句</param>
    /// <returns>DataSet</returns>
    public static DataSet Query (string SQLString)
    {
        DataSet ds = new DataSet();
        conn = new OleDbConnection(strAccess);
        try
        {
            conn.Open();
            oda = new OleDbDataAdapter(SQLString, conn);
            oda.Fill(ds, "ds");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            conn.Close();
        }
        return ds;
    }
    /// <summary>
    /// 执行查询语句，返回DataSet
    /// </summary>
    /// <param name="SQLString">查询语句</param>
    /// <returns>DataSet</returns>
    public static DataSet GetList (string SQLString)
    {
        return Query(SQLString);
    }
    /// <summary>
    /// 执行查询语句，返回DataSet
    /// </summary>
    /// <param name="tabcolumn">列名</param>
    /// <param name="tabname">表名</param>
    /// <param name="SQLWhere">查询条件</param>
    /// <returns></returns>
    public static DataSet GetList (string tabcolumn, string tabname, string SQLWhere)
    {
        if (string.IsNullOrEmpty(SQLWhere))
        {
            SQLWhere = "1=1";
        }
        string str = "select " + tabcolumn + " from " + tabname + " where " + SQLWhere;
        return Query(str);
    }
    /// <summary>
    /// 执行SQL语句，返回影响的记录数
    /// </summary>
    /// <param name="SQLString">SQL语句</param>
    /// <returns>影响的记录数</returns>
    public static int ExecuteSql (string SQLString)
    {
        conn = new OleDbConnection(strAccess);

        using (OleDbCommand cmd = new OleDbCommand(SQLString, conn))
        {
            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                return -1;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }
    }


    /// <summary>
    /// 执行一条计算查询结果语句，返回查询结果（object）。
    /// </summary>
    /// <param name="SQLString">计算查询结果语句</param>
    /// <returns>查询结果（object）</returns>
    public static object GetSingle (string SQLString)
    {
        conn = new OleDbConnection(strAccess);
        using (OleDbCommand cmd = new OleDbCommand(SQLString, conn))
        {
            try
            {
                conn.Open();
                object obj = cmd.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
    }

    /// <summary>
    /// 查看文件是否存在
    /// </summary>
    /// <returns></returns>
    public static string GetFile ()
    {
        string str = "";
        var file = new FileInfo(strAccess);
        if (!file.Exists)
        {
            str = "获取失败！";
        }
        return str;
    }
    /// <summary>
    /// 记录测试记事本
    /// </summary>
    /// <param name="text">信息</param>
    public static void WriteTextLog (string text)
    {
        try
        {
            string dirpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LOG//" + DateTime.Now.ToString("yyyyMMdd") + "log.txt";
            if (!File.Exists(dirpath))
            {
                FileStream fs1 = new FileStream(dirpath, FileMode.Create, FileAccess.Write);//创建写入文件
                fs1.Close();
            }
            StreamWriter sw = new StreamWriter(dirpath, true);
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + text);
            sw.Close();
        }
        catch
        {

        }
    }
}
