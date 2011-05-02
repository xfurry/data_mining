<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication_OLAP.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Miner</title>
    <link rel="Stylesheet" href="style.css" type="text/css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="page">
        <div id="header">
            <h1>
                <a href="#">Data Miner</a>
            </h1>
            <div class="description">
                Data Mining software
             </div>
        </div>
        <div id="mainarea">
            <div id="contentarea">
                <h2>
                    WELCOME
                </h2>
                Greysleek is a CSS template that is free and fully standards compliant. <a href="http://www.free-css-templates.com/">
                    Free CSS Templates</a> designed this template. This template is allowed for
                all uses, including commercial use, as it is released under the <strong>Creative Commons
                    Attributions 2.5</strong> license. The only stipulation to the use of this free
                template is that the links appearing in the footer remain intact. Beyond that, simply
                enjoy and have fun with it!
                <br/>
                <br/>
                Lorem ipsum dolor sit amet, consectetur. Lorem ipsum dolor sit amet, consectetur
                adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip
                ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit
                esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non
                proident, sunt in culpa qui officia deserunt mollit.
                <br/>
                <br/>
                Anim id est laborum adipisicing elit, sed do eiusmod tempor incididunt ut labore
                et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
                laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit
                in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat
                cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                <br/>
                <br/>
                Lorem ipsum <a href="#">link</a> dolor sit amet, consectetur adipisicing elit, sed
                do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
                veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo
                consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum
                dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident,
                sunt in culpa qui officia deserunt mollit anim id est laborum.
                <br/>
                <br/>
                Lorem ipsum dolor sit amet, consectetur. Lorem ipsum dolor sit amet, consectetur
                adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip
                ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit
                esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non
                proident, sunt in culpa qui officia deserunt mollit anim id est laborum adipisicing
                elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim
                ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea
                commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse
                cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident,
                sunt in culpa qui officia deserunt mollit anim id est laborum.
            </div>
            <div id="sidebar">
                <div id="sidebarnav">
                    <a class="active" href="Default.aspx"><span>Home</span></a>
                    <asp:HyperLink ID="HyperLinkOlap" runat="server" 
                        NavigateUrl="~/pages/OlapReporting.aspx" Visible="False"><span>Olap Report</span></asp:HyperLink>
                    <asp:HyperLink ID="HyperLinkSql" runat="server" 
                        NavigateUrl="~/pages/SqlReporting.aspx" Visible="False"><span>Sql Report</span></asp:HyperLink>
                    <a href="#"><span>Contact Us</span></a>
                </div>
                <h2>
                    Login
                </h2>
                User name:
                <br />
                <asp:TextBox ID="TextBoxUserName" runat="server"></asp:TextBox>
                <br />
                <br />
                Password:
                <br />
                <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="ButtonLogin" runat="server" Text="Login" 
                    onclick="ButtonLogin_Click" />
                <br />
                <br />
                <asp:Label ID="LabelLogin" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div id="footer">
            <a href="http://www.templatesold.com/" target="_blank">Website Templates</a> by
            <a href="http://www.free-css-templates.com/" target="_blank">Free CSS Templates</a>
        </div>
    </div>
    </form>
</body>
</html>
