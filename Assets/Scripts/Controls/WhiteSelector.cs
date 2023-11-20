
[MyState(State.WhiteMove)]
public class WhiteSelector : Selector
{
    protected override int LayerMask()
    {
         return Layers.White | Layers.Square | Layers.ArUi;
    }   
}
