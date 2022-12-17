using UnityEngine;

public class PiecesSpawner
{
    private Pieces whitePieces;
    private Pieces blackPieces;
    private Board board;
    private PiecesPositions piecesPositions;

    public PiecesSpawner(Pieces whitePieces, Pieces blackPieces, Board board, PiecesPositions piecesPositions)
    {
        this.whitePieces = whitePieces;
        this.blackPieces = blackPieces;
        this.board = board;
        this.piecesPositions = piecesPositions;
    }

    public void SpawnPiecesAtBeginningPositions()
    {
        for (int i = 8; i < 16; i++)
        {
            instantiate(whitePieces.Pawn, i);
        }

        instantiate(whitePieces.Rook, "A1");
        instantiate(whitePieces.Rook, "H1");

        instantiateRotated180(whitePieces.Knight, "B1");
        instantiateRotated180(whitePieces.Knight, "G1");

        instantiate(whitePieces.Bishop, "C1");
        instantiate(whitePieces.Bishop, "F1");

        instantiate(whitePieces.Queen, "D1");
        instantiate(whitePieces.King, "E1");

        for (int i = 8 * 6; i < 8 * 7; i++)
        {
            instantiate(blackPieces.Pawn, i, true);
        }

        instantiate(blackPieces.Rook, "A8", true);
        instantiate(blackPieces.Rook, "H8" ,true);

        instantiate(blackPieces.Knight, "B8", true);
        instantiate(blackPieces.Knight, "G8", true);

        instantiate(blackPieces.Bishop, "C8", true);
        instantiate(blackPieces.Bishop, "F8", true);

        instantiate(blackPieces.Queen, "D8", true);
        instantiate(blackPieces.King, "E8", true);
    }

    private void instantiate(Piece piece, int index, bool black = false)
    {
        var p = Object.Instantiate(piece, board.GetPosition(index), Quaternion.identity);
        p.black = black;
        piecesPositions.AddNewPiece(p, index);
    }

    private void instantiate(Piece piece, string position, bool black = false)
    {
        var p = Object.Instantiate(piece, board.GetPosition(position), Quaternion.identity);
        p.black = black;
        piecesPositions.AddNewPiece(p, Board.PositionToIndex(position));
    }

    private void instantiateRotated180(Piece piece, string position, bool black = false)
    {
        var p = Object.Instantiate(piece, board.GetPosition(position),
            Quaternion.Euler(0, 180, 0));
        p.black = black;
        piecesPositions.AddNewPiece(p, Board.PositionToIndex(position));
    }
}