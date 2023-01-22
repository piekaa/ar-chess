using System.Threading;
using UnityEngine;

public class StockfishGameSearch : EventListener
{
    [SerializeField] private LoginData loginData;

    [Listen(EventName.ArUiStockfishGameSelected)]
    private void SearchGame(EventData eventData)
    {
        new Thread(() =>
        {
            Debug.Log("Looking for stockfish game");
            var location = LichessHttp.PostMatchRequestAndGetLocation(eventData.Text, loginData.cookie);

            // Debug.Log("Status: " + status);
            Debug.Log("Location: " + location);


            Http.Get("lichess.org", location, loginData.cookie);

            //todo if status

            EventSystem.Instance.Fire(EventName.GameFound, new EventData(location));
        }).Start();
    }
}