using UnityEngine.UI;

public class PasswordErrorMessage : EventListener
{
    private Text text;
    private UnityEngine.Color originalColor;

    private void Start()
    {
        text = GetComponent<Text>();
        originalColor = text.color;
        text.color = UnityEngine.Color.clear;
    }

    [Listen(EventName.Unauthorized)]
    private void ShowMessage(EventData eventData)
    {
        text.color = originalColor;
    }
    
    [Listen(EventName.LoggeIn)]
    private void HideMessage(EventData eventData)
    {
        text.color = UnityEngine.Color.clear;
    }
}