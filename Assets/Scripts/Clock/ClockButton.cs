using UnityEngine;

public class ClockButton : Selectable
{
    [SerializeField] private Animation animation;
    [SerializeField] private EventName onClickEvent;


    public override void Select()
    {
        Debug.Log("Selected");
        animation.StartAnimation(gameObject);
        EventSystem.Fire(onClickEvent, new EventData(gameObject));
    }
}