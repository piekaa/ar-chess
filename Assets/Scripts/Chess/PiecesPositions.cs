using System.Collections.Generic;

public class PiecesPositions
{
    private Dictionary<int, Piece> piecesByIndex = new();
    private Dictionary<Piece, int> indexesByPiece = new();

    private Dictionary<int, Piece> savedPiecesByIndex = new();
    private Dictionary<Piece, int> savedIndexesByPiece = new();


    public void Save()
    {
        savedPiecesByIndex = new();
        savedIndexesByPiece = new();

        foreach (var (key, value) in piecesByIndex)
        {
            if (key == -1)
            {
                continue;
            }

            savedPiecesByIndex[key] = value;
        }

        foreach (var (key, value) in indexesByPiece)
        {
            if (value == -1)
            {
                continue;
            }

            savedIndexesByPiece[key] = value;
        }
    }

    public void Rollback()
    {
        piecesByIndex = new();
        indexesByPiece = new();

        foreach (var (key, value) in savedPiecesByIndex)
        {
            piecesByIndex[key] = value;
        }

        foreach (var (key, value) in savedIndexesByPiece)
        {
            indexesByPiece[key] = value;
        }
    }

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

    public List<Piece> GetAllPieces()
    {
        return new(indexesByPiece.Keys);
    }
}