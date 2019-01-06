<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addType.aspx.cs" Inherits="addType" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <title>管理首页-添加分类</title>
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="header"><span>陈砦花卉鲜花走势管理系统</span></div>
        <div class="main">
            <div class="sidebar">
                <div class="menu">
                    <ul>
                        <li><a href="./typeList.aspx">分类管理</a></li>
                        <li><a href="./flowerMange.aspx">鲜花管理</a></li>
                        <li><a href="./index.aspx">每日报价</a></li>
                        <li><a href="./remarks.aspx">备注信息</a></li>
                    </ul>
                </div>
            </div>
            <div class="content">
                <div class="crumbs">添加分类</div>
                <div class="cont-box">
                    <div id="typeSelect" class="form-item">
                        <div class="label">请选择类型</div>
                        <div class="inpu">
                            <asp:DropDownList runat="server" ID="drp1" class="selectsty" AutoPostBack="True" OnSelectedIndexChanged="drp1_SelectedIndexChanged">
                                <asp:ListItem Value="0">鲜花类型</asp:ListItem>
                            </asp:DropDownList>
                            <div class="tips" style="font-size: 12px; color: #f00">鲜花类型为顶级类型，如：鲜切花、盆花、苗木，其余可选项为添加的鲜花类型</div>
                        </div>
                    </div>
                    <div id="leveSelect" runat="server" class="form-item" >
                        <div class="label">系列/级别</div>
                        <div class="inpu">
                            <asp:DropDownList runat="server" ID="drp2" class="selectsty" >
                                <asp:ListItem Value="1">系列</asp:ListItem>
                                <asp:ListItem Value="2">级别</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-item">
                        <div class="label">类型名称</div>
                        <div class="inpu">
                            <input runat="server" id="ipt_lxmc" class="inputsty" type="text" />
                        </div>
                    </div>
                    <div class="form-item">
                        <div class="label"></div>
                        <div class="inpu">
                            <asp:Button runat="server" ID="btn_save" class="butsty" OnClick="btn_save_Click" Text="保存" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script src="./js/jquery.min.js"></script>
    </form>
</body>
</html>
