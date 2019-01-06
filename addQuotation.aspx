<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addQuotation.aspx.cs" Inherits="addQuotation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理首页-添加报价</title>
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
    <script src="js/jquery-1.7.1.min.js"></script>
    <script src="My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        function change() {
            document.getElementById("btn_query").click(); /* 当用户操作时自己触发服务端按钮*/
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
                        <li class="curr"><a href="./index.aspx">每日报价</a></li>
                        <li><a href="./remarks.aspx">备注信息</a></li>
                    </ul>
                </div>
            </div>
            <div class="content">
                <div class="crumbs">添加报价</div>
                <div class="cont-box">
                    <div class="form-item">
                        <div class="label">请选择类型</div>
                        <div class="inpu">
                            <asp:DropDownList runat="server" class="selectsty small" ID="drp1" AutoPostBack="true" OnSelectedIndexChanged="drp1_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-item">
                        <div class="label">名称</div>
                        <div class="inpu">
                            <input class="inputsty" runat="server" id="txt_mc" type="text" />
                        </div>
                    </div>
                    <div class="form-item">
                        <div class="label">报价日期</div>
                        <div class="inpu">
                            <input class="inputsty" type="text" runat="server" id="txt_rq" placeholder="请选择日期" onfocus="WdatePicker({skin:'whyGreen',dateFmt:'yyyy-MM-dd'})" />
                        </div>
                    </div>

                    <div class="line"></div>
                    <div class="form-item">
                        <div class="label"></div>
                        <div class="inpu">
                            <div>
                                <table id="tb" class="table-cus" cellspacing="1" cellpadding="0">
                                    <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                                        <HeaderTemplate>
                                            <tr class="table-row head">
                                                <td class="table-col order">系列名称</td>
                                                <td class="table-col aname">鲜花名称</td>
                                                <td class="table-col aprice">价格</td>
                                                <td class="table-col aprice">产地</td>
                                                <td class="table-col aprice">备注</td>
                                                <td class="table-col aprice">等级</td>
                                                <td class="table-col atrend">趋势</td>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="table-row">
                                                <td class="table-col order">
                                                   <%#Eval("lxmc")%>
                                                </td>
                                                <td class="table-col aname">
                                                    <%#Eval("mc")%>
                                                </td>
                                                <td class="table-col aprice">
                                                    <asp:HiddenField runat="server" ID="hd_id" Value='<%#Eval("id") %>' />
                                                    <asp:HiddenField runat="server" ID="hd_fid" Value='<%#Eval("fid") %>' />
                                                    <asp:TextBox runat="server" ID="txt_jg" Text='<%#Eval("jg")%>' CssClass="inputsty samll"></asp:TextBox>
                                                </td>
                                                <td class="table-col aprice">
                                                    <asp:TextBox runat="server" ID="txt_cd" Text='<%#Eval("cd")%>' CssClass="inputsty samll"></asp:TextBox>
                                                </td>
                                                <td class="table-col aprice">
                                                    <asp:TextBox runat="server" ID="txt_bz" Text='<%#Eval("bz")%>' CssClass="inputsty samll"></asp:TextBox>
                                                </td>
                                                <td class="table-col aprice">
                                                    <asp:DropDownList runat="server" ID="drp_dj">
                                                        <asp:ListItem Value="0">无</asp:ListItem>
                                                        <asp:ListItem Value="1">特等</asp:ListItem>
                                                        <asp:ListItem Value="2">A级</asp:ListItem>
                                                        <asp:ListItem Value="3">B级</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="table-col atrend">
                                                    <asp:DropDownList runat="server" ID="drp_qs">
                                                        <asp:ListItem Value="1">上升</asp:ListItem>
                                                        <asp:ListItem Value="2">持平</asp:ListItem>
                                                        <asp:ListItem Value="3">下降</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                            <asp:Button ID="btn_query" runat="server" OnClick="btn_query_Click" Text="Button" Style="display: none" />
                            <%-- 隐藏服务端铵钮--%>
                        </div>
                    </div>
                    <!---->
                    <div class="form-item">
                        <div class="label"></div>
                        <div class="inpu">
                            <asp:Button runat="server" ID="btn_save" OnClick="btn_save_Click" Text="保存" CssClass="butsty" />
                        </div>
                    </div>
                    <!---->
                </div>
            </div>
        </div>
    </form>
</body>
</html>
