using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : EventListener
{
    [SerializeField] private BoardSquare[] squares;

    [SerializeField] private GameObject FirstCaptureSpotWhite;

    [SerializeField] private GameObject FirstCaptureSpotBlack;
    
    [SerializeField][Range(0.002f, 0.2f)] private float spaceBetweenCaptured = 0.25f;

    private Dictionary<string, BoardSquare> squaresByPosition = new();

    private List<int> lastSelectedSquares = new();

    private void Awake()
    {
        for (var i = 0; i < squares.Length; i++)
        {
            squaresByPosition[squares[i].name] = squares[i];
        }
    }

    public Vector3 GetPosition(int index)
    {
        return squares[index].transform.position;
    }

    public Vector3 GetPosition(string position)
    {
        return squaresByPosition[position].transform.position;
    }

    public Vector3 CaptureSpotWhite(int n) // todo rename n
    {
        return FirstCaptureSpotWhite.transform.position + new Vector3(spaceBetweenCaptured * n, 0);
    }
    
    public Vector3 CaptureSpotBlack(int n) // todo rename n
    {
        return FirstCaptureSpotBlack.transform.position - new Vector3(spaceBetweenCaptured * n, 0);
    }

    public static int Row(int index)
    {
        var position = IndexToPosition(index);
        return position[1] - '0';
    }

    public static string IndexToPosition(int index)
    {
        var row = index / 8;
        var column = index % 8;

        return "" + (char)('A' + column) + (row + 1);
    }

    public static int PositionToIndex(string position)
    {
        var column = position[0] - 'A';
        var row = position[1] - '0' - 1;

        return row * 8 + column;
    }

    public static Tuple<int, int> IndexToXY(int index)
    {
        var position = IndexToPosition(index);
        return PositionToXY(position);
    }

    public static Tuple<int, int> PositionToXY(string position)
    {
        var column = position[0] - 'A';
        var row = position[1] - '0' - 1;
        return new Tuple<int, int>(column, row);
    }

    public static string XYToPosition(Tuple<int, int> xy)
    {
        return IndexToPosition(XYToIndex(xy));
    }
    
    public static int XYToIndex(Tuple<int, int> xy)
    {
        return xy.Item1 + xy.Item2 * 8;
    }

    public void SelectSquares(List<int> indexes)
    {
        foreach (var squareIndex in lastSelectedSquares)
        {
            if (squareIndex >= 64 || squareIndex < 0) continue;

            squares[squareIndex].Deselect();
        }

        foreach (var squareIndex in indexes)
        {
            if (squareIndex >= 64 || squareIndex < 0) continue;

            squares[squareIndex].Select();
        }

        lastSelectedSquares = indexes;
    }

    [Listen(EventName.Move)]
    public void DeselectSquares(EventData eventData)
    {
        foreach (var squareIndex in lastSelectedSquares)
        {
            if (squareIndex >= 64 || squareIndex < 0) continue;

            squares[squareIndex].Deselect();
        }
    }
}