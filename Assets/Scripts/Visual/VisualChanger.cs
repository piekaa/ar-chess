using UnityEngine;

public class VisualChanger : MonoBehaviour
{
    [SerializeField]
    private DynamicMaterials materials;
    
    private MeshRenderer meshRenderer;
    private Material originalMaterial;

    private bool dynamicMaterialFrame;

    private bool selected;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
    }

    private void Update()
    {
        if (selected) return;
        
        if (!dynamicMaterialFrame)
        {
            meshRenderer.material = originalMaterial;
        }
        
        dynamicMaterialFrame = false;
    }

    public void Hover()
    {
        if (selected) return;
        
        meshRenderer.material = materials.Hover;
        dynamicMaterialFrame = true;
    }

    public void Select()
    {
        meshRenderer.material = materials.Select;
        selected = true;
    }

    public void Deselect()
    {
        meshRenderer.material = originalMaterial;
        selected = false;
    }
}
