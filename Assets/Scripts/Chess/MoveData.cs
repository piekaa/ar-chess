using System;

public delegate Tuple<int, int> NextMove(Tuple<int, int> xy); 

public class MoveData
{
    private NextMove nextMove;
    private int limit;

    public MoveData(NextMove nextMove, int limit)
    {
        this.nextMove = nextMove;
        this.limit = limit;
    }

    public NextMove NextMove => nextMove;

    public int Limit => limit;
}
