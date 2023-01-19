using UnityEngine;

[MyState(State.BlackMove)]
public class BlackMoveButton : Selectable
{
    [SerializeField] private Animation animation;
    [SerializeField] private EventName onClickEvent;


    public override void Select()
    {
        animation.StartAnimation(gameObject);
        EventSystem.Instance.Fire(onClickEvent, new EventData(gameObject));
    }
}