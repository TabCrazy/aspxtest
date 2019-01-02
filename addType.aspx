<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addType.aspx.cs" Inherits="addType" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <title>管理首页-添加分类</title>
    <link rel="stylesheet" type="text/css" href="./css/style.css">
</head>
<body>
    <form id="form1" runat="server">


        <div class="header"><span>陈砦花卉鲜花走势管理系统</span></div>
        <div class="main">
            <div class="sidebar">
                <div class="menu">
                    <ul>
                        <li><a href="./typeList.html">分类管理</a></li>
                        <li><a href="./flowerMange.html">鲜花管理</a></li>
                        <li><a href="./index.html">每日报价</a></li>
                        <li><a href="./remarks.html">备注管理</a></li>
                    </ul>
                </div>
            </div>
            <div class="content">
                <div class="crumbs">添加分类</div>
                <div class="cont-box">
                    <div id="typeSelect" class="form-item">
                        <div class="label">请选择类型</div>
                        <div class="inpu">
                            <select runat="server" id="select_type" class="selectsty">
                                <option value="0">鲜花类型</option>
                            </select>
                            <div class="tips" style="font-size: 12px; color: #f00">鲜花类型为顶级类型，如：鲜切花、盆花、苗木，其余可选项为添加的鲜花类型</div>
                        </div>
                    </div>
                    <div id="leveSelect" class="form-item" style="display: none">
                        <div class="label">子类/级别</div>
                        <div class="inpu">
                            <select class="selectsty" runat="server" id="select_zl">
                                <option value="1">子类</option>
                                <option value="2">级别</option>
                            </select>
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
        <script>
            $(function () {
                console.log(222);
                $("#typeSelect").find('.selectsty').bind('change', function (e) {
                    var typeVal = $("#typeSelect").find('.selectsty').val() - 0;
                    if (typeVal !== 1) {
                        $("#leveSelect").show();
                    } else {
                        $("#leveSelect").hide();
                    }
                })
            })
        </script>
    </form>
</body>
</html>
