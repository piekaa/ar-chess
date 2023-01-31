using System.ComponentModel;
using UnityEngine;

public class Badge : EventListener
{
    [SerializeField] private LCD lcd;
    [SerializeField] private bool black;
    

    [Listen(EventName.StartGame)]
    private void DisplayName(EventData eventData)
    {
        if (black)
        {
            lcd.ToDisplay = eventData.Players.BlackPlayerName;
        }
        else
        {
            lcd.ToDisplay = eventData.Players.WhitePlayerName;   
        }
    }
    
}
