using UnityEngine;

[CreateAssetMenu(fileName = "Materials", menuName = "Piekoszek/DynamicMaterials")]
public class DynamicMaterials : ScriptableObject
{
    [SerializeField]
    private Material hoverBeforeSelect;
    
    [SerializeField]
    private Material hoverAfterSelect;
    
    [SerializeField]
    private Material select;
    
    [SerializeField]
    private Material check;

    public Material HoverBeforeSelect => hoverBeforeSelect;

    public Material HoverAfterSelect => hoverAfterSelect;

    public Material Select => select;

    public Material Check => check;
}
