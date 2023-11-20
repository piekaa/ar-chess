using UnityEngine;

public class VisualChanger : MonoBehaviour
{
    [SerializeField]
    private DynamicMaterials materials;
    
    private MeshRenderer meshRenderer;
    private Material originalMaterial;

    private bool dynamicMaterialFrame;

    private bool selected;
    private bool hoverSelected;
    private bool inCheck;
    
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
    }

    private void Update()
    {
        if (selected)
        {
            if (!hoverSelected)
            {
                meshRenderer.material = materials.Select;
            }   
        }
        else
        {
            if (!dynamicMaterialFrame)
            {
                if (inCheck)
                {
                    meshRenderer.material = materials.Check;
                }
                else
                {
                    meshRenderer.material = originalMaterial;    
                }
                
            }    
        }

        hoverSelected = false;
        dynamicMaterialFrame = false;
    }

    public void HoverBeforeSelect()
    {
        if (selected) return;
        
        meshRenderer.material = materials.HoverBeforeSelect;
        dynamicMaterialFrame = true;
    }

    public void HoverAfterSelect()
    {
        if (!selected) return;

        meshRenderer.material = materials.HoverAfterSelect;
        hoverSelected = true;
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

    public void Check()
    {
        inCheck = true;
    }

    public void Uncheck()
    {
        inCheck = false;
    }
}
