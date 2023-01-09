using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : Selectable
{
    public bool black;
    [HideInInspector] public string position;
    protected bool moved;

    protected int lastMoveNumber;
    protected int lastMoveBoardDistanceY;

    private float groundY;
    private bool groundYSet;


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

    public virtual void Move(Vector3 position, int moveNumber, int boardDistanceY, bool forAnalyze = false)
    {
        lastMoveNumber = moveNumber;
        lastMoveBoardDistanceY = boardDistanceY;

        if (!forAnalyze)
        {
            StartMoveAnimation(position);
        }

        moved = !forAnalyze;
    }

    protected virtual void StartMoveAnimation(Vector3 targetPosition)
    {
        StartCoroutine(MoveAnimation(targetPosition));
    }

    protected IEnumerator MoveAnimation(Vector3 targetPosition)
    {
        var startX = transform.position.x;
        var startZ = transform.position.z;

        for (float step = 0; step < 1; step += 10 * Time.deltaTime)
        {
            transform.position = new Vector3(
                Mathf.Lerp(startX, targetPosition.x, step),
                transform.position.y,
                Mathf.Lerp(startZ, targetPosition.z, step));
            yield return null;
        }

        transform.position = targetPosition;
    }

    protected IEnumerator JumpAnimation(float height)
    {
        if (!groundYSet)
        {
            groundY = transform.position.y;
            groundYSet = true;
        }

        for (float step = 0; step < 1; step += 20 * Time.deltaTime)
        {
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Lerp(groundY, height, step),
                transform.position.z);
            yield return null;
        }

        for (float step = 0; step < 1; step += 20 * Time.deltaTime)
        {
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Lerp(height, groundY, step),
                transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
    }

    [Listen(EventName.GameEnd)]
    protected virtual void OnGameEnd(EventData eventData)
    {
        gameObject.AddComponent<Rigidbody>();
    }
}