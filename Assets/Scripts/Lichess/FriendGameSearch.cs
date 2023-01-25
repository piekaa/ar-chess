﻿using System.Collections;
using System.Threading;
using UnityEngine;

public class FriendGameSearch : EventListener
{
    [SerializeField] private LoginData loginData;

    private volatile WebSocket webSocket;
    private string location;

    private Coroutine nulls;
    private Coroutine pings;
    
    [Listen(EventName.ArUiFriendNameSelected)]
    private void SearchGame(EventData eventData)
    {
        new Thread(() =>
        {
            Debug.Log("Setup request");
            location =
                LichessHttp.PostMatchRequestAndGetLocation(eventData.Text, loginData.cookie, "/setup/friend");

            Debug.Log("Location: " + location);

          
            var (status, _) = Http.PostMultipart("lichess.org", $"/challenge{location}/to-friend", loginData.cookie,
                new()
                {
                    //todo get name from event
                    { "username", "piekaa" }
                });

            webSocket = new WebSocket("socket4.lichess.org", "lichess.org",
                $"/challenge{location}/socket/v6?sri=CVZEVKrY9Fry&v=1", loginData.cookie);

            Debug.Log("To fiend status");
        }).Start();

        nulls = StartCoroutine(SendNulls());
        pings = StartCoroutine(SendPings());
    }

