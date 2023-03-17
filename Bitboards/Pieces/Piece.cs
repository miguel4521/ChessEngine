namespace ChessEngine.Bitboards.Pieces;

public abstract class Piece : Bitboard
{
    protected bool IsWhite { get; init; }
    
    public abstract List<Move> GenerateMoves(Position position);
}