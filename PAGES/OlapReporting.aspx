<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OlapReporting.aspx.cs" Inherits="WebApplication_OLAP._Default" %>

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
                Multidimensional reporting
            </div>
        </div>
        <div id="mainarea">
            <div id="contentarea">
                <h2>
                    REPORT
                </h2>
                <div style="width: 536px; height: 300px; overflow: auto;">
                    <asp:Label ID="Label1" runat="server" Text="Cubes"></asp:Label>
                    &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <br />
                    <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp &nbsp<br />
                    <asp:Label ID="Label2" runat="server" Text="Dimensions"></asp:Label>
                    <br />
                    <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
                    <br />
&nbsp;&nbsp<asp:TreeView ID="TreeView1" runat="server">
                    </asp:TreeView>
                    </div>
                <div style="width: 536px; height: 34px; overflow: auto;">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Execute Query" />
                    &nbsp &nbsp &nbsp
                    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Export result" />
                    &nbsp &nbsp &nbsp
                    <asp:Label ID="Label3" runat="server" Text="Export status:"></asp:Label>
                    <br />
                </div>
                <div style="width: 536px; height: 227px; overflow: auto;">
                    <asp:GridView ID="GridView1" runat="server">
                    </asp:GridView>
                </div>
                <div style="width: 536px; height: 121px; overflow: auto;">
                    <asp:TextBox ID="TextBox1" runat="server" Height="108px" TextMode="MultiLine" 
                        Width="515px"></asp:TextBox>
                </div>
            </div>
            <div id="sidebar">
                <div id="sidebarnav">
                    <a class="active" href="../Default.aspx"><span>Home</span></a>
                    <asp:HyperLink ID="HyperLinkMining" runat="server" 
                        NavigateUrl="~/pages/OlapDataMining.aspx" Visible="False"><span>Mining Report</span></asp:HyperLink>
                    <a href="#"><span>Services</span></a>
                    <a href="#"><span>Pricing</span></a>
                    <a href="#"><span>Contact Us</span></a>
                </div>
                <h2>
                    TESTIMONIALS
                </h2>
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
