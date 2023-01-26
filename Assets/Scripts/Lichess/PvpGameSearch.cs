using System;
using System.Collections;
using UnityEngine;

public class PvpGameSearch : EventListener
{
    [SerializeField] private LoginData loginData;
    private WebSocket webSocket;
    private Coroutine sendNullsCoroutine; 


    [Listen(EventName.ArUiPvpGameSelected)]
    private void SearchGame(EventData eventData)
    {
        var poolInMessage = new PoolInMessage(new PoolInPayload(eventData.TimeControl.ToString()));
        var poolInJson = Piekson.ToJson(poolInMessage);
        Debug.Log(poolInJson);

        webSocket = new WebSocket("socket4.lichess.org", "lichess.org",
            "/lobby/socket/v5?sri=CVZEVKrY9Fry", loginData.cookie);
        webSocket.Send("null");
        webSocket.Send(poolInJson);
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
                 Debug.Log(textMessage.Text);

                if (textMessage.Text.StartsWith("{"))
                {
                    try
                    {
                        var lichessMessage = Piekson.FromJson<LichessMessage>(textMessage.Text);

                        if (lichessMessage.t == "redirect")
                        {
                            EventSystem.Instance.Fire(EventName.GameFound, new EventData(lichessMessage.d.url));
                            webSocket.Disconnect();
                            StopCoroutine(sendNullsCoroutine);
                            webSocket = null;
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
    
    private IEnumerator SendNulls()
    {
        for (;;)
        {
            yield return new WaitForSeconds(2);
            webSocket?.Send("null");
        }
    }
}

public class PoolInMessage
{
    public string t = "poolIn";
    public PoolInPayload d;

    public PoolInMessage(PoolInPayload d)
    {
        this.d = d;
    }
}

public class PoolInPayload
{
    public string id;

    public PoolInPayload(string id)
    {
        this.id = id;
    }
}