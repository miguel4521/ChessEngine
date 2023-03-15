namespace ChessEngine;

public class Bishop : Piece
{
    public Bishop(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x0000000000000024UL : 0x2400000000000000UL;
    }

    public override List<Move> GenerateMoves(ulong emptySquares)
    {
        return null;
    }
}