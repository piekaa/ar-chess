using System.Collections;
using System.Threading;
using UnityEngine;

public class FriendGameSearch : EventListener
{
    [SerializeField] private LoginData loginData;

    private WebSocket webSocket;

    [Listen(EventName.ArUiFriendGameSelected)]
    private void SearchGame(EventData eventData)
    {
        new Thread(() =>
        {
            Debug.Log("Setup request");
            var location = LichessHttp.PostMatchRequestAndGetLocation(eventData.Text, loginData.cookie, "/setup/friend");

            Debug.Log("Location: " + location);
            
            var (status, _) = Http.PostMultipart("lichess.org", $"/challenge{location}/to-friend", loginData.cookie, new()
            {
                { "username", "piekaa" }
            });
            
            webSocket = new WebSocket("socket4.lichess.org", "lichess.org",
                $"/challenge{location}/socket/v6?sri=CVZEVKrY9Fry&v=1", loginData.cookie);

            Debug.Log("To fiend status");
            
            
        }).Start();
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

        Debug.Log(message);
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