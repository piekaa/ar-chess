using UnityEngine.UI;

public class FriendNameUI : EventListener
{
    private TimeControl timeControl;

    private void Awake()
    {
        Hide();
    }


    [Listen(EventName.ArUiFriendGameSelected)]
    private void Show(EventData eventData)
    {
        timeControl = eventData.TimeControl;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Accept()
    {
        EventSystem.Instance.Fire(EventName.ArUiFriendNameSelected,
            new EventData(timeControl, GetComponentInChildren<InputField>().text));
        Hide();
    }

    public void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}