namespace ChessEngine;

public class Queen : Piece
{
    public Queen(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x0000000000000008UL : 0x0800000000000000UL;
    }
    
    public override List<Move> GenerateMoves(Position position)
    {
        return null;
    }
}