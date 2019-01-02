<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addFlower.aspx.cs" Inherits="addFlower" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理首页-添加鲜花</title>
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
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
                    <div class="crumbs">添加鲜花</div>
                    <div class="cont-box">
                        <div class="form-item">
                            <div class="label">鲜花类型</div>
                            <div class="inpu">
                                <asp:DropDownList runat="server" class="selectsty" ID="drp1"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-item">
                            <div class="label">鲜花子类</div>
                            <div class="inpu">
                                <asp:DropDownList runat="server" class="selectsty" ID="drp2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-item">
                            <div class="label">鲜花名称</div>
                            <div class="inpu">
                                <input class="inputsty" type="text" runat="server" id="txt_mc" />
                            </div>
                        </div>
                        <div class="form-item">
                            <div class="label">规格(支/扎)</div>
                            <div class="inpu">
                                <input class="inputsty" type="text" runat="server" id="txt_gg" />
                            </div>
                        </div>
                        <div class="form-item">
                            <div class="label">鲜花图片</div>
                            <div class="inpu">
                                <asp:FileUpload ID="img_tp" runat="server" CssClass="inputsty" />
                            </div>
                        </div>
                        <div class="form-item">
                            <div class="label">是否推荐</div>
                            <div class="inpu">
                                <asp:CheckBox runat="server" ID="ck_tj" />
                            </div>
                        </div>
                        <div class="form-item">
                            <div class="label"></div>
                            <div class="inpu">
                                <asp:Button runat="server" CssClass="butsty" ID="btn_save" OnClick="btn_save_Click" Text="保存" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
