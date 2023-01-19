[MyState(State.BlackMove)]
public class BlackSelector : Selector
{
    protected override int LayerMask()
    {
        return Layers.Black | Layers.Square | Layers.ArUi;
    }
}
