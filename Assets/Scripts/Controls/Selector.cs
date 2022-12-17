using UnityEngine;

[MyState(State.WhiteMove)]
public class Selector : EventListener
{
    private Selectable lastSelected;


    protected override void MyUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (Input.GetMouseButtonDown(0))
            {
                //todo 
                lastSelected?.Deselect();
                lastSelected = hit.collider.GetComponentInParent<Selectable>();
                lastSelected?.Select();

                var piece = lastSelected?.GetComponent<Piece>();
                if (piece != null)
                {
                    GameInfo.SelectedPiece = piece;
                    EventSystem.Fire(EventName.SelectedPiece, new EventData(piece.gameObject));
                }

                var square = hit.collider.GetComponent<BoardSquare>();
                if (square != null && square.Selected)
                {
                    EventSystem.Fire(EventName.ChosenSquare, new EventData(square));
                }
            }
            else
            {
                hit.collider.GetComponentInParent<Selectable>()?.Target();
            }
        }
    }
}