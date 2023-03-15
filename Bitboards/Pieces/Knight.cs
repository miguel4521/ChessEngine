namespace ChessEngine;

public class Knight : Piece
{
    public Knight(bool isWhite)
    {
        isWhite = isWhite;
        bits = isWhite ? 0x0000000000000042UL : 0x4200000000000000UL;
    }
    
    public override List<Move> GenerateMoves(ulong emptySquares)
    {
        return null;
    }
}