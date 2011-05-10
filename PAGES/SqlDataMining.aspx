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
                    Mining data manager
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
            </div>
            <div id="sidebar">
                <div id="sidebarnav">
                    <a class="active" href="../Default.aspx"><span>Home</span></a>
                    <a class="active" href="SqlReporting.aspx"><span>SQL Report</span></a>
                </div>
                <h2>
                    Options
                </h2>
                <p>Select key column</p>
                <asp:DropDownList ID="DropDownListKey" runat="server">
                    </asp:DropDownList>
                <br />
                <p>Select input columns</p>
                <asp:CheckBoxList ID="CheckBoxListColumns" runat="server">
                </asp:CheckBoxList>
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