    protected override void MyUpdate()
    {
        if (webSocket == null)
        {
            return;
        }

        var message = webSocket.DequeueMessageOrNull();

        if (message == null)
        {
            return;
        }


        var textMessage = message as WebSocketTextMessage;

        if (textMessage != null)
        {
            Debug.Log(textMessage.Text);
            var text = textMessage.Text;

            if (!text.StartsWith("{"))
            {
                return;
            }
            
            var lichessMessage = Piekson.FromJson<LichessMessage>(text);

            if (lichessMessage.t == "reload")
            {
                new Thread(() =>
                {
                    var response = Http.Get("lichess.org", location, loginData.cookie);
                    Debug.Log(response);

                    if (webSocket == null)
                    {
                        return;
                    }
                    
                    if (response.Contains("<div id=\"zenzone\">"))
                    {
                        webSocket.Disconnect();
                        webSocket = null;
                        EventSystem.Instance.Fire(EventName.GameFound, new EventData(location));
                    }
                }).Start();
                /*



oczekiwanie
<!DOCTYPE html><html lang="pl-PL"><!-- Lichess is open source! See https://lichess.org/source --><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1,viewport-fit=cover"><meta http-equiv="Content-Security-Policy" content="default-src 'self' lichess1.org; connect-src 'self' lichess1.org wss://socket0.lichess.org wss://socket1.lichess.org wss://socket2.lichess.org wss://socket3.lichess.org wss://socket4.lichess.org wss://socket5.lichess.org https://explorer.lichess.ovh https://tablebase.lichess.ovh; style-src 'self' 'unsafe-inline' lichess1.org; frame-src 'self' lichess1.org www.youtube.com player.twitch.tv; worker-src 'self' lichess1.org; img-src data: *; script-src 'nonce-o2C5RR55d545d1zIwBCtPfQQ' 'self' lichess1.org; font-src 'self' lichess1.org; base-uri 'none';"><meta name="theme-color" content="#2e2a24"><title>Bullet (Â¾+1) casual Chess â¢ piekoszekdev (869) challenges piekaa (1030) â¢ lichess.org</title><link href="https://lichess1.org/assets/_YpE1iP/css/site.ltr.dark.min.css" rel="stylesheet" /><link href="https://lichess1.org/assets/_YpE1iP/css/challenge.page.ltr.dark.min.css" rel="stylesheet" /><link id="piece-sprite" href="https://lichess1.org/assets/_YpE1iP/piece-css/cburnett.css" rel="stylesheet" /><meta content="Join the challenge or watch the game here." name="description" /><link rel="mask-icon" href="https://lichess1.org/assets/_YpE1iP/logo/lichess.svg" color="black" /><link rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-512.png" sizes="512x512"><link rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-256.png" sizes="256x256"><link rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-192.png" sizes="192x192"><link rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-128.png" sizes="128x128"><link rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-64.png" sizes="64x64"><link id="favicon" rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-32.png" sizes="32x32"><meta name="google" content="notranslate"><meta property="og:title" content="Bullet (Â¾+1) casual Chess â¢ piekoszekdev (869) challenges piekaa (1030)" /><meta property="og:description" content="Join the challenge or watch the game here." /><meta property="og:url" content="https://lichess.org/MWZK8pQf" /><meta property="og:type" content="website" /><meta property="og:site_name" content="lichess.org" /><meta name="twitter:card" content="summary" /><meta name="twitter:title" content="Bullet (Â¾+1) casual Chess â¢ piekoszekdev (869) challenges piekaa (1030)" /><meta name="twitter:description" content="Join the challenge or watch the game here." /><link href="/blog.atom" title="Blog" type="application/atom+xml" rel="alternate" /><link rel="preload" href="https://lichess1.org/assets/_YpE1iP/font/lichess.woff2" as="font" type="font/woff2" crossorigin><link rel="preload" href="https://lichess1.org/assets/_YpE1iP/font/noto-sans-v14-latin-regular.woff2" as="font" type="font/woff2" crossorigin><link rel="preload" href="https://lichess1.org/assets/_YpE1iP/font/lichess.chess.woff2" as="font" type="font/woff2" crossorigin><link rel="preload" href="https://lichess1.org/assets/_YpE1iP/images/board/svg/brown.svg" as="image" ><link rel="manifest" href="/manifest.json"><meta name="twitter:site" content="@lichess"><link rel="jslicense" href="/source"></head><body class="dark brown Woodi Basic coords-in" data-vapid="BGr5CL0QlEYa7qW7HLqe7DFkCeTsYMLsi1Db+5Vwt1QBIs6+WxN8066AjtP8S9u+w+CbleE8xWY+qQaNEMs7sAs=" data-user="piekoszekdev" data-sound-set="standard" data-socket-domains="socket0.lichess.org,socket1.lichess.org,socket2.lichess.org,socket3.lichess.org,socket4.lichess.org,socket5.lichess.org" data-asset-url="https://lichess1.org" data-asset-version="YpE1iP" data-theme="dark" data-board-theme="brown" data-piece-set="cburnett"><form id="blind-mode" action="/toggle-blind-mode" method="POST"><input type="hidden" name="enable" value="1"><input type="hidden" name="redirect" value="/MWZK8pQf"><button type="submit">Accessibility: Enable blind mode</button></form><header id="top"><div class="site-title-nav">
<input type="checkbox" id="tn-tg" class="topnav-toggle fullscreen-toggle" autocomplete="off" aria-label="Navigation">
<label for="tn-tg" class="fullscreen-mask"></label>
<label for="tn-tg" class="hbg"><span class="hbg__in"></span></label><h1 class="site-title"><a href="/">lichess<span>.org</span></a></h1><nav id="topnav" class="hover"><section><a href="/"><span class="play">Zagraj</span><span class="home">lichess.org</span></a><div role="group"><a href="/?any#hook">Nowa partia</a><a href="/tournament">Areny</a><a href="/swiss">System szwajcarski</a><a href="/simul">Symultany</a></div></section><section><a href="/training">Zadania szachowe</a><div role="group"><a href="/training">Zadania szachowe</a><a href="/training/dashboard/30">Tablica z zadaniami</a><a href="/streak">Puzzle Streak</a><a href="/storm">Puzzle Storm</a><a href="/racer">Puzzle Racer</a></div></section><section><a href="/practice">Nauka</a><div role="group"><a href="/learn">Podstawy gry</a><a href="/practice">Äwiczenia</a><a href="/training/coordinate">WspÃ³ÅrzÄdne pÃ³l</a><a href="/study">Opracowania</a><a href="/coach">Trenerzy</a></div></section><section><a href="/tv">OglÄdaj</a><div role="group"><a href="/tv">Lichess TV</a><a href="/games">Partie w toku</a><a href="/streamer">Streamerzy</a><a href="/broadcast">Transmisje</a><a href="/video">Wideoteka</a></div></section><section><a href="/player">SpoÅecznoÅÄ</a><div role="group"><a href="/player">Gracze</a><a href="/team">Kluby</a><a href="/forum">Forum</a><a href="/blog/community">Blog</a></div></section><section><a href="/analysis">NarzÄdzia</a><div role="group"><a href="/analysis">Analiza partii</a><a href="/opening">Otwarcia</a><a href="/editor">Edytor pozycji</a><a href="/paste">Importuj partiÄ</a><a href="/games/search">Wyszukiwanie zaawansowane</a></div></section></nav><a class="site-title-nav__donate" href="/patron">PrzekaÅ¼ datek</a></div><div class="site-buttons"><div id="clinput"><a class="link"><span data-icon="î¬"></span></a><input spellcheck="false" autocomplete="false" aria-label="Szukaj" placeholder="Szukaj" /></div><div><a id="challenge-toggle" class="toggle link"><span title="Wyzwania" class="data-count" data-count="0" data-icon="î"></span></a><div id="challenge-app" class="dropdown"></div>
</div>
<div><a id="notify-toggle" class="toggle link"><span title="Powiadomienia" class="data-count" data-count="0" data-icon="î"></span></a><div id="notify-app" class="dropdown"></div>
</div><div class="dasher"><a id="user_tag" class="toggle link" href="/logout">piekoszekdev</a><div id="dasher_app" class="dropdown"></div></div></div></header><div id="main-wrap" class="is2d"><main class="page-small challenge-page box box-pad challenge--created"><div id="ping-challenge"><h1 class="box__top">ZaproÅ do gry</h1><div class="details"><div class="variant" data-icon="î"><div><span title="Partie bardzo szybkie: mniej niÅ¼ 3 minuty">Bullet</span><br /><span class="clock">Â¾+1</span></div></div><div class="mode">BiaÅe â¢ Towarzyska</div></div><div class="waiting"><a class="online user-link target ulpt" href="/@/piekaa"><i class="line"></i>piekaa</a><div class="spinner"><svg viewBox="-2 -2 54 54"><g mask="url(#mask)" fill="none"><path id="a" stroke-width="3.779" d="m21.78 12.64c-1.284 8.436 8.943 12.7 14.54 17.61 3 2.632 4.412 4.442 5.684 7.93"/><path id="b" stroke-width="4.157" d="m43.19 36.32c2.817-1.203 6.659-5.482 5.441-7.623-2.251-3.957-8.883-14.69-11.89-19.73-0.4217-0.7079-0.2431-1.835 0.5931-3.3 1.358-2.38 1.956-5.628 1.956-5.628"/><path id="c" stroke-width="4.535" d="m37.45 2.178s-3.946 0.6463-6.237 2.234c-0.5998 0.4156-2.696 0.7984-3.896 0.6388-17.64-2.345-29.61 14.08-25.23 27.34 4.377 13.26 22.54 25.36 39.74 8.666"/></g></svg></div><p>Oczekiwanie na przeciwnika</p></div><form method="post" action="/challenge/MWZK8pQf/cancel" class="cancel xhr"><button type="submit" class="button button-red text" data-icon="î¿">Anuluj</button></form></div></main></div><div id="friend_box"><div class="friend_box_title"><i data-icon="î"></i> znajomych online</div><div class="content_wrap none"><div class="content list"></div></div></div><a id="reconnecting" class="link text" data-icon="îµ">Ponowne ÅÄczenie</a><svg width="0" height="0"><mask id="mask"><path fill="#fff" stroke="#fff" stroke-linejoin="round" d="M38.956.5c-3.53.418-6.452.902-9.286 2.984C5.534 1.786-.692 18.533.68 29.364 3.493 50.214 31.918 55.785 41.329 41.7c-7.444 7.696-19.276 8.752-28.323 3.084C3.959 39.116-.506 27.392 4.683 17.567 9.873 7.742 18.996 4.535 29.03 6.405c2.43-1.418 5.225-3.22 7.655-3.187l-1.694 4.86 12.752 21.37c-.439 5.654-5.459 6.112-5.459 6.112-.574-1.47-1.634-2.942-4.842-6.036-3.207-3.094-17.465-10.177-15.788-16.207-2.001 6.967 10.311 14.152 14.04 17.663 3.73 3.51 5.426 6.04 5.795 6.756 0 0 9.392-2.504 7.838-8.927L37.4 7.171z"/></mask></svg><script defer="defer" src="https://lichess1.org/assets/_YpE1iP/javascripts/vendor/chessground.min.js"></script><script defer="defer" src="https://lichess1.org/assets/_YpE1iP/javascripts/fipr.js"></script><script nonce="o2C5RR55d545d1zIwBCtPfQQ">lichess={load:new Promise(r=>document.addEventListener("DOMContentLoaded",r)),quantity:o=>{const e=o%100,t=o%10;return 1==o?"one":t>=2&&t<=4&&!(e>=12&&e<=14)?"few":"other"},siteI18n:{"pause":"Pauza","resume":"Wzn\u00f3w","nbFriendsOnline:one":"%s znajomy online","nbFriendsOnline:few":"%s znajomych online","nbFriendsOnline:many":"%s znajomych online","nbFriendsOnline":"%s znajomych online","justNow":"w\u0142a\u015bnie teraz","inNbSeconds:one":"za %s sekund\u0119","inNbSeconds:few":"za %s sekundy","inNbSeconds:many":"za %s sekund","inNbSeconds":"za %s sekund","inNbMinutes:one":"za %s minut\u0119","inNbMinutes:few":"za %s minuty","inNbMinutes:many":"za %s minuty","inNbMinutes":"za %s minut","inNbHours:one":"za %s godzin\u0119","inNbHours:few":"za %s godziny","inNbHours:many":"za %s godzin","inNbHours":"za %s godzin","inNbDays:one":"za %s dzie\u0144","inNbDays:few":"za %s dni","inNbDays:many":"za %s dni","inNbDays":"za %s dni","inNbWeeks:one":"za %s tydzie\u0144","inNbWeeks:few":"za %s tygodnie","inNbWeeks:many":"za %s tygodni","inNbWeeks":"za %s tygodni","inNbMonths:one":"za %s miesi\u0105c","inNbMonths:few":"za %s miesi\u0105ce","inNbMonths:many":"za %s miesi\u0119cy","inNbMonths":"za %s miesi\u0119cy","inNbYears:one":"za %s rok","inNbYears:few":"za %s lata","inNbYears:many":"za %s lat","inNbYears":"za %s lat","rightNow":"w tej chwili","nbSecondsAgo:one":"%s sekund\u0119 temu","nbSecondsAgo:few":"%s sekundy temu","nbSecondsAgo:many":"%s sekund temu","nbSecondsAgo":"%s sekund temu","nbMinutesAgo:one":"%s minut\u0119 temu","nbMinutesAgo:few":"%s minuty temu","nbMinutesAgo:many":"%s minut temu","nbMinutesAgo":"%s minut temu","nbHoursAgo:one":"%s godzin\u0119 temu","nbHoursAgo:few":"%s godziny temu","nbHoursAgo:many":"%s godzin temu","nbHoursAgo":"%s godzin temu","nbDaysAgo:one":"%s dzie\u0144 temu","nbDaysAgo:few":"%s dni temu","nbDaysAgo:many":"%s dni temu","nbDaysAgo":"%s dni temu","nbWeeksAgo:one":"%s tydzie\u0144 temu","nbWeeksAgo:few":"%s tygodnie temu","nbWeeksAgo:many":"%s tygodni temu","nbWeeksAgo":"%s tygodni temu","nbMonthsAgo:one":"%s miesi\u0105c temu","nbMonthsAgo:few":"%s miesi\u0105ce temu","nbMonthsAgo:many":"%s miesi\u0119cy temu","nbMonthsAgo":"%s miesi\u0119cy temu","nbYearsAgo:one":"%s rok temu","nbYearsAgo:few":"%s lata temu","nbYearsAgo:many":"%s lat temu","nbYearsAgo":"%s lat temu"}}</script><script defer="defer" src="https://lichess1.org/assets/_YpE1iP/compiled/lichess.min.js"></script><script defer="defer" src="https://lichess1.org/assets/_YpE1iP/compiled/challengePage.min.js"></script><script nonce="o2C5RR55d545d1zIwBCtPfQQ">lichess.load.then(()=>{challengePageStart({"socketUrl":"/challenge/MWZK8pQf/socket/v6","xhrUrl":"/challenge/MWZK8pQf","owner":true,"data":{"challenge":{"id":"MWZK8pQf","url":"https://lichess.org/MWZK8pQf","status":"created","challenger":{"id":"piekoszekdev","name":"piekoszekdev","title":null,"rating":869,"online":true},"destUser":{"id":"piekaa","name":"piekaa","title":null,"rating":1030,"online":true},"variant":{"key":"standard","name":"Standard","short":"Std"},"rated":false,"speed":"bullet","timeControl":{"type":"clock","limit":45,"increment":1,"show":"\u00be+1"},"color":"white","finalColor":"white","perf":{"icon":"\ue047","name":"Bullet"},"direction":"out"},"socketVersion":2}})})</script></body></html>

gra sie zaczela
<!DOCTYPE html><html lang="pl-PL"><!-- Lichess is open source! See https://lichess.org/source --><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1,viewport-fit=cover"><meta http-equiv="Content-Security-Policy" content="default-src 'self' lichess1.org; connect-src wss://0.peerjs.com 'self' lichess1.org wss://socket0.lichess.org wss://socket1.lichess.org wss://socket2.lichess.org wss://socket3.lichess.org wss://socket4.lichess.org wss://socket5.lichess.org https://explorer.lichess.ovh https://tablebase.lichess.ovh; style-src 'self' 'unsafe-inline' lichess1.org; frame-src 'self' lichess1.org www.youtube.com player.twitch.tv; worker-src 'self' lichess1.org; img-src data: *; script-src 'nonce-u44b9gCT0A4cDPgdnNEsY7P2' 'self' lichess1.org; font-src 'self' lichess1.org; base-uri 'none';"><meta name="theme-color" content="#2e2a24"><title>Zagraj piekaa â¢ lichess.org</title><link href="https://lichess1.org/assets/_YpE1iP/css/site.ltr.dark.min.css" rel="stylesheet" /><link href="https://lichess1.org/assets/_YpE1iP/css/round.ltr.dark.min.css" rel="stylesheet" /><link id="piece-sprite" href="https://lichess1.org/assets/_YpE1iP/piece-css/cburnett.css" rel="stylesheet" /><meta content="piekoszekdev (869) is playing piekaa (1030) in a casual Bullet (Â¾+1) game of chess. Game is still ongoing after 0 moves. Click to replay, analyse, and discuss the game!" name="description" /><link rel="mask-icon" href="https://lichess1.org/assets/_YpE1iP/logo/lichess.svg" color="black" /><link rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-512.png" sizes="512x512"><link rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-256.png" sizes="256x256"><link rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-192.png" sizes="192x192"><link rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-128.png" sizes="128x128"><link rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-64.png" sizes="64x64"><link id="favicon" rel="icon" type="image/png" href="https://lichess1.org/assets/_PrEUKy/logo/lichess-favicon-32.png" sizes="32x32"><meta content="noindex, nofollow" name="robots"><meta name="google" content="notranslate"><meta property="og:title" content="Bullet Chess â¢ piekoszekdev vs piekaa" /><meta property="og:description" content="piekoszekdev (869) is playing piekaa (1030) in a casual Bullet (Â¾+1) game of chess. Game is still ongoing after 0 moves. Click to replay, analyse, and discuss the game!" /><meta property="og:url" content="https://lichess.org/MWZK8pQf" /><meta property="og:type" content="website" /><meta property="og:site_name" content="lichess.org" /><meta property="og:image" content="https://lichess1.org/game/export/gif/thumbnail/MWZK8pQf.gif" /><meta name="twitter:card" content="summary" /><meta name="twitter:title" content="Bullet Chess â¢ piekoszekdev vs piekaa" /><meta name="twitter:description" content="piekoszekdev (869) is playing piekaa (1030) in a casual Bullet (Â¾+1) game of chess. Game is still ongoing after 0 moves. Click to replay, analyse, and discuss the game!" /><meta name="twitter:image" content="https://lichess1.org/game/export/gif/thumbnail/MWZK8pQf.gif" /><link href="/blog.atom" title="Blog" type="application/atom+xml" rel="alternate" /><link rel="preload" href="https://lichess1.org/assets/_YpE1iP/font/lichess.woff2" as="font" type="font/woff2" crossorigin><link rel="preload" href="https://lichess1.org/assets/_YpE1iP/font/noto-sans-v14-latin-regular.woff2" as="font" type="font/woff2" crossorigin><link rel="preload" href="https://lichess1.org/assets/_YpE1iP/font/lichess.chess.woff2" as="font" type="font/woff2" crossorigin><link rel="preload" href="https://lichess1.org/assets/_YpE1iP/images/board/svg/brown.svg" as="image" ><link rel="manifest" href="/manifest.json"><meta name="twitter:site" content="@lichess"><link rel="jslicense" href="/source"></head><body class="dark brown Woodi Basic coords-in playing fixed-scroll zenable" data-vapid="BGr5CL0QlEYa7qW7HLqe7DFkCeTsYMLsi1Db+5Vwt1QBIs6+WxN8066AjtP8S9u+w+CbleE8xWY+qQaNEMs7sAs=" data-user="piekoszekdev" data-sound-set="standard" data-socket-domains="socket0.lichess.org,socket1.lichess.org,socket2.lichess.org,socket3.lichess.org,socket4.lichess.org,socket5.lichess.org" data-asset-url="https://lichess1.org" data-asset-version="YpE1iP" data-theme="dark" data-board-theme="brown" data-piece-set="cburnett" style="--zoom:85"><form id="blind-mode" action="/toggle-blind-mode" method="POST"><input type="hidden" name="enable" value="1"><input type="hidden" name="redirect" value="/MWZK8pQf"><button type="submit">Accessibility: Enable blind mode</button></form>
<div id="zenzone"><a href="/" class="zen-home"></a><a data-icon="î¸" id="zentog" class="text fbt active">Tryb Zen</a>
</div><header id="top"><div class="site-title-nav">
<input type="checkbox" id="tn-tg" class="topnav-toggle fullscreen-toggle" autocomplete="off" aria-label="Navigation">
<label for="tn-tg" class="fullscreen-mask"></label>
<label for="tn-tg" class="hbg"><span class="hbg__in"></span></label><h1 class="site-title"><a href="/">lichess<span>.org</span></a></h1><nav id="topnav" class="hover"><section><a href="/"><span class="play">Zagraj</span><span class="home">lichess.org</span></a><div role="group"><a href="/?any#hook">Nowa partia</a><a href="/tournament">Areny</a><a href="/swiss">System szwajcarski</a><a href="/simul">Symultany</a></div></section><section><a href="/training">Zadania szachowe</a><div role="group"><a href="/training">Zadania szachowe</a><a href="/training/dashboard/30">Tablica z zadaniami</a><a href="/streak">Puzzle Streak</a><a href="/storm">Puzzle Storm</a><a href="/racer">Puzzle Racer</a></div></section><section><a href="/practice">Nauka</a><div role="group"><a href="/learn">Podstawy gry</a><a href="/practice">Äwiczenia</a><a href="/training/coordinate">WspÃ³ÅrzÄdne pÃ³l</a><a href="/study">Opracowania</a><a href="/coach">Trenerzy</a></div></section><section><a href="/tv">OglÄdaj</a><div role="group"><a href="/tv">Lichess TV</a><a href="/games">Partie w toku</a><a href="/streamer">Streamerzy</a><a href="/broadcast">Transmisje</a><a href="/video">Wideoteka</a></div></section><section><a href="/player">SpoÅecznoÅÄ</a><div role="group"><a href="/player">Gracze</a><a href="/team">Kluby</a><a href="/forum">Forum</a><a href="/blog/community">Blog</a></div></section><section><a href="/analysis">NarzÄdzia</a><div role="group"><a href="/analysis">Analiza partii</a><a href="/opening">Otwarcia</a><a href="/editor">Edytor pozycji</a><a href="/paste">Importuj partiÄ</a><a href="/games/search">Wyszukiwanie zaawansowane</a></div></section></nav></div><div class="site-buttons"><div id="clinput"><a class="link"><span data-icon="î¬"></span></a><input spellcheck="false" autocomplete="false" aria-label="Szukaj" placeholder="Szukaj" /></div><div><a id="challenge-toggle" class="toggle link"><span title="Wyzwania" class="data-count" data-count="0" data-icon="î"></span></a><div id="challenge-app" class="dropdown"></div>
</div>
<div><a id="notify-toggle" class="toggle link"><span title="Powiadomienia" class="data-count" data-count="0" data-icon="î"></span></a><div id="notify-app" class="dropdown"></div>
</div><div class="dasher"><a id="user_tag" class="toggle link" href="/logout">piekoszekdev</a><div id="dasher_app" class="dropdown"></div></div></div></header><div id="main-wrap" class="is2d"><main class="round"><aside class="round__side"><div class="game__meta"><section><div class="game__meta__infos" data-icon="î"><div><div class="header"><div class="setup"><a class="bookmark" href="/bookmark/MWZK8pQf" title="Dodaj do ulubionych"><i data-icon="î§" class="on is3"></i><i data-icon="î¦" class="off is3"></i><span></span></a>Â¾+1 â¢ Towarzyska â¢ <span title="Partie bardzo szybkie: mniej niÅ¼ 3 minuty">Bullet</span></div><time class="timeago" datetime="2023-01-24T18:54:56.337Z">&nbsp;right now</time></div></div></div><div class="game__meta__players"><div class="player color-icon is white text"><a class="user-link ulpt" href="/@/piekoszekdev">piekoszekdev (869)</a></div><div class="player color-icon is black text"><a class="user-link ulpt" href="/@/piekaa">piekaa (1030)</a></div></div></section></div><section class="mchat"><div class="mchat__tabs"><div class="mchat__tab">&nbsp;</div></div><div class="mchat__content"></div></section></aside><div class="round__app"><div class="round__app__board main-board"><div class="cg-wrap"><cg-container><cg-board><piece class="white rook" style="top:87.5%;left:0.0%"></piece><piece class="white bishop" style="top:87.5%;left:62.5%"></piece><piece class="black knight" style="top:0.0%;left:12.5%"></piece><piece class="white pawn" style="top:75.0%;left:62.5%"></piece><piece class="white bishop" style="top:87.5%;left:25.0%"></piece><piece class="black queen" style="top:0.0%;left:37.5%"></piece><piece class="white rook" style="top:87.5%;left:87.5%"></piece><piece class="white queen" style="top:87.5%;left:37.5%"></piece><piece class="black pawn" style="top:12.5%;left:25.0%"></piece><piece class="white pawn" style="top:75.0%;left:37.5%"></piece><piece class="black pawn" style="top:12.5%;left:87.5%"></piece><piece class="black pawn" style="top:12.5%;left:37.5%"></piece><piece class="white king" style="top:87.5%;left:50.0%"></piece><piece class="white pawn" style="top:75.0%;left:25.0%"></piece><piece class="black rook" style="top:0.0%;left:0.0%"></piece><piece class="black pawn" style="top:12.5%;left:50.0%"></piece><piece class="white pawn" style="top:75.0%;left:75.0%"></piece><piece class="black bishop" style="top:0.0%;left:62.5%"></piece><piece class="white knight" style="top:87.5%;left:12.5%"></piece><piece class="white knight" style="top:87.5%;left:75.0%"></piece><piece class="black king" style="top:0.0%;left:50.0%"></piece><piece class="white pawn" style="top:75.0%;left:12.5%"></piece><piece class="black pawn" style="top:12.5%;left:62.5%"></piece><piece class="white pawn" style="top:75.0%;left:50.0%"></piece><piece class="black pawn" style="top:12.5%;left:75.0%"></piece><piece class="black pawn" style="top:12.5%;left:12.5%"></piece><piece class="black pawn" style="top:12.5%;left:0.0%"></piece><piece class="black rook" style="top:0.0%;left:87.5%"></piece><piece class="white pawn" style="top:75.0%;left:0.0%"></piece><piece class="black bishop" style="top:0.0%;left:25.0%"></piece><piece class="white pawn" style="top:75.0%;left:87.5%"></piece><piece class="black knight" style="top:0.0%;left:75.0%"></piece></cg-board></cg-container></div></div><div class="col1-rmoves-preload"></div></div><div class="round__underboard"><div class="crosstable"><povs><a href="/nPPI6oww?pov=piekoszekdev" class="glpt win">1</a><a href="/nPPI6oww?pov=piekaa" class="glpt loss">0</a></povs><povs><a href="/13XJNu84?pov=piekoszekdev" class="glpt win">1</a><a href="/13XJNu84?pov=piekaa" class="glpt loss">0</a></povs><povs><a href="/pTDA5Tid?pov=piekoszekdev" class="glpt loss">0</a><a href="/pTDA5Tid?pov=piekaa" class="glpt win">1</a></povs><povs><a href="/mq3Hobgh?pov=piekoszekdev" class="glpt loss">0</a><a href="/mq3Hobgh?pov=piekaa" class="glpt win">1</a></povs><povs><a href="/O4MnAfW7?pov=piekoszekdev" class="glpt loss">0</a><a href="/O4MnAfW7?pov=piekaa" class="glpt win">1</a></povs><povs><a href="/QEUOh6ZM?pov=piekoszekdev" class="glpt loss">0</a><a href="/QEUOh6ZM?pov=piekaa" class="glpt win">1</a></povs><povs><a href="/1yjoknzC?pov=piekoszekdev" class="glpt loss">0</a><a href="/1yjoknzC?pov=piekaa" class="glpt win">1</a></povs><povs><a href="/HvrfSYSU?pov=piekoszekdev" class="glpt loss">0</a><a href="/HvrfSYSU?pov=piekaa" class="glpt win">1</a></povs><povs><a href="/ujXKH2c0?pov=piekoszekdev" class="glpt loss">0</a><a href="/ujXKH2c0?pov=piekaa" class="glpt win">1</a></povs><povs><a href="/j6dK7osW?pov=piekoszekdev" class="glpt loss">0</a><a href="/j6dK7osW?pov=piekaa" class="glpt win">1</a></povs><povs><a href="/eQzSlRJo?pov=piekoszekdev" class="glpt loss">0</a><a href="/eQzSlRJo?pov=piekaa" class="glpt win">1</a></povs><povs><a href="/JFacC5hi?pov=piekoszekdev" class="glpt win">1</a><a href="/JFacC5hi?pov=piekaa" class="glpt loss">0</a></povs><povs><a href="/a0AJrTvy?pov=piekoszekdev" class="glpt win">1</a><a href="/a0AJrTvy?pov=piekaa" class="glpt loss">0</a></povs><povs><a href="/XNh9Fsbi?pov=piekoszekdev" class="glpt win">1</a><a href="/XNh9Fsbi?pov=piekaa" class="glpt loss">0</a></povs><povs><a href="/7y3qgjU8?pov=piekoszekdev" class="glpt win">1</a><a href="/7y3qgjU8?pov=piekaa" class="glpt loss">0</a></povs><povs><a href="/iUxFtBDt?pov=piekoszekdev" class="glpt loss">0</a><a href="/iUxFtBDt?pov=piekaa" class="glpt win">1</a></povs><povs><a href="/wjJnOyuB?pov=piekoszekdev" class="glpt win">1</a><a href="/wjJnOyuB?pov=piekaa" class="glpt loss">0</a></povs><povs><a href="/VXXVxmlg?pov=piekoszekdev" class="glpt win">1</a><a href="/VXXVxmlg?pov=piekaa" class="glpt loss">0</a></povs><povs><a href="/mcdsfxOP?pov=piekoszekdev" class="glpt win">1</a><a href="/mcdsfxOP?pov=piekaa" class="glpt loss">0</a></povs><povs><a href="/JAwi1PWo?pov=piekoszekdev" class="glpt win">1</a><a href="/JAwi1PWo?pov=piekaa" class="glpt loss">0</a></povs><div class="crosstable__users"><a class="user-link ulpt" href="/@/piekoszekdev">piekoszekdev</a><a class="user-link ulpt" href="/@/piekaa">piekaa</a></div><div class="crosstable__score force-ltr" title="Wynik wszystkich partii"><span>15</span><span>15</span></div></div></div><div class="round__underchat"><div class="chat__members none" aria-live="off" aria-relevant="additions removals text"></div></div></main></div><div id="friend_box"><div class="friend_box_title"><i data-icon="î"></i> znajomych online</div><div class="content_wrap none"><div class="content list"></div></div></div><a id="reconnecting" class="link text" data-icon="îµ">Ponowne ÅÄczenie</a><svg width="0" height="0"><mask id="mask"><path fill="#fff" stroke="#fff" stroke-linejoin="round" d="M38.956.5c-3.53.418-6.452.902-9.286 2.984C5.534 1.786-.692 18.533.68 29.364 3.493 50.214 31.918 55.785 41.329 41.7c-7.444 7.696-19.276 8.752-28.323 3.084C3.959 39.116-.506 27.392 4.683 17.567 9.873 7.742 18.996 4.535 29.03 6.405c2.43-1.418 5.225-3.22 7.655-3.187l-1.694 4.86 12.752 21.37c-.439 5.654-5.459 6.112-5.459 6.112-.574-1.47-1.634-2.942-4.842-6.036-3.207-3.094-17.465-10.177-15.788-16.207-2.001 6.967 10.311 14.152 14.04 17.663 3.73 3.51 5.426 6.04 5.795 6.756 0 0 9.392-2.504 7.838-8.927L37.4 7.171z"/></mask></svg><script defer="defer" src="https://lichess1.org/assets/_YpE1iP/javascripts/fipr.js"></script><script nonce="u44b9gCT0A4cDPgdnNEsY7P2">lichess={load:new Promise(r=>document.addEventListener("DOMContentLoaded",r)),quantity:o=>{const e=o%100,t=o%10;return 1==o?"one":t>=2&&t<=4&&!(e>=12&&e<=14)?"few":"other"},siteI18n:{"pause":"Pauza","resume":"Wzn\u00f3w","nbFriendsOnline:one":"%s znajomy online","nbFriendsOnline:few":"%s znajomych online","nbFriendsOnline:many":"%s znajomych online","nbFriendsOnline":"%s znajomych online","justNow":"w\u0142a\u015bnie teraz","inNbSeconds:one":"za %s sekund\u0119","inNbSeconds:few":"za %s sekundy","inNbSeconds:many":"za %s sekund","inNbSeconds":"za %s sekund","inNbMinutes:one":"za %s minut\u0119","inNbMinutes:few":"za %s minuty","inNbMinutes:many":"za %s minuty","inNbMinutes":"za %s minut","inNbHours:one":"za %s godzin\u0119","inNbHours:few":"za %s godziny","inNbHours:many":"za %s godzin","inNbHours":"za %s godzin","inNbDays:one":"za %s dzie\u0144","inNbDays:few":"za %s dni","inNbDays:many":"za %s dni","inNbDays":"za %s dni","inNbWeeks:one":"za %s tydzie\u0144","inNbWeeks:few":"za %s tygodnie","inNbWeeks:many":"za %s tygodni","inNbWeeks":"za %s tygodni","inNbMonths:one":"za %s miesi\u0105c","inNbMonths:few":"za %s miesi\u0105ce","inNbMonths:many":"za %s miesi\u0119cy","inNbMonths":"za %s miesi\u0119cy","inNbYears:one":"za %s rok","inNbYears:few":"za %s lata","inNbYears:many":"za %s lat","inNbYears":"za %s lat","rightNow":"w tej chwili","nbSecondsAgo:one":"%s sekund\u0119 temu","nbSecondsAgo:few":"%s sekundy temu","nbSecondsAgo:many":"%<message truncated> 




                */
                
                
            }
        }
    }

    [Listen(EventName.GameFound)]
    private void StopCoroutines(EventData eventData)
    {
        StopCoroutine(nulls);
        StopCoroutine(pings);
    }
    
    private IEnumerator SendNulls()
    {
        for (;;)
        {
            yield return new WaitForSeconds(2);
            webSocket?.Send("null");
        }
    }

    private IEnumerator SendPings()
    {
        for (;;)
        {
            webSocket?.Send("{\"t\":\"ping\"}");
            yield return new WaitForSeconds(10);
        }
    }
}