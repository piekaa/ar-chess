using UnityEngine;

[MyState(State.ArUiLoading)]
public class LoadingClock : EventListener
{
    [SerializeField] private DisplayTime time;
    [SerializeField] private DisplayTime increment;

    protected override void MyUpdate()
    {
        time.Loading = true;
        increment.Loading = true;
    }
}