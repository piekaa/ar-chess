using UnityEngine;

public class Clock : EventListener
{
    [SerializeField] private DisplayTime white;
    [SerializeField] private DisplayTime black;

    private float whiteTime;
    private float blackTime;

    private int moveCount;


    [Listen(EventName.StartGame)]
    private void InitializeClock(EventData eventData)
    {
        moveCount = 0;
        SynchronizeClocks(eventData.ClockData);
    }

    [Listen(EventName.GameEnd)]
    private void StopClock(EventData eventData)
    {
        moveCount = 0;
    }

    [Listen(EventName.ClockUpdate)]
    private void UpdateClock(EventData eventData)
    {
        SynchronizeClocks(eventData.ClockData);
    }

    [Listen(EventName.Move)]
    private void IncrementMoveCount(EventData eventData)
    {
        moveCount++;
    }

    private void SynchronizeClocks(ClockData clockData)
    {
        if (clockData == null)
        {
            return;
        }

        whiteTime = clockData.white;
        blackTime = clockData.black;
        UpdateClockDisplay();
    }

    protected override void MyUpdate()
    {
        if (moveCount < 2)
        {
            return;
        }

        if (StateSystem.Instance.CurrentState is State.WhiteMove or State.WhitePromotion)
        {
            whiteTime -= Time.deltaTime;
        }

        if (StateSystem.Instance.CurrentState is State.BlackMove or State.BlackPromotion)
        {
            blackTime -= Time.deltaTime;
        }

        UpdateClockDisplay();
    }

    private void UpdateClockDisplay()
    {
        white.Seconds = (int)whiteTime;
        black.Seconds = (int)blackTime;
    }

    [Listen(EventName.ArUiSmallButtonClick)]
    private void Surrender(EventData eventData)
    {
        if (moveCount > 0)
        {
            EventSystem.Instance.Fire(EventName.Surrender, new EventData());
        }
    }
}