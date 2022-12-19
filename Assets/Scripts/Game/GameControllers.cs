using System;
using UnityEngine;

public class GameControllers : EventListener
{
    [SerializeField] private string gameId;


    private LichessController lichessController;
    
    private void Start()
    {
        lichessController = new GameObject().AddComponent<LichessController>();
        lichessController.Connect(gameId);
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
            
            var gameObject = new GameObject();
            gameObject.AddComponent<WhiteSelector>();
            gameObject.AddComponent<PlayerController>();
            gameObject.transform.parent = transform;
            lichessController.playingBlack = true;
        }
        else
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<BlackSelector>();
            gameObject.AddComponent<PlayerController>();
            gameObject.transform.parent = transform;
            lichessController.playingBlack = false;
        }
    }
    
    protected override void MyUpdate()
    {
    }
}
