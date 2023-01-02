using System;
using UnityEngine;

public enum State
{
    Initial,
    WaitingForGameToStart,
    WhiteMove,
    BlackMove,
    WhitePromotion,
    BlackPromotion,
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

    [Listen(EventName.StartPromotion)]
    private void OnPromotion(EventData eventData)
    {
        ChangeState(CurrentState == State.WhiteMove ? State.WhitePromotion : State.BlackPromotion);
    }
    
    private void ChangeState(State newState, string eventText = "")
    {
        var oldState = CurrentState;
        EventSystem.Fire(EventName.StateChange, new EventData(oldState, newState, eventText));
        
    }

    [Listen(EventName.StateChange)]
    private void LogState(EventData eventData)
    {
        CurrentState = eventData.NewState;
        Debug.Log("New state: " + eventData.NewState);
    }
}
