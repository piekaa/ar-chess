public class PlayerController : EventListener
{
    [Listen(EventName.ChosenSquare)]
    private void MovePiece(EventData eventData)
    {
        EventSystem.Instance.Fire(EventName.PlayerMovedPiece, new EventData((GameInfo.SelectedPiece.position+eventData.BoardSquare.name).ToUpper()));
    }
}
