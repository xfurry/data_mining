<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SqlDataMining.aspx.cs" Inherits="WebApplication_OLAP.pages.SqlDataMining" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Data Miner</title>
    <link rel="Stylesheet" href="../style.css" type="text/css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="page">
        <div id="header">
            <h1>
                <a href="#">Data Miner</a></h1>
            <div class="description">
                Mining reporting</div>
        </div>
        <div id="mainarea">
            <div id="contentarea">
                <h2>
                    WELCOME
                </h2>
                <div>
                <br />
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                        Text="Create Mining Structure" />
                    <br />
                    <br />
                    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
                        Text="Show mining results" />
                    <br />
                    <br />
                    <asp:Label ID="LabelStatus" runat="server" Text="Mining status:"></asp:Label>
                <br />
                <br />
                </div>
                <br />
                <div style="width: 536px; height: 227px; overflow: auto;">
                    <asp:GridView ID="GridView1" runat="server">
                    </asp:GridView>
                </div>
                <br />
                <br />
                <br />
                Greysleek is a CSS template that is free and fully standards compliant. <a href="http://www.free-css-templates.com/">
                    Free CSS Templates</a> designed this template. This template is allowed for
                all uses, including commercial use, as it is released under the <strong>Creative Commons
                    Attributions 2.5</strong> license. The only stipulation to the use of this free
                template is that the links appearing in the footer remain intact. Beyond that, simply
                enjoy and have fun with it!
                <br />
                <br />
                Lorem ipsum dolor sit amet, consectetur. Lorem ipsum dolor sit amet, consectetur
                adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip
                ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit
                esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non
                proident, sunt in culpa qui officia deserunt mollit.
                <br />
                <br />
                Anim id est laborum adipisicing elit, sed do eiusmod tempor incididunt ut labore
                et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
                laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit
                in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat
                cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                <br />
                <br />
                Lorem ipsum <a href="#">link</a> dolor sit amet, consectetur adipisicing elit, sed
                do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
                veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo
                consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum
                dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident,
                sunt in culpa qui officia deserunt mollit anim id est laborum.
                <br />
                <br />
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
                    <a class="active" href="../Default.aspx"><span>Home</span></a>
                    <a href="#"><span>Services</span></a>
                    <a href="#"><span>Pricing</span></a>
                    <a href="#"><span>Contact Us</span></a>
                </div>
                <h2>
                    TESTIMONIALS</h2>
                Lorem ipsum dolor sit amet, consectetur. Lorem ipsum dolor sit amet, consectetur
                adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                Lorem ipsum dolor sit amet, consectetur. Lorem ipsum dolor sit amet, consectetur
                adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                <br />
                <br />
                Lorem ipsum dolor sit amet, consectetur. Lorem ipsum dolor sit amet, consectetur
                adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                <br />
                <br />
                <strong>Joe Bloggs</strong>
                <br />
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
