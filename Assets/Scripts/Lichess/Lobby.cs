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
                            if (lichessMessage.d.@in.Count == 0 || lichessMessage.d.@in[0] == null)
                            {
                                return;
                            }

                            challengeId = lichessMessage.d.@in[0].id;
                            EventSystem.Instance.Fire(EventName.Challenged,
                                new EventData(new FriendChallenge(
                                    lichessMessage.d.@in[0].challenger.name,
                                    lichessMessage.d.@in[0].timeControl.show,
                                    lichessMessage.d.@in[0].rated
                                        ? lichessMessage.d.i18n.rated
                                        : lichessMessage.d.i18n.casual
                                )));
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

    [Listen(EventName.ChallengeDeclined)]
    private void DeclineChallenge(EventData eventData)
    {
        LichessHttp.PostDeclineChallenge(challengeId, loginData.cookie);
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