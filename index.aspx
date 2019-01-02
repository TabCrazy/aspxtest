<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理首页-每日报价</title>
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
                <div class="crumbs">每日报价</div>
                <div class="cont-box">
                    <div class="list-filter-bar">
                        <a class="btn-left" href="./addQuotation.aspx">添加报价</a>
                    </div>
                    <div class="list-wrap">
                        <table class="table-cus" cellspacing="1" cellpadding="0">
                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                <HeaderTemplate>
                                    <tr class="table-row">
                                        <td class="table-col order">序号
                                        </td>
                                        <td class="table-col lname">名称
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
                                        <td class="table-col lname">
                                            <%#Eval("bjName")%>
                                        </td>
                                        <td class="table-col opt">
                                            <asp:LinkButton ID="LinkButton1" CommandArgument='<%#Eval("id")%>'
                                                CommandName="Edit" runat="server">编辑</asp:LinkButton>
                                            <asp:LinkButton ID="LinkButton2" CommandArgument='<%#Eval("id")%>' CommandName="Delete"
                                                runat="server" OnClientClick='return confirm("是否确定删除此记录！");'>删除</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <table>
                            <tr>
                                <td>总记录数：<asp:Label ID="lblCount" runat="server" Text=""></asp:Label>
                                </td>
                                <td>第<asp:Label ID="lblCurrentPage" runat="server"></asp:Label>页/ 共<asp:Label ID="lblPage"
                                    runat="server"></asp:Label>页
                                </td>
                                <td>
                                    <asp:LinkButton ID="first" runat="server" OnClick="first_Click"
                                        OnClientClick="return upData();">首页</asp:LinkButton>
                                    <asp:LinkButton ID="up" runat="server" OnClick="up_Click"
                                        OnClientClick="return upData();">上一页</asp:LinkButton>
                                    <asp:LinkButton ID="next" runat="server" OnClick="next_Click"
                                        OnClientClick="return upData();">下一页</asp:LinkButton>
                                    <asp:LinkButton ID="last" runat="server" OnClick="last_Click">末页</asp:LinkButton>
                                </td>
                                <td>跳转：<asp:TextBox ID="txtPage" Width="50px" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                                </td>
                                <td>
                                    <asp:HiddenField ID="hfData" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Button ID="btn_query" runat="server" OnClick="btn_query_Click" Text="Button" Style="display: none" />
                    <%-- 隐藏服务端铵钮--%>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
