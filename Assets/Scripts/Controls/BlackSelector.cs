[MyState(State.BlackMove)]
public class BlackSelector : Selector
{
    protected override int LayerMask()
    {
        return 0b101 << 6;
    }
}
