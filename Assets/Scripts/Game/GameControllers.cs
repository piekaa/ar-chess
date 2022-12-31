using System;
using Unity.Tutorials.Core.Editor;
using UnityEngine;

public class GameControllers : EventListener
{
    [SerializeField] private string mainPagePath;
    private LichessController lichessController;
    
    private void Start()
    {
        new GameObject().AddComponent<PlayerController>();
        if (!mainPagePath.IsNullOrWhiteSpace())
        {
            lichessController = new GameObject().AddComponent<LichessController>();
            lichessController.Connect(mainPagePath);    
        }
        else
        {
            EventSystem.Fire(EventName.StartGame, new EventData("both"));
        }
    }

    [Listen(EventName.StartGame)]
    private void StartGame(EventData eventData)
    {
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0));
            
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
        gameObject.transform.parent = transform;
        if (lichessController != null)
        {
            lichessController.playingBlack = false;    
        }
    }
}
