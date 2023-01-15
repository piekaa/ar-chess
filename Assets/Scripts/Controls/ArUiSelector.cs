
using UnityEditor;
using UnityEditor.Experimental.GraphView;

[MyState(State.ArUi)]
public class ArUiSelector : Selector
{
    protected override int LayerMask()
    {
         return 0b1 << 11;
    }   
}
