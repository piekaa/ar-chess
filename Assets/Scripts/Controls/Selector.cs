using System;
using UnityEngine;

public abstract class Selector : EventListener
{
    private Selectable lastSelected;

    private Camera camera;

    private int cooldown;
    
    private void Start()
    {
        camera = Camera.main;
    }

    protected override void MyUpdate()
    {
        cooldown--;

        if (cooldown > 0)
        {
            return;
        }
        
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity, LayerMask()))
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