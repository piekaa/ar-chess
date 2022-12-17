using System;
using UnityEngine;

public enum State
{
    Initial,
    WhiteMove,
    BlackMove
}


public class StateSystem : EventListener
{
    public State CurrentState
    {
        get;
        private set;
    } = State.Initial;


    private void Start()
    {
        CurrentState = State.WhiteMove;
        EventSystem.Fire(EventName.StateChange, new EventData(State.WhiteMove));
    }

    [Listen(EventName.Move)]
    private void OnMove(EventData eventData)
    {
        CurrentState = CurrentState == State.WhiteMove ? State.BlackMove : State.WhiteMove;
        EventSystem.Fire(EventName.StateChange, new EventData(CurrentState));
    }

    [Listen(EventName.StateChange)]
    private void LogState(EventData eventData)
    {
        Debug.Log("New state: " + eventData.State);
    }

    protected override void MyUpdate()
    {
        
    }
}
