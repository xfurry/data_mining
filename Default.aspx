<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication_OLAP.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Easy Data Miner</title>
    <meta http-equiv="Content-Type" content="text/html;charset=iso-8859-1" />
    <link rel="stylesheet" type="text/css" href="style.css" media="screen" />
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <meta name="author" content="Radu Cantor" />
    <meta name="description" content="Data Mining" />
    <meta name="keywords" content="Data Mining" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="content">
        <%-- Login --%>
        <div id="top_info">
            <p>
                Bine ati venit la <b>Easy Data Miner</b> <span id="loginbutton"><a href="#" title="Log In">
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
                <a class="current" href="Default.aspx" accesskey="h"><span class="key">H</span>ome </a>
            </li>
            <li>
                <a href="pages\SqlDataMining.aspx" accesskey="m"><span class="key">M</span>ining relational </a>
            </li>
            <li>
                <a href="pages\OlapDataMining.aspx" accesskey="o"><span class="key">M</span>ining OLAP </a>
            </li>
            <li>
                <a href="pages\SqlReporting.aspx" accesskey="e"><span class="key">R</span>aportare relationala </a>
            </li>
            <li>
                <a href="pages\OlapReporting.aspx" accesskey="r"><span class="key">R</span>aportare OLAP </a>
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
                    <a href="#">Lorem ipsum dolor</a> sit amet, consectetuer adipiscing elit, sed diam
                    nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut
                    wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis
                    nisl ut aliquip ex.</p>
            </div>
            <div class="left_articles">
                <h2>
                    Bine ati venit la Easy Data Miner</h2>
                <p class="date">
                    Posted on 8th September</p>
                <img class="bigimage" src="images/bigimage.gif" alt="Big Image" />
                <p>
                    <a href="#">Lorem ipsum dolor sit amet</a>, consectetuer adipiscing elit, sed diam
                    nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. <a
                        href="#">Ut wisi enim ad minim veniam</a>, quis nostrud exerci tation ullamcorper
                    suscipit lobortis nisl ut aliquip ex.</p>
                <p>
                    <a href="#">Lorem ipsum dolor sit amet</a>, consectetuer adipiscing elit, sed diam
                    nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. <a
                        href="#">Ut wisi enim ad minim veniam</a>, quis nostrud exerci tation ullamcorper
                    suscipit lobortis nisl ut aliquip ex.</p>
            </div>
            <div class="left_box">
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh
                    euismod tincidunt ut laoreet dolore.</p>
            </div>
            <%-- Downpage boxes --%>
            <div class="thirds">
                <p>
                    <b><a href="#" class="title">Web 2.0 business startup tips</a></b><br />
                    Lorem ipsum dolor sit amet esta pa, consectetuer adipiscing elit, sed diam nonummy
                    nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim
                    ad minim veniam, quis nostrud exerci. <a href="#">
                        <img src="images/comment.gif" alt="Comment" /></a></p>
            </div>
            <div class="thirds">
                <p>
                    <b><a href="http://andreasviklund.com/templates/" class="title">Visualize your website</a></b><br />
                    Lorem ipsum dolor sit amet esta pa, consectetuer adipiscing elit, sed diam nonummy
                    nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim
                    ad minim veniam, quis nostrud exerci. <a href="#">
                        <img src="images/comment.gif" alt="Comment" /></a></p>
            </div>
            <div class="thirds">
                <p>
                    <b><a href="http://snews.solucija.com/" class="title">Manage your content</a></b><br />
                    Lorem ipsum dolor sit amet esta pa, consectetuer adipiscing elit, sed diam nonummy
                    nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim
                    ad minim veniam, quis nostrud exerci. <a href="#">
                        <img src="images/comment.gif" alt="Comment" /></a></p>
            </div>
        </div>
        <%-- Right Side --%>
        <div id="right">
            <div class="right_articles">
                <p>
                    <img src="images/image.gif" alt="Image" title="Image" class="image" /><b>Lorem ipsum
                        dolor sit amet</b><br />
                    consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet
                    dolore magna aliquam <a href="#">erat volutpat</a>. Ut wisi enim ad minim veniam,
                    quis nostrud exerci tation ullamcorper suscipit lobortis <a href="#">nisl ut aliquip
                        ex</a>.</p>
            </div>
            <div class="right_articles">
                <p>
                    <img src="images/image.gif" alt="Image" title="Image" class="image" /><b>Lorem ipsum
                        dolor sit amet</b><br />
                    consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet
                    dolore magna aliquam <a href="#">erat volutpat</a>. Ut wisi enim ad minim veniam,
                    quis nostrud exerci tation ullamcorper suscipit lobortis <a href="#">nisl ut aliquip
                        ex</a>.</p>
            </div>
            <div class="right_articles">
                <p>
                    <img src="images/image.gif" alt="Image" title="Image" class="image" /><b>Lorem ipsum
                        dolor sit amet</b><br />
                    consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet
                    dolore magna aliquam <a href="#">erat volutpat</a>. Ut wisi enim ad minim veniam,
                    quis nostrud exerci tation ullamcorper suscipit lobortis <a href="#">nisl ut aliquip
                        ex</a>.</p>
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
