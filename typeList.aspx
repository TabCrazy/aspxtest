<%@ Page Language="C#" AutoEventWireup="true" CodeFile="typeList.aspx.cs" Inherits="typeList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理首页-分类管理</title>
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
    <script type="text/javascript">
        function change() {
            document.getElementById("btn_query").click(); /* 当用户操作时自己触发服务端按钮*/
        }
        function change2() {
            document.getElementById("btn_query2").click(); /* 当用户操作时自己触发服务端按钮*/
        }
    </script>
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
                <div class="crumbs">分类管理</div>
                <div class="cont-box">
                    <div class="list-filter-bar">
                        <div class="label">请选择类型</div>
                        <div class="inpu">
                            <select id="query_lb" runat="server" class="selectsty" onchange="return change()">
                                <option value="0">全部</option>
                            </select>
                            <select class="selectsty" runat="server" onchange="return change2()" id="query_zl">
                                <option value="0">全部</option>
                            </select>
                        </div>
                        <a href="./addType.aspx">添加分类</a>
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
                                            <%#Eval("lxmc")%>
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
                    </div>
                    <asp:Button ID="btn_query" runat="server" OnClick="btn_query_Click" Text="Button" Style="display: none" />
                    <asp:Button ID="btn_query2" runat="server" OnClick="btn_query2_Click" Text="Button" Style="display: none" />
                    <%-- 隐藏服务端铵钮--%>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
