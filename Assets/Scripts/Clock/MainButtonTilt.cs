using System;
using UnityEngine;

public class MainButtonTilt : EventListener
{
    [SerializeField] private Animation BlackTilt;
    [SerializeField] private Animation WhiteTilt;

    private bool whiteMove;

    [Listen(EventName.StartGame)]
    private void Initialize(EventData eventData)
    {
        whiteMove = true;
    }
    
    [Listen(EventName.Move)]
    private void Tilt(EventData eventData)
    {
        var animation = whiteMove ? BlackTilt : WhiteTilt;
        animation.StartAnimation(gameObject);
        whiteMove = !whiteMove;
    }
    
}
