
using UnityEditor;
using UnityEditor.Experimental.GraphView;

[MyState(State.BlackPromotion)]
public class BlackPromotionSelector : Selector
{
    protected override int LayerMask()
    {
         return 0b1 << 10;
    }   
}
