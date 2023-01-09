using UnityEngine;

[CreateAssetMenu(fileName = "DigitMaterials", menuName = "Piekoszek/DigitMaterials")]
public class DigitMaterials : ScriptableObject
{
    [SerializeField] private Material off;
    [SerializeField] private Material on;

    public Material Off => off;
    public Material On => on;
}
