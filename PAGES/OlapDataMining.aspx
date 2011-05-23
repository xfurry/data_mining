<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OlapDataMining.aspx.cs" Inherits="WebApplication_OLAP.classes.DataMining" %>

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
                <a class="current" href="OlapDataMining.aspx" accesskey="o"><span class="key">M</span>ining OLAP </a>
            </li>
            <li>
                <a href="SqlReporting.aspx" accesskey="e"><span class="key">R</span>aportare relationala </a>
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
                <p>
                    <asp:Button ID="ButtonResult" runat="server" Text="Afiseaza continutul structurii" />
                    &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp
                    <asp:DropDownList ID="DropDownListStructures" runat="server">
                    </asp:DropDownList>
                </p>
                <p>
                    <asp:Button ID="ButtonStructure" runat="server" Text="Creaza structura de mining cu numele:"
                        OnClick="ButtonStructure_Click" />
                    &nbsp &nbsp &nbsp
                    <asp:TextBox ID="TextBoxName" runat="server"></asp:TextBox>
                    &nbsp &nbsp &nbsp
                    <asp:Label ID="LabelStatus" runat="server" Text="Rezultatul procesului:"></asp:Label>
                </p>
            </div>
            <div class="left_articles">
                <h2>
                    Rezultatele procesului de mining</h2>
                <p class="date">
                    Posted on 8th September</p>
                <div style="width: 600px; height: 230px; overflow: auto;">
                    <asp:GridView ID="GridViewMain" runat="server" Height="225px" Width="595px">
                    </asp:GridView>
                </div>
            </div>
            <div class="left_articles">
            <h2>
                    Rezultatele detaliate</h2>
                <div style="width: 600px; height: 230px; overflow: auto;">
                    <asp:Panel ID="PanelViewer" runat="server" Height="225px" Width="595px">
                    </asp:Panel>
                    <asp:GridView ID="GridViewDistribution" runat="server" Height="150px" Width="595px"
                        Visible="false">
                    </asp:GridView>
                </div>
            </div>
            <div class="left_box">
                <p>
                    Pentru interogari complexe va rugam sa folositi casuta de mai jos:
                    &nbsp &nbsp &nbsp<asp:Button ID="ButtonExecute" runat="server" Text="Executa interogarea" />
                </p>
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
            </div> --%>
        </div>
        <%-- Right Side --%>
        <div id="right">
            <div class="right_articles">
                <p>
                    Selecteaza algoritmul
                </p>
                <asp:DropDownList ID="DropDownListAlgorithm" runat="server">
                    <asp:ListItem>Clustering</asp:ListItem>
                    <asp:ListItem>Decision Trees</asp:ListItem>
                    <asp:ListItem>Naive Bayes</asp:ListItem>
                    <asp:ListItem>Time Series</asp:ListItem>
                </asp:DropDownList>
                <p>
                    Selecteaza dimensiunea:
                </p>
                <asp:DropDownList ID="DropDownListDimensions" runat="server">
                </asp:DropDownList>
                <p>
                    Selecteaza atributul cheie:
                </p>
                <asp:DropDownList ID="DropDownListKey" runat="server">
                </asp:DropDownList>
            </div>
            <div class="right_articles">
                <p>
                    Selecteaza atributele de intrare:
                    <br />
                    <asp:Button ID="ButtonInput" runat="server" Text="Check All" />
                </p>
                <div style="width: 250px; height: 190px; overflow: auto;">
                    <asp:CheckBoxList ID="CheckBoxListInput" runat="server">
                    </asp:CheckBoxList>
                </div>
            </div>
            <div class="right_articles">
                <p>
                    Selecteaza atributele de predictie:
                    <br />
                    <asp:Button ID="ButtonPredict" runat="server" Text="Check All" />
                </p>
                <div style="width: 250px; height: 190px; overflow: auto;">
                    <asp:CheckBoxList ID="CheckBoxListPredict" runat="server">
                    </asp:CheckBoxList>
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
