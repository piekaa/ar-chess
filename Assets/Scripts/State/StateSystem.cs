using System;
using UnityEngine;

public enum State
{
    Initial,
    WaitingForGameToStart,
    WhiteMove,
    BlackMove
}


public class StateSystem : EventListener
{

    public static StateSystem Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    public State CurrentState
    {
        get;
        private set;
    } = State.Initial;


    private void Start()
    {
        ChangeState(State.WaitingForGameToStart);
    }

    [Listen(EventName.StartGame)]
    private void OnStart(EventData eventData)
    {
        ChangeState(State.WhiteMove);
    }

    [Listen(EventName.Move)]
    private void OnMove(EventData eventData)
    {
        ChangeState(CurrentState == State.WhiteMove ? State.BlackMove : State.WhiteMove);
    }

    private void ChangeState(State newState)
    {
        var oldState = CurrentState;
        EventSystem.Fire(EventName.StateChange, new EventData(oldState, newState));
        
    }

    [Listen(EventName.StateChange)]
    private void LogState(EventData eventData)
    {
        CurrentState = eventData.NewState;
        Debug.Log("New state: " + eventData.NewState);
    }

    protected override void MyUpdate()
    {
        
    }
}
