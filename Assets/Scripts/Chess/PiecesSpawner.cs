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
            Instantiate(whitePieces.Pawn, i);
        }

        Instantiate(whitePieces.Rook, "A1");
        Instantiate(whitePieces.Rook, "H1");

        InstantiateRotated180(whitePieces.Knight, "B1");
        InstantiateRotated180(whitePieces.Knight, "G1");

        Instantiate(whitePieces.Bishop, "C1");
        Instantiate(whitePieces.Bishop, "F1");

        Instantiate(whitePieces.Queen, "D1");
        Instantiate(whitePieces.King, "E1");

        for (int i = 8 * 6; i < 8 * 7; i++)
        {
            Instantiate(blackPieces.Pawn, i, true);
        }

        Instantiate(blackPieces.Rook, "A8", true);
        Instantiate(blackPieces.Rook, "H8" ,true);

        Instantiate(blackPieces.Knight, "B8", true);
        Instantiate(blackPieces.Knight, "G8", true);

        Instantiate(blackPieces.Bishop, "C8", true);
        Instantiate(blackPieces.Bishop, "F8", true);

        Instantiate(blackPieces.Queen, "D8", true);
        Instantiate(blackPieces.King, "E8", true);
    }

    public void SpawnPiece(string position, char pieceType, bool black)
    {
        var piecesDeck = black ? blackPieces : whitePieces;
        
        switch (pieceType)
        {
            case 'Q':
                Instantiate(piecesDeck.Queen, position, black);
                break;
            case 'R':
                Instantiate(piecesDeck.Rook, position, black);
                break;
            case 'N':
                Instantiate(piecesDeck.Knight, position, black);
                break;
            case 'B':
                Instantiate(piecesDeck.Bishop, position, black);
                break;
        }
    }
    
    private void Instantiate(Piece piece, int index, bool black = false)
    {
        Instantiate(piece, Board.IndexToPosition(index), black);
    }

    private void Instantiate(Piece piece, string position, bool black = false)
    {
        var p = Object.Instantiate(piece, board.GetPosition(position), Quaternion.identity);
        p.black = black;
        p.position = position;
        piecesPositions.AddNewPiece(p, Board.PositionToIndex(position));
    }

    private void InstantiateRotated180(Piece piece, string position, bool black = false)
    {
        var p = Object.Instantiate(piece, board.GetPosition(position),
            Quaternion.Euler(0, 180, 0));
        p.black = black;
        p.position = position;
        piecesPositions.AddNewPiece(p, Board.PositionToIndex(position));
    }
}