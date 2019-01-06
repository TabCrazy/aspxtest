<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditRemarks.aspx.cs" Inherits="EditRemarks" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理首页-编辑备注</title>
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="sidebar">
                <div class="menu">
                    <ul>
                        <li><a href="./typeList.aspx">分类管理</a></li>
                        <li><a href="./flowerMange.aspx">鲜花管理</a></li>
                        <li><a href="./index.aspx">每日报价</a></li>
                        <li class="curr"><a href="./remarks.aspx">备注管理</a></li>
                    </ul>
                </div>
            </div>
            <div class="content">
                <div class="crumbs">编辑备注</div>
                <div class="cont-box">
                    <div class="form-item">
                        <div class="label">备注</div>
                        <div class="inpu">
                            <input runat="server" class="inputsty" type="text" id="txt_bz" />
                        </div>
                    </div>
                    <div class="form-item">
                        <div class="label"></div>
                        <div class="inpu">
                            <asp:Button runat="server" ID="btn_save" OnClick="btn_save_Click" Text="保存" CssClass="butsty" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
