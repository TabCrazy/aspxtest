<%@ Page Language="C#" AutoEventWireup="true" CodeFile="remarks.aspx.cs" Inherits="remarks" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理首页-备注管理</title>
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
                        <li><a href="./remarks.aspx">备注管理</a></li>
                    </ul>
                </div>
            </div>
            <div class="content">
                <div class="crumbs">备注管理</div>
                <div class="cont-box">
                    <div class="list-wrap">
                        <table class="table-cus" cellspacing="1" cellpadding="0">
                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                <HeaderTemplate>
                                    <tr class="table-row head">
                                        <td class="table-col order">序号
                                        </td>
                                        <td class="table-col sname">备注
                                        </td>
                                        <td class="table-col opt">操作
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="table-row">
                                        <td class="table-col order">
                                            <%#Container.ItemIndex + 1%>
                                        </td>
                                        <td class="table-col remarks">
                                            <%#Eval("bz")%>
                                        </td>
                                        <td class="table-col opt">
                                            <asp:LinkButton ID="LinkButton1" CommandArgument='<%#Eval("id")%>' CommandName="Edit" runat="server">编辑</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
