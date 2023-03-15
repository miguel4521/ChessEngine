namespace ChessEngine;

public class Rook : Piece
{
    public Rook(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x0000000000000081UL : 0x8100000000000000UL;
    }

    public override List<Move> GenerateMoves(ulong emptySquares)
    {
        return null;
    }
}