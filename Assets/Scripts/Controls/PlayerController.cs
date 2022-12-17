public class PlayerController : EventListener
{
    [Listen(EventName.ChosenSquare)]
    private void MovePiece(EventData eventData)
    {
        EventSystem.Fire(EventName.Move, new EventData((GameInfo.SelectedPiece.position+eventData.BoardSquare.name).ToUpper()));
    }

    protected override void MyUpdate()
    {
        
    }
}
