using System;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    private Selectable lastSelected;

    // todo redesign
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastSelected?.Deselect();
                lastSelected = hit.collider.GetComponentInParent<Selectable>();
                lastSelected?.Select();

                var piece = lastSelected?.GetComponent<Piece>();
                if (piece != null)
                {
                    gameInfo.PieceSelected(piece);
                }

                var square = hit.collider.GetComponent<BoardSquare>();
                if (square != null && square.Selected)
                {
                    gameInfo.SquareChosen(square);
                }
            }
            else
            {
                hit.collider.GetComponentInParent<Selectable>()?.Target();
            }
        }
    }
}