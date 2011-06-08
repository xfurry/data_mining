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
                <a class="current" href="Default.aspx" accesskey="h"><span class="key">H</span>ome </a>
            </li>
            <li>
                <a href="pages/SqlDataMining.aspx" accesskey="m"><span class="key">M</span>ining relational </a>
            </li>
            <li>
                <a href="pages/OlapDataMining.aspx" accesskey="o"><span class="key">M</span>ining OLAP </a>
            </li>
            <li>
                <a href="pages/SqlReporting.aspx" accesskey="e"><span class="key">R</span>aportare relationala </a>
            </li>
            <li>
                <a href="pages/OlapReporting.aspx" accesskey="r"><span class="key">R</span>aportare OLAP </a>
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
                    Data mining permite unei afaceri a colecta informatii dintr-o varietate de surse,
                    analiza de date folosind datele-mining software, de incarcare de informatii intr-o baza
                    de date, stocheaza informatiile, si sa furnizeze informatii analizate intr-un format util,
                    cum ar fi un raport, tabel, sau un grafic. Care se refera la prognoza analiza de business si
                    de afaceri, informatiile analizate este clasificata de a determina modelele importante si relatii.</p>
            </div>
            <div class="left_articles">
                <h2>
                    Bine ati venit la Easy Data Miner</h2>
                <p class="date">
                    6th June 2011</p>
                <%--<img class="bigimage" src="images/bigimage.gif" alt="Big Image" />--%>
                <p>
                    Solutiile noastre de data management au ca scop schimbul de informatii corecte intre
                    bazele de date si canale de comunicare prin standardizarea si automatizarea transferului
                    de date. Companiile acorda o atentie din ce in ce mai mare datelor despre clienti
                    pentru a analiza comportamentul acestora. Comportamentul consumatorilor nu este
                    singurul motiv pentru a gandi o strategie de managementul datelor din interiorul
                    companiei. Companiile se confrunta cu o noua provocare actuala. Datele importante
                    despre clienti, produse, furnizori si locatii sunt raspandite in sisteme disparate,
                    canalele de vanzare, unitati comerciale, baze de date ale departamentelor si alte
                    locatii. Solutiile de data managemant reunesc aceste date si le mentin intr-un mod
                    centralizat. Acestea au ca scop schimbul de informatii corecte intre bazele de date
                    si canale de comunicare prin standardizarea si automatizarea transferului de date.
                    Astfel, se imbunatatesc procesele de afaceri, avand ca efect cresterea profitabilitatii,
                    o gestionare eficienta a inventarului, si cresterea cotei de vanzari. Produsele,
                    clientii si furnizorii sunt cele trei entitati care sunt implicate in procesul de
                    analiza si gestionare a sistemelor de managemant a datelor.</p>
            </div>
            <div class="left_box">
                <p>
                    Atunci cand o afacere beneficiaza de puterea data mining, ei sunt capabili de a dobandi cunostinte importante care ii vor ajuta sa nu dezvolte strategii eficiente de marketing numai duc la decizii mai bune in afaceri, dar va ajuta la identificarea tendintelor viitoare, in special industria lor. Data mining a devenit un instrument esential pentru a ajuta afacerile castiga un avantaj competitiv</p>
            </div>
            <%-- Downpage boxes --%>
<%--            <div class="thirds">
                <p>
                    Privit în ansamblu, o soluţie de data managemant reprezintă o gestionare a validărilor
                    şi a modalităţii în care este efectuată prelucrarea efectivă a datelor la nivelul
                    creării, editării şi ştergerii obiectelor implicate în proces. Din punctul de vedere
                    al beneficiilor pe care o soluţie de data managemant le oferă acestea sunt multiple
                    şi pornesc de la identificarea şi eliminarea în timp real a erorilor, înainte ca
                    acestea să fie înregistrate în baza de date a companiei şi identificare surselor
                    de eroare</p>
            </div>
            <div class="thirds">
                <p>
                    De asemenea, se creează un set optim de interfeţe care elimină redundanţele de date
                    apărute în grilele bazei de date a companiei. Un alt avantaj este reprezentat de
                    uşurinţa cu care se realizează comunicarea eficientă cu informaţiile aflate pe suport
                    extern. Accelerarea vitezei de procesare a datelor este de asemenea unul din avantajele
                    oferite de soluţiile de managemant a datelor. Aceasta se obţine prin eliminarea
                    redundanţei informaţionale şi prin înglobarea tuturor informaţiilor externe necesare
                    într-o bază de date</p>
            </div>
            <div class="thirds">
                <p>
                    De asemenea, prin utilizarea unor astfel de soluţii se optimizează şi se automatizează
                    metodelele de comunicare, respectiv mail-uri automate către alte departamente, gestiune
                    a task-urilor, notificări automate. Un avantaj major este reprezentat de posibilitatea
                    corectării erorilor atât prin procese automate cât şi prin operaţiuni de responsabilizare
                    validare, decizie managerială, etc. Nu în ultimul rând, soluţiile de managemant
                    a datelor oferă posibilitatea de reducere a muncii manuale şi integrarea ei într-un
                    proces eficient pe baza de validare la nivel de user.</p>
            </div>--%>
        </div>
        <%-- Right Side --%>
        <div id="right">
            <div class="right_articles">
                <p>
                    Data mining este cea mai mare parte folosite de catre companii, cu un accent puternic pe obiceiurile de cumparaturi ale consumatorilor, cum informatii, analize financiare, de marketing evaluari, etc </p>
            </div>
            <div class="right_articles">
                <p>
                    Privit in ansamblu, o solutie de data managemant reprezinta o gestionare a validarilor si a modalitatii in care este efectuata prelucrarea efectiva a datelor la nivelul crearii, editarii si stergerii obiectelor implicate in proces</p>
            </div>
            <div class="right_articles">
                <p>
                    Accelerarea vitezei de procesare a datelor este de asemenea unul din avantajele oferite de solutiile de managemant a datelor. Aceasta se obtine prin eliminarea redundantei informationale si prin inglobarea tuturor informatiilor externe necesare intr-o baza de date</p>
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
