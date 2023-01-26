public class LichessHttp
{
    public static string PostMatchRequestAndGetLocation(TimeControl timeControl, string cookie, string path)
    {
        //todo handle status
        var (status, headers) = Http.PostMultipart("lichess.org", path, cookie, new()
        {
            { "variant", "1" },
            { "fen", "" },
            { "timeMode", "1" },
            { "time", timeControl.TimeMinutes + "" },
            { "time_range", "9" },
            { "increment", timeControl.IncrementSeconds + "" },
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

        return headers["Location"];
    }
}