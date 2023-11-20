using UnityEngine;

public class Badge : EventListener
{
    [SerializeField] private LCD line1;
    [SerializeField] private LCD line2;
    [SerializeField] private bool black;


    [Listen(EventName.StartGame)]
    private void DisplayName(EventData eventData)
    {
        if (black)
        {
            line1.ToDisplay = eventData.Players.BlackPlayerName;
            line2.ToDisplay = eventData.Players.BlackPlayerRating + "";
        }
        else
        {
            line1.ToDisplay = eventData.Players.WhitePlayerName;
            line2.ToDisplay = eventData.Players.WhitePlayerRating + "";
        }
    }

    [Listen(EventName.GameEnd)]
    private void DisplayWinnerOrLoser(EventData eventData)
    {
        if (black)
        {
            line2.ToDisplay = eventData.Text == "black" ? "Winner!" : "Loser";
        }
        else
        {
            line2.ToDisplay = eventData.Text == "white" ? "Winner!" : "Loser";
        }
    }

    [Listen(EventName.Challenged)]
    private void DisplayChallenge(EventData eventData)
    {
        line1.ToDisplay = eventData.FriendChallenge.ChallengerName;
        line2.ToDisplay = eventData.FriendChallenge.TimeControlString + " " +
                          eventData.FriendChallenge.ScoringTypeTranslated;
    }

    [Listen(EventName.ChallengeDeclined)]
    private void ClearDisplay(EventData eventData)
    {
        line1.ToDisplay = "";
        line2.ToDisplay = "";
    }
}