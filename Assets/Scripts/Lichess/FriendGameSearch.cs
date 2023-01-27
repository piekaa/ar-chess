using System.Collections;
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
                LichessHttp.PostMatchRequestAndGetLocation(eventData.TimeControl, loginData.cookie, "/setup/friend");

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

                    if (response.Contains("challenge--declined"))
                    {
                        webSocket.Disconnect();
                        webSocket = null;
                        EventSystem.Instance.Fire(EventName.ArUiFriendGameDeclined, new EventData());
                    }
                    
                }).Start();
            }
        }
    }

    [Listen(EventName.GameFound)]
    private void StopCoroutines(EventData eventData)
    {
        if (nulls != null)
        {
            StopCoroutine(nulls);
        }

        if (pings != null)
        {
            StopCoroutine(pings);
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

    private IEnumerator SendPings()
    {
        for (;;)
        {
            webSocket?.Send("{\"t\":\"ping\"}");
            yield return new WaitForSeconds(10);
        }
    }
}