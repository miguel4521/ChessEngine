namespace ChessEngine.Bitboards.Pieces;

public class Bishop : SlidingPiece
{
    public Bishop(bool isWhite)
    {
        IsWhite = isWhite;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>();
        int startSquare = LSB();
        Bitboard bishopAttacks =
            GetBishopAttacks(startSquare, position.GetOccupiedSquares()) & ~position.GetWhitePieces();

        Bitboard captures = bishopAttacks & (IsWhite ? position.GetBlackPieces() : position.GetWhitePieces());
        Bitboard nonCaptures = bishopAttacks & ~captures;

        while (captures != 0)
        {
            int captureSquare = captures.LSB();
            moves.Add(new Move(startSquare, captureSquare, position));
            captures ^= 1UL << captureSquare;
        }

        while (nonCaptures != 0)
        {
            int nonCaptureSquare = nonCaptures.LSB();
            moves.Add(new Move(startSquare, nonCaptureSquare, position));
            nonCaptures ^= 1UL << nonCaptureSquare;
        }

        return moves;
    }
    
    public override char GetSymbol()
    {
        return IsWhite ? 'b' : 'B';
    }
}