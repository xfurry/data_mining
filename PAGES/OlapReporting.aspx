<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OlapReporting.aspx.cs" Inherits="WebApplication_OLAP._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Easy Data Miner</title>
    <link rel="Stylesheet" href="../style.css" type="text/css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="content">
        <%-- Login --%>
        <div id="top_info">
            <p>
                Bine ati venit la <b>Easy Data Miner</b> <span id="loginbutton"><a href="#" title="Autentificare">
                    &nbsp;</a></span><br />
                <b>Nu sunteti autentificat!</b> <a href="#">Autentificare</a> </p>
        </div>
        <%-- Logo --%>
        <div id="logo">
            <h1>
                <a href="#" title="Noi scoatem datele la lumina.">Easy Data Miner</a></h1>
            <p id="slogan">
                Noi scoatem datele la lumina.</p>
        </div>
        <%-- Tab links --%>
        <ul id="tablist">
            <li>
                <a href="../Default.aspx" accesskey="h"><span class="key">H</span>ome </a>
            </li>
            <li>
                <a href="SqlDataMining.aspx" accesskey="m"><span class="key">M</span>ining relational </a>
            </li>
            <li>
                <a href="OlapDataMining.aspx" accesskey="o"><span class="key">M</span>ining OLAP </a>
            </li>
            <li>
                <a href="SqlReporting.aspx" accesskey="e"><span class="key">R</span>aportare relationala </a>
            </li>
            <li>
                <a class="current" href="OlapReporting.aspx" accesskey="r"><span class="key">R</span>aportare OLAP </a>
            </li>
        </ul>
        <%-- Topic list --%>
        <div id="topics">
            <div class="thirds">
                <p>
                    <br />
                    Anunturi importante:</p>
            </div>
            <div class="thirds">
                <ul>
                    <li><a href="http://dataminingarticles.com/data-mining-classification.html">
                        Data Mining Classification</a></li>
                    <li><a href="http://www.nag.co.uk/IndustryArticles/DMinFinancialApps.pdf">
                        Data Mining in financial applications</a></li>
                </ul>
            </div>
            <div class="thirds">
                <ul>
                    <li><a href="http://www.nag.co.uk/IndustryArticles/UnleashingDMpotential.pdf">
                        Data Mining full potential</a></li>
                    <li><a href="http://www.dataminingarticles.com/Grab-Bag-Frequently-Asked-Data-Mining-Questions.html">
                        FAQ</a></li>
                </ul>
            </div>
        </div>
        <%-- Search form --%>
        <div id="search">
            <form method="post" action="?">
            <p>
                <input type="text" name="search" class="search" />
                <input type="submit" value="Search" class="button" /></p>
            </form>
        </div>
        <div id="left">
            <%-- Content Area --%>
            <div class="subheader">
                <p>
                    <asp:Button ID="ButtonExecute" runat="server" OnClick="Button1_Click" Text="Executa interogarea" />
                    &nbsp &nbsp &nbsp
                    <asp:Button ID="ButtonExport" runat="server" OnClick="Button2_Click" Text="Exporta rezultatul" />
                    &nbsp &nbsp &nbsp
                    <asp:Label ID="LabelStatus" runat="server" Text="Rezultatul procesului:"></asp:Label>
                </p>
            </div>
            <div class="left_articles">
                <h2>
                    Resultatele raportului OLAP Easy Data Miner</h2>
                <p class="date">
                    6th June 2011</p>
                    <%-- Result table --%>
                <div style="width: 600px; height: 230px; overflow: auto;">
                    <asp:GridView ID="GridViewData" runat="server" Height="225px" Width="595px">
                    </asp:GridView>
                </div>
            </div>
            <%-- Query box --%>
            <div class="left_box">
                <p>
                    Pentru o interogare customizata va rog sa scrieti codul in casuta de mai jos</p>
                    <asp:TextBox ID="TextBoxQuery" runat="server" Height="70px" 
                    TextMode="MultiLine" Width="600px"></asp:TextBox>
            </div>
            <%-- Downpage boxes
            <div class="thirds">
                <p></p>
            </div>
            <div class="thirds">
                <p></p>
            </div>
            <div class="thirds">
                <p></p>
            </div> --%>
        </div>
        <%-- Right Side --%>
        <div id="right">
            <div class="right_articles">
                <p>
                    Lista de cuburi:</p>
                <asp:DropDownList ID="DropDownListCubes" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="right_articles">
                <p>
                    Lista de dimensiuni:</p>
                <asp:ListBox ID="ListBoxDimensions" runat="server" Height="75"></asp:ListBox>
            </div>
            <div class="right_articles">
                <p>
                    Lista de membrii:</p>
                <asp:TreeView ID="TreeViewMembers" runat="server" Height="150">
                </asp:TreeView>
            </div>
            <div class="notes">
                <p>
                    If you liked this template you might like some other CSS templates from <a href="http://www.solucija.com/">
                        Solucija</a>.</p>
            </div>
        </div>
        <%-- Footer --%>
        <div id="footer">
            <p class="right">
                &copy; 2011 Easy Data Mining, Design: Radu Cantor, <a title="Awsome Web Templates"
                    href="http://www.solucija.com/">Solucija</a></p>
            <p>
                <a href="#">RSS Feed</a> &middot; <a href="#">Contact</a> &middot; <a href="#">Accessibility</a>
                &middot; <a href="#">Products</a> &middot; <a href="#">Disclaimer</a> &middot; <a
                    href="http://jigsaw.w3.org/css-validator/check/referer">CSS</a> and <a href="http://validator.w3.org/check?uri=referer">
                        XHTML</a><br />
            </p>
        </div>
    </div>
    </form>
    </body>
</html>
