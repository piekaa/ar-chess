using UnityEngine;

public class ClockButton : Selectable
{
    [SerializeField] private Animation animation;
    
    public override void Select()
    {
        Debug.Log("Selected");
        
        animation.StartAnimation(gameObject);
    }
}