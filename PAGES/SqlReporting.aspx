<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SqlReporting.aspx.cs" Inherits="WebApplication_OLAP.SqlReporting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                Relational reporting
            </div>
        </div>
        <div id="mainarea">
            <div id="contentarea">
                <h2>
                    Retrieve data here</h2>
                <div style="width: 536px; height: 137px; overflow: auto;">
                    <asp:Button ID="ButtonExecute" runat="server" OnClick="ButtonExecute_Click" Text="Execute Query" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ButtonExport" runat="server" onclick="Button1_Click" 
                        Text="Export to Excel" />
&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                    <br />
                    <asp:TextBox ID="TextBoxQuery" runat="server" Height="106px" TextMode="MultiLine"
                        Width="524px"></asp:TextBox>
                    <br />
                </div>
                <div style="width: 536px; height: 227px; overflow: auto;">
                    <asp:GridView ID="GridView1" runat="server">
                    </asp:GridView>
                </div>
                <br />
                <br />
                Lorem ipsum dolor sit amet, consectetur. Lorem ipsum dolor sit amet, consectetur
                adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip
                ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit
                esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non
                proident, sunt in culpa qui officia deserunt mollit.
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
