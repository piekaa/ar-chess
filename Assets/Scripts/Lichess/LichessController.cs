using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichessController : EventListener
{
    private WebSocket webSocket;

    private Queue<WebSocketMessage> messagesBeforeStart = new();

    public bool playingBlack = true;

    private bool started = false;
    
    public void Connect(string gameId)
    {
        var cookie = Http.PostMultipartAndGetCookie("lichess.org", "/login", new Dictionary<string, string>
        {
            { "username", "piekoszekdev" },
            { "password", "piekoszek" },
            { "remember", "true" },
            { "token", "" },
        }).Trim();

        webSocket = new WebSocket("socket4.lichess.org", "lichess.org",
            "/play/" + gameId + "/v6?sri=" + gameId + "&v=0", cookie);
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

        if (!started)
        {
            var message = webSocket.DequeueMessageOrNull();
            switch (message)
            {
                case WebSocketTextMessage textMessage:
                    Debug.Log(textMessage.Text);
                    if (textMessage.Text.StartsWith("{"))
                    {
                        var lichessMessage = Piekson.FromJson<LichessMessage>(textMessage.Text);
                        if (lichessMessage.t == "crowd")
                        {
                            EventSystem.Fire(EventName.StartGame, new EventData(lichessMessage.d.white ? "white" : "black"));
                            started = true;
                            foreach (var webSocketMessage in messagesBeforeStart)
                            {
                                HandleMessage(webSocketMessage);   
                            }

                            messagesBeforeStart = new();
                        }
                        else
                        {
                            messagesBeforeStart.Enqueue(message);
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
        else
        {
            HandleMessage(webSocket.DequeueMessageOrNull());
        }
    }

    private void HandleMessage(WebSocketMessage message)
    {
        switch (message)
        {
            case WebSocketTextMessage textMessage:
                Debug.Log(textMessage.Text);

                if (textMessage.Text.StartsWith("{"))
                {
                    var lichessMessage = Piekson.FromJson<LichessMessage>(textMessage.Text);
                    if (lichessMessage.t == "move" && lichessMessage.v % 2 == (playingBlack ? 0 : 1))
                    {
                        Debug.Log(lichessMessage.d.uci);
                        EventSystem.Fire(EventName.Move, new EventData(lichessMessage.d.uci));
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
        if (StateSystem.Instance.CurrentState == State.WhiteMove && playingBlack)
        {
            webSocket.Send("{\"t\":\"move\",\"d\":{\"u\":\"" + eventData.Text.ToLower() + "\",\"a\":14}}");
        }

        if (StateSystem.Instance.CurrentState == State.BlackMove && !playingBlack)
        {
            webSocket.Send("{\"t\":\"move\",\"d\":{\"u\":\"" + eventData.Text.ToLower() + "\",\"a\":14}}");
        }
    }
}