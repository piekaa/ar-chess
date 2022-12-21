using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : Selectable
{
    public bool black;
    public string position;
    protected bool moved;

    public bool Moved => moved;

    private VisualChanger visualChanger;

    private void Awake()
    {
        visualChanger = GetComponent<VisualChanger>();
    }

    public override void Target()
    {
        visualChanger.HoverBeforeSelect();
    }

    public override void Select()
    {
        visualChanger.Select();
    }

    public override void Deselect()
    {
        visualChanger.Deselect();
    }

    public abstract List<MoveData> AvailableMoves(int currentIndex);

    public virtual List<MoveData> CaptureMoves(int currentIndex)
    {
        return AvailableMoves(currentIndex);
    }
    
    public virtual List<MoveData> CastleMoves(int currentIndex)
    {
        return new();
    }
    
    public void Move(Vector3 position)
    {
        transform.position = position;
        moved = true;
    }
}