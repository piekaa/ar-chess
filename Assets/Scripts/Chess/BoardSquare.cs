using UnityEngine;

public class BoardSquare : MonoBehaviour
{
    private VisualChanger visualChanger;

    private bool selected;
    
    private void Awake()
    {
        visualChanger = GetComponent<VisualChanger>();
    }
    
    public void Select()
    {
        visualChanger.Select();
        selected = true;
    }

    public void Deselect()
    {
        visualChanger.Deselect();
        selected = false;
    }

    public bool Selected => selected;
}