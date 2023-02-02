using System;
using System.Collections;
using UnityEngine;

public class Lobby : EventListener
{
    [SerializeField] private LoginData loginData;

    private WebSocket webSocket;
    private Coroutine sendNullsCoroutine;

    private string challengeId;

    [Listen(EventName.ARSpawn)]
    private void Connect(EventData eventData)
    {
        webSocket = new WebSocket("socket4.lichess.org", "lichess.org",
            "/lobby/socket/v5?sri=CVZEVKrY9Fry", loginData.cookie);
        sendNullsCoroutine = StartCoroutine(SendNulls());
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
        if (message == null)
        {
            return;
        }

        switch (message)
        {
            case WebSocketTextMessage textMessage:

                if (textMessage.Text.StartsWith("{"))
                {
                    try
                    {
                        var lichessMessage = Piekson.FromJson<LichessMessage>(textMessage.Text);

                        if (lichessMessage.t == "challenges")
                        {
                            challengeId = lichessMessage.d.@in[0].id;
                            EventSystem.Instance.Fire(EventName.Challenged,
                                new EventData(lichessMessage.d.@in[0].challenger.name));
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

    [Listen(EventName.ChallengeAccepted)]
    private void AcceptChallenge(EventData eventData)
    {
        webSocket.Disconnect();
        webSocket = null;
        StopCoroutine(sendNullsCoroutine);
        
        var location = LichessHttp.PostAcceptChallengeAndGetLocation(challengeId, loginData.cookie);
        EventSystem.Instance.Fire(EventName.GameFound, new EventData(location));
    }


    private IEnumerator SendNulls()
    {
        for (;;)
        {
            yield return new WaitForSeconds(2);
            webSocket?.Send("null");
        }
    }
}