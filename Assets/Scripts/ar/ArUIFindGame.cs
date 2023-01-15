using UnityEngine;

public class ArUIFindGame : Selectable
{
    [SerializeField] private LoginData loginData;

    public override void Select()
    {
        Debug.Log("Looking for stockfish game");

        var (status, headers) = Http.PostMultipart("lichess.org", "/setup/ai", loginData.cookie, new()
        {
            { "variant", "1" },
            { "fen", "" },
            { "timeMode", "1" },
            { "time", "5" },
            { "time_range", "9" },
            { "increment", "0" },
            { "increment_range", "0" },
            { "days", "2" },
            { "days_range", "2" },
            { "mode", "0" },
            { "ratingRange", "1168-2168" },
            { "ratingRange_range_min", "-500" },
            { "ratingRange_range_max", "500" },
            { "level", "1" },
            { "color", "white" }
        });

        var location = headers["Location"];

        Debug.Log("Status: " + status);
        Debug.Log("Location: " + location);


        Http.Get("lichess.org", location, loginData.cookie);

        //todo if status

        EventSystem.Fire(EventName.GameFound, new EventData(location));
    }
}