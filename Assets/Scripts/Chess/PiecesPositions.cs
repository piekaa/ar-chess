using System.Collections.Generic;

public class PiecesPositions
{
    private Dictionary<int, Piece> piecesByIndex = new();
    private Dictionary<Piece, int> indexesByPiece = new();

    public void AddNewPiece(Piece piece, int index)
    {
        piecesByIndex[index] = piece;
        indexesByPiece[piece] = index;
    }

    public int GetIndex(Piece piece)
    {
        return indexesByPiece[piece];
    }

    public void SetIndex(Piece piece, int index)
    {
        var oldIndex = indexesByPiece[piece];

        piecesByIndex.Remove(oldIndex);

        indexesByPiece[piece] = index;
        piecesByIndex[index] = piece;
    }

    public Piece GetPiece(int index)
    {
        return piecesByIndex.ContainsKey(index) ? piecesByIndex[index] : null;
    }
}