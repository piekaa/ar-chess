using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LichessController : EventListener
{
    private WebSocket webSocket;
    public bool playingBlack = true;


    public void Connect(string mainPagePath)
    {
        
        var cookie = Http.PostMultipartAndGetCookie("lichess.org", "/login", new Dictionary<string, string>
        {
            { "username", "piekoszekdev" },
            { "password", "piekoszek" },
            { "remember", "true" },
            { "token", "" },
        }).Trim();
        
        var initialHtml = Http.Get("lichess.org", mainPagePath, cookie);

        var jsonStart = initialHtml.IndexOf("{LichessRound.boot(") + "{LichessRound.boot(".Length;
        var jsonEnd = initialHtml.IndexOf("})</script></body></html>") - 1;
        
        
        Debug.Log(initialHtml.Substring(initialHtml.Length / 2));

        var json = initialHtml.Substring(jsonStart, jsonEnd - jsonStart);

        Debug.Log(json);

        // var initialData = Piekson.FromJson<InitialData>("{\n  \"data\": {\n    \"game\": {\n      \"id\": \"peQRU5Vo\",\n      \"variant\": {\n        \"key\": \"standard\",\n        \"name\": \"Standard\",\n        \"short\": \"Std\"\n      },\n      \"speed\": \"correspondence\",\n      \"perf\": \"correspondence\",\n      \"rated\": true,\n      \"fen\": \"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\",\n      \"player\": \"white\",\n      \"turns\": 0,\n      \"startedAtTurn\": 0,\n      \"source\": \"friend\",\n      \"status\": {\n        \"id\": 20,\n        \"name\": \"started\"\n      },\n      \"createdAt\": 1672338617519\n    },\n    \"player\": {\n      \"color\": \"black\",\n      \"user\": {\n        \"id\": \"piekoszekdev\",\n        \"username\": \"piekoszekdev\",\n        \"perfs\": {\n          \"correspondence\": {\n            \"games\": 2,\n            \"rating\": 1744,\n            \"rd\": 308,\n            \"prog\": 0,\n            \"prov\": true\n          }\n        }\n      },\n      \"rating\": 1744,\n      \"provisional\": true,\n      \"id\": \"EE9l\",\n      \"version\": 0\n    },\n    \"opponent\": {\n      \"color\": \"white\",\n      \"user\": {\n        \"id\": \"piekaa\",\n        \"username\": \"piekaa\",\n        \"perfs\": {\n          \"correspondence\": {\n            \"games\": 8,\n            \"rating\": 1628,\n            \"rd\": 210,\n            \"prog\": 0,\n            \"prov\": true\n          }\n        },\n        \"online\": true\n      },\n      \"rating\": 1628,\n      \"provisional\": true,\n      \"onGame\": true\n    },\n    \"url\": {\n      \"socket\": \"/play/peQRU5VoEE9l/v6\",\n      \"round\": \"/peQRU5VoEE9l\"\n    },\n    \"pref\": {\n      \"animationDuration\": 300.0,\n      \"coords\": 1,\n      \"resizeHandle\": 1,\n      \"replay\": 2,\n      \"autoQueen\": 2,\n      \"clockTenths\": 1,\n      \"moveEvent\": 2,\n      \"clockBar\": true,\n      \"clockSound\": true,\n      \"confirmResign\": true,\n      \"rookCastle\": true,\n      \"highlight\": true,\n      \"destination\": true,\n      \"enablePremove\": true,\n      \"showCaptured\": true\n    },\n    \"takebackable\": true,\n    \"moretimeable\": true,\n    \"steps\": [\n      {\n        \"ply\": 0,\n        \"fen\": \"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\"\n      }\n    ]\n  },\n  \"i18n\": {\n    \"anonymous\": \"Anonimowy\",\n    \"flipBoard\": \"Obr\\u00f3\\u0107 szachownic\\u0119\",\n    \"aiNameLevelAiLevel\": \"%1$s poziom %2$s\",\n    \"yourTurn\": \"Tw\\u00f3j ruch\",\n    \"abortGame\": \"Przerwij parti\\u0119\",\n    \"proposeATakeback\": \"Popro\\u015b o cofni\\u0119cie ruchu\",\n    \"offerDraw\": \"Zaproponuj remis\",\n    \"resign\": \"Poddaj si\\u0119\",\n    \"opponentLeftCounter:one\": \"Przeciwnik porzuci\\u0142 parti\\u0119. Mo\\u017cesz og\\u0142osi\\u0107 wygran\\u0105 za %s sekund\\u0119.\",\n    \"opponentLeftCounter:few\": \"Przeciwnik porzuci\\u0142 parti\\u0119. Mo\\u017cesz og\\u0142osi\\u0107 wygran\\u0105 za %s sekundy.\",\n    \"opponentLeftCounter:many\": \"Przeciwnik porzuci\\u0142 parti\\u0119. Mo\\u017cesz og\\u0142osi\\u0107 wygran\\u0105 za %s sekund.\",\n    \"opponentLeftCounter\": \"Przeciwnik porzuci\\u0142 parti\\u0119. Mo\\u017cesz og\\u0142osi\\u0107 wygran\\u0105 za %s sekund.\",\n    \"opponentLeftChoices\": \"Przeciwnik opu\\u015bci\\u0142 gr\\u0119. Mo\\u017cesz og\\u0142osi\\u0107 swoje zwyci\\u0119stwo, uzna\\u0107 parti\\u0119 za nierozstrzygni\\u0119t\\u0105 lub poczeka\\u0107.\",\n    \"forceResignation\": \"Og\\u0142o\\u015b wygran\\u0105\",\n    \"forceDraw\": \"Og\\u0142o\\u015b remis\",\n    \"threefoldRepetition\": \"Trzykrotne powt\\u00f3rzenie pozycji\",\n    \"claimADraw\": \"Og\\u0142o\\u015b remis\",\n    \"drawOfferSent\": \"Wys\\u0142ano propozycj\\u0119 remisu\",\n    \"cancel\": \"Anuluj\",\n    \"yourOpponentOffersADraw\": \"Przeciwnik proponuje remis\",\n    \"accept\": \"Przyjmij\",\n    \"decline\": \"Odrzu\\u0107\",\n    \"takebackPropositionSent\": \"Wys\\u0142ano pro\\u015bb\\u0119 o cofni\\u0119cie ruchu\",\n    \"yourOpponentProposesATakeback\": \"Przeciwnik chce cofn\\u0105\\u0107 ruch\",\n    \"thisAccountViolatedTos\": \"U\\u017cytkownik tego konta naruszy\\u0142 warunki korzystania z Lichess\",\n    \"gameAborted\": \"Gra zosta\\u0142a przerwana\",\n    \"checkmate\": \"Mat\",\n    \"cheatDetected\": \"Wykryto oszustwo\",\n    \"whiteResigned\": \"Bia\\u0142e podda\\u0142y si\\u0119\",\n    \"blackResigned\": \"Czarne podda\\u0142y si\\u0119\",\n    \"whiteDidntMove\": \"Bia\\u0142e nie ruszy\\u0142y si\\u0119\",\n    \"blackDidntMove\": \"Czarne nie ruszy\\u0142y si\\u0119\",\n    \"stalemate\": \"Pat\",\n    \"whiteLeftTheGame\": \"Bia\\u0142e porzuci\\u0142y parti\\u0119\",\n    \"blackLeftTheGame\": \"Czarne porzuci\\u0142y parti\\u0119\",\n    \"draw\": \"Remis\",\n    \"whiteTimeOut\": \"Up\\u0142yn\\u0105\\u0142 czas bia\\u0142ych\",\n    \"blackTimeOut\": \"Up\\u0142yn\\u0105\\u0142 czas czarnych\",\n    \"whiteIsVictorious\": \"Zwyci\\u0119stwo bia\\u0142ych\",\n    \"blackIsVictorious\": \"Zwyci\\u0119stwo czarnych\",\n    \"drawByMutualAgreement\": \"Remis za obop\\u00f3ln\\u0105 zgod\\u0105\",\n    \"fiftyMovesWithoutProgress\": \"50 posuni\\u0119\\u0107 bez bicia i ruchu pionem\",\n    \"insufficientMaterial\": \"Niewystarczaj\\u0105cy materia\\u0142\",\n    \"withdraw\": \"Wycofaj si\\u0119\",\n    \"rematch\": \"Rewan\\u017c\",\n    \"rematchOfferSent\": \"Wys\\u0142ano propozycj\\u0119 rewan\\u017cu\",\n    \"rematchOfferAccepted\": \"Przyj\\u0119to propozycj\\u0119 rewan\\u017cu\",\n    \"waitingForOpponent\": \"Oczekiwanie na przeciwnika\",\n    \"cancelRematchOffer\": \"Wycofaj propozycj\\u0119 rewan\\u017cu\",\n    \"newOpponent\": \"Nowy przeciwnik\",\n    \"confirmMove\": \"Potwierd\\u017a ruch\",\n    \"viewRematch\": \"Zobacz rewan\\u017c\",\n    \"whitePlays\": \"Ruch bia\\u0142ych\",\n    \"blackPlays\": \"Ruch czarnych\",\n    \"giveNbSeconds:one\": \"Dodaj %s sekund\\u0119\",\n    \"giveNbSeconds:few\": \"Dodaj %s sekundy\",\n    \"giveNbSeconds:many\": \"Dodaj %s sekund\",\n    \"giveNbSeconds\": \"Dodaj %s sekund\",\n    \"giveMoreTime\": \"Dodaj wi\\u0119cej czasu\",\n    \"gameOver\": \"Partia zako\\u0144czona\",\n    \"analysis\": \"Analiza partii\",\n    \"yourOpponentWantsToPlayANewGameWithYou\": \"Przeciwnik chce zagra\\u0107 z Tob\\u0105 now\\u0105 parti\\u0119\",\n    \"youPlayTheWhitePieces\": \"Grasz bia\\u0142ymi\",\n    \"youPlayTheBlackPieces\": \"Grasz czarnymi\",\n    \"itsYourTurn\": \"Tw\\u00f3j ruch!\",\n    \"oneDay\": \"1 dzie\\u0144\",\n    \"nbDays:one\": \"%s dzie\\u0144\",\n    \"nbDays:few\": \"%s dni\",\n    \"nbDays:many\": \"%s dni\",\n    \"nbDays\": \"%s dni\",\n    \"nbHours:one\": \"godzina\",\n    \"nbHours:few\": \"%s godziny\",\n    \"nbHours:many\": \"%s godzin\",\n    \"nbHours\": \"%s godzin\"\n  },\n  \"userId\": \"piekoszekdev\",\n  \"chat\": {\n    \"data\": {\n      \"id\": \"peQRU5Vo\",\n      \"name\": \"Czat\",\n      \"lines\": [],\n      \"userId\": \"piekoszekdev\",\n      \"resourceId\": \"game/peQRU5Vo\",\n      \"palantir\": true\n    },\n    \"i18n\": {\n      \"talkInChat\": \"B\\u0105d\\u017a mi\\u0142y na czacie\",\n      \"toggleTheChat\": \"Poka\\u017c/ukryj rozmowy\",\n      \"loginToChat\": \"Zaloguj si\\u0119, aby rozmawia\\u0107\",\n      \"youHaveBeenTimedOut\": \"Zosta\\u0142e\\u015b czasowo wyciszony.\",\n      \"notes\": \"Notatki\",\n      \"typePrivateNotesHere\": \"Pisz tutaj notatki prywatne\"\n    },\n    \"writeable\": true,\n    \"public\": false,\n    \"permissions\": {\n      \"local\": false\n    },\n    \"noteId\": \"peQRU5Vo\",\n    \"noteAge\": 98215\n  }\n}");
        
        var initialData = Piekson.FromJson<InitialData>(json);
        //
        Debug.Log(initialData.data.url.socket);
        //
        
        
        EventSystem.Fire(EventName.StartGame,
            new EventData(initialData.data.player.color == InitialDataColor.white ? "white" : "black"));

        webSocket = new WebSocket("socket4.lichess.org", "lichess.org",
        initialData.data.url.socket + "?sri=CVZEVKrY9Fry&v=0", cookie);
        StartCoroutine(SendNulls());
    }

    private void Start()
    {
        Debug.Log("start");
    }

    protected override void MyUpdate()
    {
        if (webSocket == null)
        {
            return;
        }
        HandleMessage(webSocket.DequeueMessageOrNull());
    }

    private void HandleMessage(WebSocketMessage message)
    {
        switch (message)
        {
            case WebSocketTextMessage textMessage:
                Debug.Log(textMessage.Text);

                if (textMessage.Text.StartsWith("{"))
                {
                    try
                    {
                        var lichessMessage = Piekson.FromJson<LichessMessage>(textMessage.Text);
                        if (lichessMessage.t == "move" && lichessMessage.v % 2 == (playingBlack ? 0 : 1))
                        {
                            FixCastleMove(lichessMessage);
                            Debug.Log(lichessMessage.d.uci);
                            EventSystem.Fire(EventName.Move, new EventData(lichessMessage.d.uci));
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.StackTrace);
                        Debug.Log("Piekson deserialization exception: " + e.Message +  "\n For JSON: \n" + textMessage.Text);
                    }
                }

                break;

            // todo move to websocket
            case WebScoketPingMessage pingMessage:
                // Debug.Log("Ping: " + string.Join(" ", pingMessage.Payload));
                webSocket.Send(pingMessage.Payload, 10);

                break;
        }
    }

    private void FixCastleMove(LichessMessage lichessMessage)
    {
        var line = lichessMessage.d.uci[1];
        if (lichessMessage.d.san == "O-O")
        {
            lichessMessage.d.uci = "e" + line + "g" + line;
        }

        if (lichessMessage.d.san == "O-O-O")
        {
            lichessMessage.d.uci = "e" + line + "c" + line;
        }
    }

    private IEnumerator SendNulls()
    {
        for (;;)
        {
            yield return new WaitForSeconds(2);
            webSocket.Send("null");
        }
    }

    [Listen(EventName.Move)]
    private void OnMove(EventData eventData)
    {
        if (StateSystem.Instance.IsWhiteTurn() && playingBlack)
        {
            webSocket.Send("{\"t\":\"move\",\"d\":{\"u\":\"" + eventData.Text.ToLower() + "\",\"a\":14}}");
        }

        if (StateSystem.Instance.IsWhiteTurn() && !playingBlack)
        {
            webSocket.Send("{\"t\":\"move\",\"d\":{\"u\":\"" + eventData.Text.ToLower() + "\",\"a\":14}}");
        }
    }
}