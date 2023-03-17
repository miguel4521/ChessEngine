namespace ChessEngine.Bitboards.Pieces;

public class Rook : SlidingPiece
{
    public Rook(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x0000000000000081UL : 0x8100000000000000UL;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>();
        int startSquare = LSB();
        Bitboard rookAttacks =
            GetRookAttacks(startSquare, ~position.GetEmptySquares()) & ~position.GetWhitePieces();

        Bitboard captures = rookAttacks & (IsWhite ? position.GetBlackPieces() : position.GetWhitePieces());
        Bitboard nonCaptures = rookAttacks & ~captures;

        while (captures != 0)
        {
            int captureSquare = captures.LSB();
            moves.Add(new Move(startSquare, captureSquare));
            captures ^= 1UL << captureSquare;
        }

        while (nonCaptures != 0)
        {
            int nonCaptureSquare = nonCaptures.LSB();
            moves.Add(new Move(startSquare, nonCaptureSquare));
            nonCaptures ^= 1UL << nonCaptureSquare;
        }
        
        return moves;    
    }
}