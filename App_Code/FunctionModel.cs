using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

/// <summary>
/// FunctionModel 的摘要说明
/// </summary>
public static class FunctionModel
{
    /// <summary>
    /// list 转 datatable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static DataTable ToDataTable<T> (IEnumerable<T> collection)
    {
        var props = typeof(T).GetProperties();
        var dt = new DataTable();
        dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
        if (collection.Count() > 0)
        {
            for (int i = 0; i < collection.Count(); i++)
            {
                ArrayList tempList = new ArrayList();
                foreach (PropertyInfo pi in props)
                {
                    object obj = pi.GetValue(collection.ElementAt(i), null);
                    tempList.Add(obj);
                }
                object[] array = tempList.ToArray();
                dt.LoadDataRow(array, true);
            }
        }
        return dt;
    }
}