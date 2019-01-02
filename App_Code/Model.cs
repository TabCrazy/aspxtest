using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Model 的摘要说明
/// </summary>
public class Model
{
    public class Type
    {
        public string id;
        public string lx;
        public string zl;
        public string mc;
        public string sjlx;
    }
    public class FlowerPrice
    {
        public string id;
        public string name;
        public string price;
    }
    public class GetTrendModel
    {
        public string title;
        public string date;
        public string id;
        // public List<FlowerPrice> list;
        public DataTable list;
    }
}