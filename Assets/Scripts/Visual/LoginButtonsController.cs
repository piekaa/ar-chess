using UnityEngine;
using UnityEngine.UI;

public class LoginButtonsController : EventListener
{
    [SerializeField] private Button arImageButton;
    [SerializeField] private Button arScanWallsButton;
    [SerializeField] private Button logInButton;

    [Listen(EventName.LoggeIn)]
    private void OnLoggedIn(EventData eventData)
    {
        arImageButton.gameObject.SetActive(true);
        arScanWallsButton.gameObject.SetActive(true);
        logInButton.gameObject.SetActive(false);
    }
}