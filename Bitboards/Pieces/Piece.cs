namespace ChessEngine.Bitboards.Pieces;

public abstract class Piece : Bitboard
{
    public bool IsWhite { get; set; }
    
    public abstract List<Move> GenerateMoves(Position position);
}