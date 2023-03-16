namespace ChessEngine;


public class King : Piece
{
    public King(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x0000000000000010UL : 0x1000000000000000UL;
    }
    
    public override List<Move> GenerateMoves(Position position)
    {
        return null;
    }
}