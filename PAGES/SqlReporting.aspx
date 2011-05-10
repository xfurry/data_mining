<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SqlReporting.aspx.cs" Inherits="WebApplication_OLAP.SqlReporting" %>

<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>

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
                    <asp:GridView ID="GridViewMain" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="Select columns">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBoxColumn" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:CheckBox ID="CheckBoxColumnCheck" runat="server" />
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <asp:Button ID="ButtonCheck" runat="server" onclick="ButtonCheck_Click" 
                        Text="Check All" />
                    <asp:Button ID="ButtonUncheck" runat="server" onclick="ButtonUncheck_Click" 
                        Text="Uncheck All" />
                    <br />
                    <br />
                    <asp:Button ID="ButtonRunFields" runat="server" onclick="ButtonRunFields_Click" 
                        Text="Execute selected fields" />
                </div>
                <div style="width: 536px; height: 227px; overflow: auto;">
                    <asp:GridView ID="GridViewData" runat="server">
                    </asp:GridView>
                </div>
                <br />
                <br />
                </div>
            <div id="sidebar">
                <div id="sidebarnav">
                    <a class="active" href="../Default.aspx"><span>Home</span></a>
                    <asp:HyperLink ID="HyperLinkMining" runat="server" 
                        NavigateUrl="~/pages/SqlDataMining.aspx" Visible="False"><span>Mining Report</span></asp:HyperLink>
                </div>
                <h2>
                    Table list</h2>
                <asp:ListBox ID="ListBoxTables" runat="server" AutoPostBack="True" 
                    Height="95px" onselectedindexchanged="ListBoxTables_SelectedIndexChanged" 
                    Width="251px"></asp:ListBox>
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
