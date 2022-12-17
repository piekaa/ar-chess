using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichessController : EventListener
{
    [SerializeField] private string gameId;
    private WebSocket webSocket;

    void Start()
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


    private void Update()
    {
        var message = webSocket.DequeueMessageOrNull();
        switch (message)
        {
            case WebSocketTextMessage textMessage:
                Debug.Log(textMessage.Text);

                if (textMessage.Text.StartsWith("{"))
                {
                    var lichessMessage = Piekson.FromJson<LichessMessage>(textMessage.Text);

                    if (lichessMessage.t == "move" && lichessMessage.v % 2 == 0)
                    {
                        Debug.Log(lichessMessage.d.uci);
                        EventSystem.Fire(EventName.EnemyMove, new EventData(lichessMessage.d.uci));
                    }
                }

                break;

            case WebScoketPingMessage pingMessage:
                Debug.Log("Ping: " + string.Join(" ", pingMessage.Payload));
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

    [Listen(EventName.PlayerMove)]
    private void OnMove(EventData eventData)
    {
        webSocket.Send("{\"t\":\"move\",\"d\":{\"u\":\"" + eventData.Text + "\",\"a\":14}}");
    }
}