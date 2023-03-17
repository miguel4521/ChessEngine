namespace ChessEngine.Bitboards.Pieces;

public class Bishop : SlidingPiece
{
    public Bishop(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x0000000000000024UL : 0x2400000000000000UL;
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