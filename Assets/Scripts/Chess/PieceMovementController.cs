using UnityEngine;

public class PieceMovementController : EventListener
{
    [SerializeField] private PiecesController piecesController;
    [SerializeField] private Board board;

    private BoardAnalyzer boardAnalyzer;
    private King kingInCheck;

    private string lastPromotionMove = "";

    private void Awake()
    {
        boardAnalyzer = new BoardAnalyzer(piecesController);
    }

    [Listen(EventName.SelectedPiece)]
    private void ShowAvailableMovements(EventData eventData)
    {
        var piece = eventData.GameObject.GetComponent<Piece>();

        var toPromote = piece as ToPromote;
        if (toPromote != null)
        {
            return;
        }

        board.SelectSquares(boardAnalyzer.GetAvailableMoves(piece));
    }

    [Listen(EventName.SelectedPiece)]
    private void PromotePiece(EventData eventData)
    {
        var piece = eventData.GameObject.GetComponent<Piece>();
        var toPromote = piece as ToPromote;
        if (toPromote == null)
        {
            return;
        }

        EventSystem.Fire(EventName.Move,
            new EventData((lastPromotionMove + toPromote.pieceFirstLetter).ToUpper()));
    }

    [Listen(EventName.PlayerMovedPiece)]
    private void PlayerMovedPiece(EventData eventData)
    {
        var move = eventData.Text.ToUpper();
        var firstSquare = move[..2];
        var secondSquare = move.Substring(2, 2);

        lastPromotionMove = move;

        var pawn = piecesController.GetPiece(Board.PositionToIndex(firstSquare)) as Pawn;

        if (pawn != null && secondSquare[1] is '1' or '8')
        {
            EventSystem.Fire(EventName.StartPromotion, eventData);
        }
        else
        {
            EventSystem.Fire(EventName.Move, eventData);
        }
    }

    [Listen(EventName.Move)]
    private void MovePiece(EventData eventData)
    {
        var move = eventData.Text.ToUpper();


        var firstSquare = move[..2];
        var secondSquare = move.Substring(2, 2);


        if (move.Length == 5)
        {
            var pawnToPromote = piecesController.GetPiece(Board.PositionToIndex(firstSquare));
            piecesController.CapturePiece(pawnToPromote);

            var toCapture = piecesController.GetPiece(Board.PositionToIndex(secondSquare));

            if (toCapture != null)
            {
                piecesController.CapturePiece(toCapture);
            }

            piecesController.PromoteToPiece(secondSquare, move[4], pawnToPromote.black);
            CheckChecks();
            return;
        }

        var pieceToMove = piecesController.GetPiece(Board.PositionToIndex(firstSquare));

        var king = pieceToMove as King;
        var pawn = pieceToMove as Pawn;

        if (move is "E1G1" or "E1C1" && king != null)
        {
            WhiteCastleMove(king, secondSquare);
            return;
        }

        if (move is "E8G8" or "E8C8" && king != null)
        {
            BlackCastleMove(king, secondSquare);
            return;
        }

        if (IsEnPassant(pawn, firstSquare, secondSquare))
        {
            var otherPawnPosition = "" + secondSquare[0] + firstSquare[1];
            var otherPawn = piecesController.GetPiece(Board.PositionToIndex(otherPawnPosition));
            piecesController.CapturePiece(otherPawn);
        }


        RegularMove(firstSquare, secondSquare);

        CheckChecks();
    }

    private bool IsEnPassant(Pawn pawn, string firstSquare, string secondSquare)
    {
        if (pawn == null)
        {
            return false;
        }

        if (firstSquare[0] == secondSquare[0])
        {
            return false;
        }

        return piecesController.IsFree(Board.PositionToIndex(secondSquare));
    }

    private void CheckChecks()
    {
        kingInCheck?.GetComponent<VisualChanger>().Uncheck();
        var kingsInCheck = boardAnalyzer.GetKingsInCheck();
        foreach (var king in kingsInCheck)
        {
            king.GetComponent<VisualChanger>()?.Check();
            kingInCheck = king;
        }
    }

    private void RegularMove(string firstSquare, string secondSquare)
    {
        var pieceToMove = piecesController.GetPiece(Board.PositionToIndex(firstSquare));
        var otherPiece = piecesController.GetPiece(Board.PositionToIndex(secondSquare));

        if (otherPiece)
        {
            piecesController.CapturePiece(otherPiece);
        }

        piecesController.MovePiece(pieceToMove, secondSquare);
    }

    private void WhiteCastleMove(King king, string secondSquare)
    {
        Piece rook = piecesController.GetPiece(Board.PositionToIndex("A1"));
        var rookNewPosition = "D1";
        if (secondSquare == "G1")
        {
            rook = piecesController.GetPiece(Board.PositionToIndex("H1"));
            rookNewPosition = "F1";
        }

        king.nextMoveCastle = true;
        piecesController.MovePiece(king, secondSquare);
        piecesController.MovePiece(rook, rookNewPosition);
    }

    private void BlackCastleMove(King king, string secondSquare)
    {
        Piece rook = piecesController.GetPiece(Board.PositionToIndex("A8"));
        var rookNewPosition = "D8";
        if (secondSquare == "G8")
        {
            rook = piecesController.GetPiece(Board.PositionToIndex("H8"));
            rookNewPosition = "F8";
        }

        king.nextMoveCastle = true;
        piecesController.MovePiece(king, secondSquare);
        piecesController.MovePiece(rook, rookNewPosition);
    }
}