using System.Collections.Generic;
using UnityEngine;

public class DisplayTime : EventListener
{
    [SerializeField] private DisplayDigit[] digits;

    private List<char> loadingAnimation = new() { 'a', 'b', 'c', 'd', 'e', 'f' };

    private int seconds;

    public int Seconds
    {
        get => seconds;
        set
        {
            Loading = false;
            seconds = value;
        }
    }

    public bool Loading;


    private void Update()
    {
        if (Loading)
        {
            foreach (var displayDigit in digits)
            {
                displayDigit.toDisplay =
                    loadingAnimation[((int)(Time.time * 5)) % loadingAnimation.Count];
            }
            return;
        }

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