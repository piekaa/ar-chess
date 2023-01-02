
using UnityEditor;
using UnityEditor.Experimental.GraphView;

[MyState(State.WhitePromotion)]
public class WhitePromotionSelector : Selector
{
    protected override int LayerMask()
    {
         return 0b1 << 9;
    }   
}
