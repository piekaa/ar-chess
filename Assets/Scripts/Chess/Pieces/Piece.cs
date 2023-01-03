﻿using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : Selectable
{
    public bool black;
    [HideInInspector]
    public string position;
    protected bool moved;

    protected int lastMoveNumber;
    protected int lastMoveBoardDistanceY;


    public int LastMoveNumber => lastMoveNumber;

    public int LastMoveBoardDistanceY => lastMoveBoardDistanceY;

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

    //todo fix, only stuck pieces are selected
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

    public virtual List<MoveData> EnPassantMoves(int currentIndex)
    {
        return new();
    }
    
    public void Move(Vector3 position, int moveNumber, int boardDistanceY, bool setMoved = true)
    {
        lastMoveNumber = moveNumber;
        lastMoveBoardDistanceY = boardDistanceY;
        transform.position = position;
        moved = setMoved;
    }
}