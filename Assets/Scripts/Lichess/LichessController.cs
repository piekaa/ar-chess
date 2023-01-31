using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class LichessController : EventListener
{
    public LoginData LoginData;

    private WebSocket webSocket;
    public bool playingBlack = true;

    private int moveCount = 0;


    // todo after some time those moves are not sent through web socket, handle it from fen steps
    private int moveCountBeforeStart = 0;

    // todo disconnect on game end
    public void Connect(string mainPagePath)
    {
        new Thread(() =>
        {
            var initialHtml = Http.Get("lichess.org", mainPagePath, LoginData.cookie);

            var jsonStart = initialHtml.IndexOf("{LichessRound.boot(") + "{LichessRound.boot(".Length;
            var jsonEnd = initialHtml.IndexOf("})</script></body></html>") - 1;

            var json = initialHtml.Substring(jsonStart, jsonEnd - jsonStart);

            var initialData = Piekson.FromJson<InitialData>(json);

            moveCountBeforeStart = initialData.data.steps.Max(step => step.ply);


            Players players;

            if (initialData.data.player.color == InitialDataColor.white)
            {
                players = new Players(initialData.data.player.user.username, initialData.data.opponent.user.username);
            }
            else
            {
                players = new Players(initialData.data.opponent.user.username, initialData.data.player.user.username);
            }

            EventSystem.Instance.Fire(EventName.StartGame,
                new EventData(initialData.data.player.color == InitialDataColor.white ? "white" : "black",
                    initialData.data.clock, new Fen(initialData.data.steps[0].fen), players));

            webSocket = new WebSocket("socket4.lichess.org", "lichess.org",
                initialData.data.url.socket + "?sri=CVZEVKrY9Fry&v=0", LoginData.cookie);
        }).Start();

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

                if (textMessage.Text.StartsWith("{"))
                {
                    try
                    {
                        var lichessMessage = Piekson.FromJson<LichessMessage>(textMessage.Text);

                        if (lichessMessage.t == "move")
                        {
                            Debug.Log(lichessMessage.d.uci);
                            if (lichessMessage.v % 2 == (playingBlack ? 0 : 1) || moveCount < moveCountBeforeStart)
                            {
                                FixCastleMove(lichessMessage);
                                FixPromotionMove(lichessMessage);

                                EventSystem.Instance.Fire(EventName.Move, new EventData(lichessMessage.d.uci));

                                if (lichessMessage.d.clock != null)
                                {
                                    EventSystem.Instance.Fire(EventName.ClockUpdate,
                                        new EventData(lichessMessage.d.clock));
                                }
                            }

                            moveCount++;
                        }

                        if (lichessMessage.t == "endData")
                        {
                            Debug.Log(lichessMessage.d.winner);
                            EventSystem.Instance.Fire(EventName.GameEnd, new EventData(lichessMessage.d.winner));
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.StackTrace);
                        Debug.Log("Piekson deserialization exception: " + e.Message + "\n For JSON: \n" +
                                  textMessage.Text);
                    }
                }

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

    private void FixPromotionMove(LichessMessage lichessMessage)
    {
        var pieceClassToLetter = new Dictionary<string, char>()
        {
            { "queen", 'q' },
            { "rook", 'r' },
            { "knight", 'n' },
            { "bishop", 'b' },
        };

        if (lichessMessage.d.promotion != null)
        {
            lichessMessage.d.uci += pieceClassToLetter[lichessMessage.d.promotion.pieceClass];
        }
    }

    private IEnumerator SendNulls()
    {
        for (;;)
        {
            yield return new WaitForSeconds(2);
            webSocket?.Send("null");
        }
    }

    [Listen(EventName.Move)]
    private void OnMove(EventData eventData)
    {
        Debug.Log(moveCount);
        if (moveCount < moveCountBeforeStart)
        {
            return;
        }

        if (StateSystem.Instance.IsWhiteTurn() && playingBlack)
        {
            Debug.Log("Sending: " + eventData.Text.ToLower());
            webSocket.Send("{\"t\":\"move\",\"d\":{\"u\":\"" + eventData.Text.ToLower() + "\",\"a\":14}}");
        }

        if (StateSystem.Instance.IsBlackTurn() && !playingBlack)
        {
            Debug.Log("Sending: " + eventData.Text.ToLower());
            webSocket.Send("{\"t\":\"move\",\"d\":{\"u\":\"" + eventData.Text.ToLower() + "\",\"a\":14}}");
        }
    }

    [Listen(EventName.Surrender)]
    private void Surrender(EventData eventData)
    {
        webSocket.Send(Piekson.ToJson(new LichessMessage("resign")));
    }
}