using UnityEngine;

[CreateAssetMenu(fileName = "Materials", menuName = "Piekoszek/DynamicMaterials")]
public class DynamicMaterials : ScriptableObject
{
    [SerializeField]
    private Material hover;
    
    [SerializeField]
    private Material select;

    public Material Hover => hover;

    public Material Select => select;
}
