using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    Initial,
    WhiteMove,
    BlackMove,
    WhitePromotion,
    BlackPromotion,
    ArUi,
    ArUiLoading,
}


public class StateSystem : EventListener
{
    private static StateSystem instance;

    public static StateSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StateSystem>();
            }

            return instance;
        }
    }

    public State CurrentState { get; private set; } = State.Initial;

    [Listen(EventName.ARSpawn)]
    private void StartArUi(EventData eventData)
    {
        ChangeState(State.ArUi);
    }
    
    [Listen(EventName.ArUiFriendGameDeclined)]
    private void BackToArUiIfDeclined(EventData eventData)
    {
        ChangeState(State.ArUi);
    }

    [Listen(EventName.StartGame)]
    private void OnStart(EventData eventData)
    {
        ChangeState(eventData.Fen.whoseMove == Color.White ? State.WhiteMove : State.BlackMove);
    }

    [Listen(EventName.GameEnd)]
    private void OnEnd(EventData eventData)
    {
        ChangeState(State.ArUi);
    }

    [Listen(EventName.Move)]
    private void OnMove(EventData eventData)
    {
        ChangeState(IsWhiteTurn() ? State.BlackMove : State.WhiteMove);
    }

    public bool IsWhiteTurn()
    {
        return CurrentState is State.WhiteMove or State.WhitePromotion;
    }

    public bool IsBlackTurn()
    {
        return CurrentState is State.BlackMove or State.BlackPromotion;
    }

    [Listen(EventName.StartPromotion)]
    private void OnPromotion(EventData eventData)
    {
        ChangeState(CurrentState == State.WhiteMove ? State.WhitePromotion : State.BlackPromotion);
    }

    [Listen(EventName.ArUiPvpGameSelected)]
    private void StartLoadingOnPvp(EventData eventData)
    {
        ChangeState(State.ArUiLoading);
    }

    [Listen(EventName.ArUiStockfishGameSelected)]
    private void StartLoadingOnStockfish(EventData eventData)
    {
        ChangeState(State.ArUiLoading);
    }
    
    [Listen(EventName.ArUiFriendNameSelected)]
    private void StartLoadingOnFriend(EventData eventData)
    {
        ChangeState(State.ArUiLoading);
    }

    private void ChangeState(State newState, string eventText = "")
    {
        var oldState = CurrentState;
        EventSystem.Instance.Fire(EventName.StateChange, new EventData(oldState, newState, eventText));
    }

    [Listen(EventName.StateChange)]
    private void LogState(EventData eventData)
    {
        CurrentState = eventData.NewState;
        Debug.Log("New state: " + eventData.NewState);
    }
}