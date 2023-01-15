using UnityEngine;

public class GameControllers : EventListener
{
    [SerializeField] private LoginData loginData;
    private LichessController lichessController;

    [Listen(EventName.GameFound)]
    private void Connect(EventData eventData)
    {
        new GameObject().AddComponent<PlayerController>();
        lichessController = new GameObject().AddComponent<LichessController>();
        lichessController.LoginData = loginData;
        lichessController.Connect(eventData.Text);
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