using UnityEngine;

public abstract class Selectable : EventListener
{
    public virtual void Target()
    {
    }

    public virtual void Select()
    {
    }

    public virtual void Deselect()
    {
    }
}