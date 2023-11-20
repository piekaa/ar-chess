using System;
using UnityEngine;

public class GameControllers : EventListener
{
    [SerializeField] private LoginData loginData;
    private LichessController lichessController;

    private void Awake()
    {
        new GameObject().AddComponent<PlayerController>();
    }

    [Listen(EventName.GameFound)]
    private void Connect(EventData eventData)
    {
        if (lichessController == null)
        {
            lichessController = new GameObject().AddComponent<LichessController>();
        }

        lichessController.LoginData = loginData;
        lichessController.Connect(eventData.Text);
    }

    [Listen(EventName.StartGame)]
    private void StartGame(EventData eventData)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (eventData.Text == "white")
        {
            CreateWhitePlayerController();
        }
        else if (eventData.Text == "black")
        {
            CreateBlackPlayerController();
        }
        else
        {
            CreateWhitePlayerController();
            CreateBlackPlayerController();
        }
    }

    private void CreateWhitePlayerController()
    {
        var gameObject = new GameObject();
        gameObject.AddComponent<WhiteSelector>();
        gameObject.AddComponent<WhitePromotionSelector>();
        gameObject.transform.parent = transform;
        if (lichessController != null)
        {
            lichessController.playingBlack = true;
        }
    }

    private void CreateBlackPlayerController()
    {
        var gameObject = new GameObject();
        gameObject.AddComponent<BlackSelector>();
        gameObject.AddComponent<BlackPromotionSelector>();
        gameObject.transform.parent = transform;
        if (lichessController != null)
        {
            lichessController.playingBlack = false;
        }
    }
}