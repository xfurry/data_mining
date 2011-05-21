<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SqlReporting.aspx.cs" Inherits="WebApplication_OLAP.SqlReporting" %>

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
                <a class="current" href="SqlReporting.aspx" accesskey="e"><span class="key">R</span>aportare relationala </a>
            </li>
            <li>
                <a href="OlapReporting.aspx" accesskey="r"><span class="key">R</span>aportare OLAP </a>
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
                    <li><a href="#">Conducting a CMS Survey</a></li>
                    <li><a href="#">Interests behind politics</a></li>
                </ul>
            </div>
            <div class="thirds">
                <ul>
                    <li><a href="#">How stress affects your health</a></li>
                    <li><a href="#">10 ways to buy a used car</a></li>
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
                <p><asp:Button ID="ButtonRunFields" runat="server" OnClick="ButtonRunFields_Click"
                        Text="Executa interogarea" />
                    &nbsp &nbsp &nbsp
                    <asp:Button ID="ButtonExport" runat="server" OnClick="Button1_Click" Text="Exporta rezultatul" />
                    &nbsp &nbsp &nbsp
                    <asp:Label ID="LabelStatus" runat="server" Text="Rezultatul procesului:"></asp:Label>
                </p>
            </div>
            <div class="left_articles">
                <h2>
                    Resultatele raportului relational Easy Data Miner</h2>
                <p class="date">
                    Posted on 8th September</p>
                <div style="width: 600px; height: 230px; overflow: auto;">
                    <asp:GridView ID="GridViewData" runat="server" Height="225px" Width="595px">
                    </asp:GridView>
                </div>
            </div>
            <div class="left_box">
                <p>
                    Pentru interogari complexe va rugam sa folositi casuta de mai jos:
                    &nbsp &nbsp &nbsp
                    <asp:Button ID="ButtonExecute" runat="server" OnClick="ButtonExecute_Click" Text="Executa interogarea" /></p>
                <asp:TextBox ID="TextBoxQuery" runat="server" Height="70px" TextMode="MultiLine"
                    Width="600px"></asp:TextBox>
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
            </div>  --%>
        </div>
        <%-- Right Side --%>
        <div id="right">
            <div class="right_articles">
                <p>
                    Lista surse de date:</p>
                <asp:DropDownList ID="DropDownListDatabases" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListDatabases_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="right_articles">
                <p>
                    Lista de tabele:</p>
                <asp:ListBox ID="ListBoxTables" runat="server" AutoPostBack="True" Height="70px"
                    OnSelectedIndexChanged="ListBoxTables_SelectedIndexChanged" Width="250px"></asp:ListBox>
            </div>
            <div class="right_articles">
                <p>
                    Lista de coloane:
                    <br />
                    <asp:Button ID="ButtonCheck" runat="server" OnClick="ButtonCheck_Click" Text="Selecteaza toate" />
                    &nbsp &nbsp &nbsp
                    <asp:Button ID="ButtonUncheck" runat="server" OnClick="ButtonUncheck_Click" Text="Deselecteaza toate" />
                </p>
                <br />
                <div style="width: 250px; height: 135px; overflow: auto;">
                    <asp:GridView ID="GridViewMain" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="Selecteaza coloana">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBoxColumn" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:CheckBox ID="CheckBoxColumnCheck" runat="server" />
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
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
