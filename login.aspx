<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>陈砦花卉鲜花走势管理系统</title>
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
</head>
<body>
    <form id="form2" runat="server">
        <div class="login-wrap-bg">
            <div class="main_login">
                <div class="form-item">
                    <div class="label">用户名:</div>
                    <div class="inpu">
                        <input class="inputsty username" runat="server" type="text" value="czadmin" id="txt_userName" />
                    </div>
                </div>
                <div class="form-item">
                    <div class="label">密码:</div>
                    <div class="inpu">
                        <input class="inputsty password" runat="server" type="password" id="txt_password" />
                    </div>
                </div>
                <div class="form-item">
                    <div class="label"></div>
                    <div class="inpu">
                        <asp:Button runat="server" ID="btn_save" class="butsty" OnClick="btn_save_Click" Text="登录" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
