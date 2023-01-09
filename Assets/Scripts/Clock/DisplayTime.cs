using UnityEngine;

public class DisplayTime : EventListener
{
    [SerializeField] private DisplayDigit[] digits;

    public int seconds = 0;


    private void Update()
    {
        if (seconds < 0)
        {
            seconds = 0;
        }

        var minutesToDisplay = (seconds / 60).ToString("D2");
        var secondsToDisplay = (seconds % 60).ToString("D2");

        digits[0].toDisplay = minutesToDisplay[0];
        digits[1].toDisplay = minutesToDisplay[1];
        digits[2].toDisplay = secondsToDisplay[0];
        digits[3].toDisplay = secondsToDisplay[1];
    }
}