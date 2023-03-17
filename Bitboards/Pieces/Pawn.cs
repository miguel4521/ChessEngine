namespace ChessEngine.Bitboards.Pieces;

public class Pawn : Piece
{
    public Pawn(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x000000000000FF00UL : 0x00FF000000000000UL;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        return null;
    }
}