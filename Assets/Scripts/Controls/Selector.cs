using UnityEngine;
using UnityEngine.XR.ARFoundation;

public abstract class Selector : EventListener
{
    private Selectable lastSelected;

    private GameObject cameraObject;

    private int cooldown;

    private void Start()
    {
        var manager = FindObjectOfType<ARCameraManager>();
        if (manager != null)
        {
            cameraObject = manager.gameObject;
        }
        else
        {
            cameraObject = Camera.main.gameObject;    
        }
    }

    protected override void MyUpdate()
    {
        cooldown--;

        if (cooldown > 0)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, Mathf.Infinity,
                LayerMask()))
        {
            if (Input.GetMouseButtonDown(0))
            {
                //todo intellij hint
                lastSelected?.Deselect();
                lastSelected = hit.collider.GetComponentInParent<Selectable>();
                lastSelected?.Select();

                var piece = lastSelected?.GetComponent<Piece>();
                if (piece != null && !piece.Captured)
                {
                    GameInfo.SelectedPiece = piece;
                    EventSystem.Instance.Fire(EventName.SelectedPiece, new EventData(piece.gameObject));
                }

                var square = hit.collider.GetComponent<BoardSquare>();
                if (square != null && square.Selected)
                {
                    EventSystem.Instance.Fire(EventName.ChosenSquare, new EventData(square));
                }
            }
            else
            {
                hit.collider.GetComponentInParent<Selectable>()?.Target();
            }
        }
    }

    protected override void EnterActiveState()
    {
        cooldown = 10;
    }

    protected abstract int LayerMask();
}