namespace ChessEngine;

public class Move
{
    public Move(int startSquare, int endSquare)
    {
        StartSquare = startSquare;
        EndSquare = endSquare;
    }

    public int StartSquare { get; }
    public int EndSquare { get; }

    public string ToChessNotation()
    {
        return SquareToChessNotation(StartSquare) + " " + SquareToChessNotation(EndSquare);
    }

    private string SquareToChessNotation(int square)
    {
        return (char)('a' + square % 8) + (8 - square / 8).ToString();
    }
}