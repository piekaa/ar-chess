public class FenIndexer
{
    private char row = '8';
    private char column = (char)('A' - 1);

    public int Next()
    {
        column++;
        if (column == 'I')
        {
            column = 'A';
            row--;
        }

        return Board.PositionToIndex("" + column + "" + row);
    }
}