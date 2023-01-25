using System;

public class FriendNameUI : EventListener
{
    private void Awake()
    {
        Hide();
    }


    [Listen(EventName.ArUiFriendGameSelected)]
    private void Show(EventData eventData)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Accept()
    {
        //todo name in event data
        //todo time+increment in event data
        EventSystem.Instance.Fire(EventName.ArUiFriendNameSelected, new EventData());
    }
    
    public void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    
}